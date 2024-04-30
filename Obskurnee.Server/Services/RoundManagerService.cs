using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Hubs;
using Obskurnee.Models;
using Obskurnee.Server;
using Obskurnee.ViewModels;
using System.Diagnostics;

namespace Obskurnee.Services;

public class RoundManagerService
{
    private readonly ILogger<RoundManagerService> _logger;
    private readonly ApplicationDbContext _db;
    private readonly BookService _bookService;
    private readonly NewsletterService _newsletter;
    private readonly IStringLocalizer<Strings> _localizer;
    private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
    private readonly Config _config;
    private readonly IHubContext<EventHub, IEventHub> _eventHub;

    public RoundManagerService(
        ILogger<RoundManagerService> logger,
        ApplicationDbContext database,
        BookService bookService,
        NewsletterService newsletter,
        IStringLocalizer<Strings> localizer,
        IStringLocalizer<NewsletterStrings> newsletterLocalizer,
        IHubContext<EventHub, IEventHub> eventHub,
        Config config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = database ?? throw new ArgumentNullException(nameof(database));
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        _newsletter = newsletter;
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _eventHub = eventHub ?? throw new ArgumentNullException(nameof(eventHub));
    }

    public async Task<IList<Round>> AllRounds()
        => await _db.Rounds
                    .Include(r => r.Book)
                    .ThenInclude(b => b.Post)
                    .OrderByDescending(r => r.CreatedOn)
                    .ToListAsync();

    public async Task<Round> NewRound(Topic topic, string title, string description, string ownerId)
    {
        var round = new Round(ownerId)
        {
            Kind = topic,
            Title = title,
        };
        var discussion = new Discussion(ownerId)
        {
            Description = description,
            Topic = topic,
        };

        await _db.Rounds.AddAsync(round);
        await _db.SaveChangesAsync();
        switch (topic)
        {
            case Topic.Books:
                discussion.Title = _localizer.Format("bookDiscussionTitle", title);
                discussion.RoundId = round.RoundId;
                await _db.Discussions.AddAsync(discussion);
                await _db.SaveChangesAsync();
                round.BookDiscussion = discussion;
                break;
            case Topic.Themes:
                discussion.Title = _localizer.Format("topicDiscussionTitle", title);
                discussion.RoundId = round.RoundId;
                await _db.Discussions.AddAsync(discussion);
                round.ThemeDiscussion = discussion;
                break;
            default:
                throw new Exception($"Invalid Topic: {topic}");
        }
        _db.Rounds.Update(round);
        await _db.SaveChangesAsync();
        await SendNewDiscussionNotification(discussion);
        await _eventHub.Clients.All.DiscussionChanged(discussion);
        return round;
    }

    public async Task<RoundUpdateResults> CloseDiscussion(int discussionId, string currentUserId)
    {
        var discussion = await _db.Discussions
                                .Include(d => d.Posts)
                                .Include(d => d.Round)
                                .FirstAsync(d => d.DiscussionId == discussionId);
        if (discussion.IsClosed)
        {
            throw new PermissionException(_localizer["discussionClosed"]);
        }
        if (discussion.Posts.Count < 1)
        {
            throw new PermissionException(_localizer["atLeastOneProposalRequired"]);
        }
        var round = discussion.Round;
        try
        {
            await _db.Database.BeginTransactionAsync();
            discussion.IsClosed = true;
            var poll = new Poll(currentUserId)
            {
                DiscussionId = discussion.DiscussionId,
                Title = _localizer.Format("pollTitle", discussion.Title),
                Options = discussion.Posts,
                Topic = discussion.Topic,
                RoundId = round.RoundId,
            };
            await _db.Polls.AddAsync(poll);
            await _db.SaveChangesAsync();
            discussion.PollId = poll.PollId;
            switch (discussion.Topic)
            {
                case Topic.Books:
                    round.BookPollId = poll.PollId;
                    break;
                case Topic.Themes:
                    round.ThemePollId = poll.PollId;
                    break;
            }
            _db.Discussions.Update(discussion);
            _db.Rounds.Update(round);
            await _db.SaveChangesAsync();
            await _db.Database.CommitTransactionAsync();
            await SendNewPollNotification(poll);
            await _eventHub.Clients.All.DiscussionChanged(null);
        }
        catch
        {
            _db.Database.RollbackTransaction();
            throw;
        }
        return new() { Discussion = discussion, Round = round };
    }

