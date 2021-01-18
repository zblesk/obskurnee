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

        public PollController(ILogger<PollController> logger, PollService polls)
        {
            _logger = logger;
            _polls = polls ?? throw new ArgumentNullException(nameof(polls));
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
            return Json(results);
        }
    }
}
