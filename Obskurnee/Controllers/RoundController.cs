using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/rounds")]
    public class RoundController : Controller
    {
        private readonly ILogger<RoundController> _logger;
        private readonly RoundManagerService _roundManager;

        public RoundController(
            ILogger<RoundController> logger,
            RoundManagerService roundManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roundManager = roundManager ?? throw new ArgumentNullException(nameof(roundManager));
        }

        [HttpGet]
        public async Task<IList<Round>> GetRounds() => await _roundManager.AllRounds();

        [HttpPost]
        [Authorize(Policy = "ModOnly")]
        [Route("close-poll/{pollId:int}")]
        public RoundUpdateResults ClosePoll(int pollId)
        {
            _logger.LogInformation("Moderator {userId} closing poll {pollId}", User.GetUserId(), pollId);
            return _roundManager.ClosePoll(pollId, User.GetUserId());
        }

        [HttpPost]
        [Authorize(Policy = "ModOnly")]
        [Route("close-discussion/{discussionId:int}")]
        public RoundUpdateResults CloseDiscussion(int discussionId)
        {
            _logger.LogInformation("Moderator {userId} closing discussion {discussionId}", User.GetUserId(), discussionId);
            return _roundManager.CloseDiscussion(discussionId, User.GetUserId());
        }

        [HttpPost]
        [Authorize(Policy = "ModOnly")]
        public async Task<Round> NewRound([FromBody] JsonElement  roundData) 
            => await _roundManager.NewRound(
                (Topic)Enum.Parse(typeof(Topic), roundData.GetProperty("topic").GetString()),
                roundData.GetProperty("title").GetString(),
                roundData.TryGetProperty("description", out _) ? roundData.GetProperty("description").GetString() : "",
                User.GetUserId());
    }
}