    public async Task<RoundUpdateResults> ClosePoll(int pollId, string currentUserId)
    {
        try
        {
            _logger.LogInformation("Closing poll {pollId}", pollId);
            await _db.Database.BeginTransactionAsync();
            RoundUpdateResults result;
            var poll = await _db.PollsWithData
                        .Include(p => p.Round)
                        .FirstAsync(p => p.PollId == pollId);
            var round = poll.Round;
            Trace.Assert(!poll.IsClosed);
            var pollResults = poll.Results;
            Trace.Assert(pollResults.Votes.Count > 0);
            poll.IsClosed = true;

            var winners = poll.FindWinningPosts();
            Trace.Assert(winners.Count > 0);

            if (winners.Count == 1
                || poll.IsTiebreaker)
            {
                pollResults.WinnerPostId = winners.OrderBy(_ => Guid.NewGuid()).First();
                Trace.Assert(pollResults.WinnerPostId != null);
                poll.Results = pollResults;
                result = new RoundUpdateResults { Poll = poll, Round = round };
                switch (poll.Topic)
                {
                    case Topic.Books:
                        result.Book = await _bookService.CreateBook(poll, round.RoundId, currentUserId);
                        round.BookId = result.Book.BookId;
                        poll.FollowupLink = new Poll.FollowupReference(Poll.LinkKind.Book, result.Book.BookId);
                        await SendNewBookNotification(result);
                        break;
                    case Topic.Themes:
                        result.Discussion = await CreateDiscussionFromTopicPoll(currentUserId, poll, round);
                        round.BookDiscussionId = result.Discussion.DiscussionId;
                        poll.FollowupLink = new Poll.FollowupReference(Poll.LinkKind.Discussion, result.Discussion.DiscussionId);
                        await SendNewDiscussionNotification(result.Discussion);
                        await _eventHub.Clients.All.DiscussionChanged(result.Discussion);
                        break;
                }
            }
            else
            {
                _logger.LogInformation("There are {count} winning posts. A tiebreaker round is needed.",
                    winners.Count);
                var posts = await (from post in _db.Posts
                                   where winners.Contains(post.PostId)
                                   orderby post.PostId
                                   select post)
                                   .ToListAsync();
                var tiebreaker = new Poll(currentUserId)
                {
                    PreviousPollId = poll.PollId,
                    DiscussionId = poll.DiscussionId,
                    Title = _localizer.Format("tiebreakerTitle", poll.Title),
                    Options = posts,
                    Topic = poll.Topic,
                    IsTiebreaker = true,
                    RoundId = round.RoundId,
                };
                await _db.Polls.AddAsync(tiebreaker);
                await _db.SaveChangesAsync();
                poll.FollowupLink = new Poll.FollowupReference(Poll.LinkKind.Poll, tiebreaker.PollId);
                switch (poll.Topic)
                {
                    case Topic.Books:
                        round.BookTiebreakerPollId = tiebreaker.PollId;
                        break;
                    case Topic.Themes:
                        round.ThemeTiebreakerPollId = tiebreaker.PollId;
                        break;
                    default:
                        throw new Exception($"Unexpected Topic: {poll.Topic}");
                }
                result = new RoundUpdateResults { Poll = poll, Round = round };
                await SendNewPollNotification(tiebreaker);
            }
            _db.Polls.Update(poll);
            _db.Rounds.Update(round);

            await _db.SaveChangesAsync();
            await _db.Database.CommitTransactionAsync();
            return result;
        }
        catch
        {
            _db.Database.RollbackTransaction();
            throw;
        }
    }

    private async Task<Discussion> CreateDiscussionFromTopicPoll(
        string currentUserId,
        Poll poll,
        Round round)
    {
        var winnerPost = _db.Posts.First(p => p.PostId == poll.Results.WinnerPostId);
        var description = $"**{winnerPost.Title}**";
        if (!string.IsNullOrWhiteSpace(winnerPost.Text))
        {
            description += $" - {winnerPost.Text}";
        }
        var bookDiscussion = new Discussion(currentUserId)
        {
            RoundId = round.RoundId,
            Topic = Topic.Books,
            Title = _localizer.Format("bookDiscussionTitle", round.Title),
            Description = description,
        };
        await _db.Discussions.AddAsync(bookDiscussion);
        await _db.SaveChangesAsync();
        return bookDiscussion;
    }

    private async Task SendNewDiscussionNotification(Discussion discussion)
    {
        var link = $"{_config.BaseUrl}/navrhy/{discussion.DiscussionId}";
        await _newsletter.SendNewsletter(
            Newsletters.BasicEvents,
            _newsletterLocalizer.Format("newRoundSubject", discussion.Title),
            _newsletterLocalizer.Format("newRoundBodyMarkdown", link)
                + (string.IsNullOrWhiteSpace(discussion.Description)
                    ? ""
                    : _newsletterLocalizer.Format("additionalDescriptionMarkdown", discussion.Description)));
    }

    private async Task SendNewPollNotification(Poll poll)
    {
        var link = $"{_config.BaseUrl}/hlasovania/{poll.PollId}";
        await _newsletter.SendNewsletter(
            Newsletters.BasicEvents,
            _newsletterLocalizer.Format("newPollSubject", poll.Title),
            _newsletterLocalizer.Format("newPollBodyMarkdown", link));
    }

    private async Task SendNewBookNotification(RoundUpdateResults result)
    {
        var link = $"{_config.BaseUrl}/knihy/{result.Book.BookId}";
        var post = _db.Posts.First(p => p.PostId == result.Book.Post.PostId);
        await _newsletter.SendNewsletter(
            Newsletters.BasicEvents,
            _newsletterLocalizer.Format("newBookSubject"),
            _newsletterLocalizer.Format("newBookVotedBodyMarkdown",
                link,
                post.Title,
                post.Author,
                post.Text.AddMarkdownQuote(),
                post.PageCount,
                post.OwnerName,
                post.Url));
    }
}
