using Microsoft.Extensions.Logging;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class UserService
    {

        private readonly ILogger<PollService> _logger;
        private readonly Database _db;
        private static IReadOnlyDictionary<string, UserInfo> _users;

        public IReadOnlyDictionary<string, UserInfo> Users
        {
            get
            {
                EnsureCacheLoaded();
                return _users;
            }
        }

        public UserService(
            ILogger<PollService> logger, 
            Database database)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
        }


        public void ReloadCache()
        {
            _users = _db.Users.FindAll().ToDictionary(u => u.Id, u => new UserInfo(u.Id, u.UserName, u.Email.Address));
        }

        private void EnsureCacheLoaded()
        {
            if (_users == null)
            {
                ReloadCache();
            }
        }

        public IList<string> GetAllUserIds()
        {
            EnsureCacheLoaded();
            return _db.Users.Query().Select(u => u.Id).ToList();
        }
    }
}