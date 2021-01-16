using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class PollService
    {
        private readonly ILogger<PollService> _logger;
        private readonly Database _db;
        private readonly UserService _users;
        private readonly object @lock = new object();
        private readonly BookService _bookService;

        public PollService(
            ILogger<PollService> logger,
            Database database,
            BookService bookService,
            UserService users)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            this._bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        public IEnumerable<Poll> GetAllPolls()
        {
            return (from poll in _db.Polls.Query()
                    orderby poll.CreatedOn descending
                    select poll)
                    .ToList();
        }

        public PollInfo GetPollInfo(int pollId, string userId)
        {
            var poll = _db.Polls
              .Include(x => x.Options)
              .FindById(pollId);
            var voteId = $"{pollId}-{userId}";
            var vote = _db.Votes
                .FindById(voteId);
            if (vote == null 
                && poll.Results != null)
            {
                poll.Results.Votes = null;
            }
            return new PollInfo(poll, vote);
        }

        public PollResults CastPollVote(Vote vote)
        {
            _logger.LogInformation("New vote: {@vote}", vote);
            Trace.Assert(!string.IsNullOrWhiteSpace(vote.OwnerId));
            Trace.Assert(vote.PollId != 0);
            var poll = _db.Polls.FindById(vote.PollId);
            if (poll.IsClosed)
            {
                throw new PermissionException("Hlasovanie uz skoncilo!");
            }
            _db.Votes.Upsert(vote);

            lock (@lock)
            {
                return UpdateVoteStats(poll, vote.OwnerId);
            }
        }

        private PollResults UpdateVoteStats(Poll poll, string currentUser)
        {
            var allVotes = _db.Votes.Find(v => v.PollId == poll.PollId);
            var totals = new Dictionary<int, int>();
            foreach (var votes in allVotes)
            {
                foreach (var vote in votes.PostIds)
                {
                    if (totals.ContainsKey(vote))
                    {
                        totals[vote] += 1;
                    }
                    else
                    {
                        totals[vote] = 1;
                    }
                }
            }
            var pollResults = new PollResults
            {
                AlreadyVoted = allVotes.Count(),
                TotalVoters = _users.GetAllUserIds().Count,
                YetToVote = (from u in _users.Users
                             where !allVotes.Any(v => v.OwnerId == u.Key)
                             select u.Value.UserName)
                             .ToList(),
                Votes = from t in totals.OrderByDescending(kvp => kvp.Value)
                        select new VoteResultItem
                        {
                            PostId = t.Key,
                            Votes = t.Value,
                            Percentage = (int)((t.Value / (decimal)allVotes.Count()) * 100)
                        },
            };

            if (pollResults.AlreadyVoted == pollResults.TotalVoters)
            {
                ClosePoll(poll, currentUser);
            }

            poll.Results = pollResults;
            _db.Polls.Update(poll);
            return pollResults;
        }

        private void ClosePoll(Poll poll, string currentUser)
        {
            _logger.LogInformation("Closing poll {pollId}", poll.PollId);
            poll.IsClosed = true;
            if (poll.CreateBookOnClose)
            {
                var book = _bookService.CreateBook(poll, currentUser);
                poll.BookId = book.BookId;
            }
            _db.Polls.Update(poll);
        }
    }
}
