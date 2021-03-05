using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Obskurnee.Services
{
    public class PollService
    {
        private readonly ILogger<PollService> _logger;
        private readonly Database _db;
        private readonly UserService _users;
        private readonly IStringLocalizer<Strings> _localizer;
        private readonly object @lock = new object();

        public PollService(
            ILogger<PollService> logger,
            Database database,
            UserService users,
            IStringLocalizer<Strings> localizer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public IEnumerable<Poll> GetAll()
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

        public Poll CastPollVote(Vote vote)
        {
            _logger.LogInformation("New vote: {@vote}", vote);
            Trace.Assert(!string.IsNullOrWhiteSpace(vote.OwnerId));
            Trace.Assert(vote.PollId != 0);
            var poll = _db.Polls.FindById(vote.PollId);
            Trace.Assert(poll != null);
            if (poll.IsClosed)
            {
                throw new PermissionException(_localizer["votingClosed"]);
            }
            _db.Votes.Upsert(vote);

            UpdateVoteStats(poll);
            return poll;
        }

        private PollResults UpdateVoteStats(Poll poll)
        {
            lock (@lock)
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
                poll.Results = new PollResults
                {
                    AlreadyVoted = allVotes.Count(),
                    TotalVoters = _users.GetAllUserIds().Count,
                    YetToVote = (from u in _users.Users
                                 where !allVotes.Any(v => v.OwnerId == u.Key)
                                 select u.Value.Name)
                                 .ToList(),
                    Votes = from t in totals.OrderByDescending(kvp => kvp.Value)
                            select new VoteResultItem
                            {
                                PostId = t.Key,
                                Votes = t.Value,
                                Percentage = (int)((t.Value / (decimal)allVotes.Count()) * 100)
                            },
                };
                _db.Polls.Update(poll);
                return poll.Results;
            }
        }

        public Poll GetLatestOpen() => _db.Polls.Find(d => !d.IsClosed).OrderByDescending(d => d.PollId).FirstOrDefault();
    }
}
