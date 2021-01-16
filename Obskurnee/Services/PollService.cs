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
        private object @lock = new object(); 

        public PollService(
            ILogger<PollService> logger,
            Database database,
            UserService users)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _users = users ?? throw new ArgumentNullException(nameof(users));
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
            var voteCount = _db.Votes.Find(v => v.PollId == pollId).Count();
            if (vote == null)
            {
                poll.Results.Votes = null;
            }
            return new PollInfo(poll, vote, voteCount);
        }

        internal PollResults CastPollVote(Vote vote)
        {
            Trace.Assert(!string.IsNullOrWhiteSpace(vote.OwnerId));
            Trace.Assert(vote.PollId != 0);
            _db.Votes.Upsert(vote);

            lock (@lock)
            {
                return UpdateVoteStats(vote.PollId);
            }
        }

        private PollResults UpdateVoteStats(int PollId)
        {
            var allVotes = _db.Votes.Find(v => v.PollId == PollId);
            var totals = new Dictionary<int, int>();
            foreach (var votes in allVotes)
            {
                foreach (var vote in votes.PostIds)
                {
                    if (totals.ContainsKey(vote))
                    {
                        totals[vote]++;
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
                Votes = totals,
            };

            var poll = _db.Polls.FindById(PollId);

            if (pollResults.AlreadyVoted == pollResults.TotalVoters)
            {
                ClosePoll(PollId);
            }

            poll.Results = pollResults;
            _db.Polls.Update(poll);
            return pollResults;
        }

        private void ClosePoll(int pid)
        {
            //throw new NotImplementedException();
        }
    }
}
