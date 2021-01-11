using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/polls")]
    public class PollController : ControllerBase
    {
        private readonly ILogger<PollController> _logger;
        private readonly Database _database;


        public PollController(ILogger<PollController> logger, Database database)
        {
            _logger = logger;
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet]
        public IEnumerable<Poll> GetPolls() => _database.GetAllPolls();

        
        [HttpGet]
        [Route("{pollId:int}")]
        public Poll GetPoll(int pollId) => _database.GetPoll(pollId);

    }
}
