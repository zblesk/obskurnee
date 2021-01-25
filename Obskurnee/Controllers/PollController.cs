using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

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
        public IEnumerable<Poll> GetPolls() => _polls.GetAllPolls();

        
        [HttpGet]
        [Route("{pollId:int}")]
        public PollInfo GetPoll(int pollId) => _polls.GetPollInfo(pollId, User.GetUserId());

        [HttpPost]
        [Route("{pollId:int}/vote")]
        public JsonResult CastVote(int pollId, Vote vote)
        {
            vote.PollId = pollId;
            var results = _polls.CastPollVote(vote.SetOwner(User));
            if (results.AlreadyVoted == results.TotalVoters)
            {
                _roundManager.ClosePoll(pollId, User.GetUserId());
            }
            return Json(results);
        }
    }
}
