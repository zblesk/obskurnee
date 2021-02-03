using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Obskurnee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/polls")]
    public class PollController : Controller
    {
        private readonly ILogger<PollController> _logger;
        private readonly PollService _polls;
        private readonly RoundManagerService _roundManager;

        public PollController(
            ILogger<PollController> logger,
            PollService polls,
            RoundManagerService roundManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _polls = polls ?? throw new ArgumentNullException(nameof(polls));
            this._roundManager = roundManager ?? throw new ArgumentNullException(nameof(roundManager));
        }

        [HttpGet]
        public IEnumerable<Poll> GetPolls() => _polls.GetAll();
        
        [HttpGet]
        [Route("{pollId:int}")]
        public PollInfo GetPoll(int pollId) => _polls.GetPollInfo(pollId, User.GetUserId());

        [HttpPost]
        [Route("{pollId:int}/vote")]
        public RoundUpdateResults CastVote(int pollId, Vote vote)
        {
            vote.PollId = pollId;
            var poll = _polls.CastPollVote(vote.SetOwner(User));
            _logger.LogInformation("User {userId} voted in poll {pollId}", User.GetUserId(), pollId);
            if (poll.Results.AlreadyVoted == poll.Results.TotalVoters)
            {
                return _roundManager.ClosePoll(pollId, User.GetUserId());
            }
            return new RoundUpdateResults { Poll = poll };
        }

        [HttpPost]
        [Authorize(Policy = "ModOnly")]
        [Route("{pollId:int}/close")]
        public RoundUpdateResults ClosePoll(int pollId)
        {
            _logger.LogInformation("Moderator {userId} closing poll {pollId}", User.GetUserId(), pollId);
            return _roundManager.ClosePoll(pollId, User.GetUserId());
        }
    }
}
