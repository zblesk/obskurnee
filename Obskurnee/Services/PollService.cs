using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
        private readonly ApplicationDbContext _db;
        private readonly IStringLocalizer<Strings> _localizer;

        public PollService(
            ILogger<PollService> logger,
            ApplicationDbContext database,
            IStringLocalizer<Strings> localizer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public Task<List<Poll>> GetAll()
            => (from poll in _db.PollsWithData
                orderby poll.CreatedOn descending
                select poll)
                .ToListAsync();

        public async Task<PollInfo> GetPollInfo(int pollId, string userId)
        {
            var poll = await _db.PollsWithData
              .FirstAsync(p => p.PollId == pollId);
            var voteId = $"{pollId}-{userId}";
            var vote = await _db.Votes.Where(v => v.PollId == pollId && v.OwnerId == userId).FirstOrDefaultAsync();
            return new PollInfo(poll, vote);
        }

        public async Task<Poll> CastPollVote(Vote vote)
        {
            _logger.LogInformation("New vote: {@vote}", vote);
            Trace.Assert(!string.IsNullOrWhiteSpace(vote.OwnerId));
            var poll = await _db.PollsWithData
                .FirstAsync(p => p.PollId == vote.PollId);
            if (poll.IsClosed)
            {
                throw new PermissionException(_localizer["votingClosed"]);
            }

            var existing = await _db.Votes.FirstOrDefaultAsync(v => v.VoteId == vote.VoteId);
            if (existing != null)
            {
                _db.Votes.Remove(existing);
                await _db.SaveChangesAsync();
            }
            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();
            await UpdateVoteStats(poll);
            return poll;
        }

        private async Task<PollResults> UpdateVoteStats(Poll poll)
        {
            try
            {
                await _db.Database.BeginTransactionAsync();
                var allVotes = await _db.Votes.Where(v => v.PollId == poll.PollId).ToListAsync();
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
                    AlreadyVoted = allVotes.Select(v => v.OwnerId).ToList(),
                    Votes = (from t in totals.OrderByDescending(kvp => kvp.Value)
                             select new VoteResultItem
                             {
                                 PostId = t.Key,
                                 Votes = t.Value,
                                 Percentage = (int)((t.Value / (decimal)allVotes.Count()) * 100)
                             })
                             .ToList(),
                };
                _db.Polls.Update(poll);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
                return poll.Results;
            }
            catch
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }

        public Task<Poll> GetLatestOpen()
            => _db.Polls.OrderByDescending(d => d.PollId).FirstOrDefaultAsync(d => !d.IsClosed);
    }
}
