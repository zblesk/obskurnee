﻿using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Obskurnee.Models.Round;

namespace Obskurnee.Services
{
    public class RoundManagerService
    {

        private readonly ILogger<RoundManagerService> _logger;
        private readonly Database _db;
        private readonly object @lock = new object();
        private readonly BookService _bookService;

        public RoundManagerService(
            ILogger<RoundManagerService> logger,
            Database database,
            BookService bookService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        public IList<Round> AllRounds() => _db.Rounds.Query().OrderByDescending(r => r.CreatedOn).ToList();

        public Round NewRound(Topic topic, string title, string description, string ownerId)
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

            _db.Rounds.Insert(round);
            switch (topic)
            {
                case Topic.Books:
                    discussion.Title = $"{title} - návrhy kníh";
                    discussion.RoundId = round.RoundId;
                    _db.Discussions.Insert(discussion);
                    round.BookDiscussionId = discussion.DiscussionId;
                    break;
                case Topic.Themes:
                    discussion.Title = $"{title} - návrhy tém";
                    discussion.RoundId = round.RoundId; 
                    _db.Discussions.Insert(discussion);
                    round.ThemeDiscussionId = discussion.DiscussionId;
                    break;
                default:
                    throw new Exception($"Invalid Topic: {topic}");
            }
            _db.Rounds.Update(round);
            return round;
        }

        public (Discussion, Round) CloseDiscussion(int discussionId, string currentUserId)
        {
            var discussion = _db.Discussions.FindById(discussionId);
            if (discussion.IsClosed)
            {
                throw new PermissionException("Diskusia uz bola uzavreta!");
            }
            var round = _db.Rounds.FindById(discussion.RoundId);
            lock (@lock)
            {
                discussion.IsClosed = true;
                var posts = (from post in _db.Posts.Query()
                             where post.DiscussionId == discussionId
                             orderby post.PostId
                             select post)
                             .ToList();
                var poll = new Poll(currentUserId)
                {
                    DiscussionId = discussion.DiscussionId,
                    Title = discussion.Title + " - hlasovanie",
                    Options = posts,
                    Topic = discussion.Topic,
                    RoundId = round.RoundId,
                };
                _db.Polls.Insert(poll);
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
            }
            return (discussion, round);
        }
    
        public (Poll, Round, Book) ClosePoll(int pollId, string currentUserId)
        {
            _logger.LogInformation("Closing poll {pollId}", pollId);
            var poll = _db.Polls.FindById(pollId);
            var round = _db.Rounds.FindById(poll.RoundId);
            Trace.Assert(!poll.IsClosed);
            poll.IsClosed = true;
            Book book = null;
            switch (poll.Topic)
            {
                case Topic.Books:
                    book = _bookService.CreateBook(poll, round.RoundId, currentUserId);
                    round.BookId = book.BookId;
                    break;
                case Topic.Themes:
                    Discussion bookDiscussion = CreateDiscussionFromTopicPoll(currentUserId, poll, round);
                    round.BookDiscussionId = bookDiscussion.DiscussionId;
                    break;
            }
            _db.Polls.Update(poll);
            _db.Rounds.Update(round);
            return (poll, round, book);
        }

        private Discussion CreateDiscussionFromTopicPoll(string currentUserId, Poll poll, Round round)
        {
            var winner = poll.FindWinningPost();
            var winnerPost = _db.Posts.FindById(winner);
            var bookDiscussion = new Discussion(currentUserId)
            {
                RoundId = round.RoundId,
                Topic = Topic.Books,
                Title = $"{round.Title} - návrhy kníh",
                Description = $"**{winnerPost.Title}** - {winnerPost.Text}",
            };
            _db.Discussions.Insert(bookDiscussion);
            return bookDiscussion;
        }
    }
}
