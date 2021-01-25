using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Obskurnee.Models.Round;

namespace Obskurnee.Services
{
    public class RoundManagerService
    {

        private readonly ILogger<RoundManagerService> _logger;
        private readonly Database _db;
        private readonly UserService _users;
        private readonly BookService _bookService;

        public RoundManagerService(
            ILogger<RoundManagerService> logger,
            Database database)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Round NewRound(RoundKind kind, string title, string description, string ownerId)
        {
            var round = new Round(ownerId)
            {
                Kind = kind,
                Title = title,
            };
            var discussion = new Discussion(ownerId)
            {
                Description = description,
            };

            switch (kind)
            {
                case RoundKind.Books:
                    discussion.Title = $"{title} - návrhy kníh";
                    _db.Discussions.Insert(discussion);
                    round.BookDiscussionId = discussion.DiscussionId;
                    break;
                case RoundKind.Themes:
                    discussion.Title = $"{title} - návrhy tém";
                    _db.Discussions.Insert(discussion);
                    round.ThemeDiscussionId = discussion.DiscussionId;
                    break;
                default:
                    throw new Exception($"Invalid Round Kind: {kind}");
            }
            _db.Rounds.Insert(round);
            return round;
        }
    }
}
