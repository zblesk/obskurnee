using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Obskurnee.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using Microsoft.Extensions.Localization;

namespace Obskurnee.Services
{
    public abstract class UserServiceBase
    {
        private readonly ILogger<UserServiceBase> _logger;

        protected readonly UserManager<Bookworm> _userManager;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        protected readonly Config _config;
        protected readonly ApplicationDbContext _dbContext;
        private static IReadOnlyDictionary<string, string> _userNames = new Dictionary<string, string>();

        public UserServiceBase(
           UserManager<Bookworm> userManager,
           ILogger<UserServiceBase> logger,
           ApplicationDbContext dbContext,
           Config config)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dbContext = dbContext;
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            var result = new List<UserInfo>();
            foreach (var user in _dbContext.Users)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                result.Add(UserInfo.From(user, claims));
            }
            return result;
        }

        public void LoadUsernameCache()
        {
            _userNames = (from user in _dbContext.Users
                          select new { user.Id, user.UserName })
                         .ToDictionary(
                            u => u.Id,
                            u => u.UserName);
        }

        public List<Bookworm> GetAllUsersAsBookworm()
            => _dbContext.Users.ToList();

        public IList<string> GetAllUserIds()
        {
            return _dbContext.Users.Select(u => u.Id).ToList();
        }

        public abstract Task<UserInfo> Register(LoginCredentials creds);

        public abstract Task<IdentityResult> MakeModerator(string email);

        public abstract Task<IdentityResult> MakeAdmin(string email);

        public abstract Task<UserInfo> RegisterFirstAdmin(LoginCredentials creds);

        public abstract Task<bool> ValidateLogin(LoginCredentials creds);

        public abstract Task<ClaimsPrincipal> GetPrincipal(LoginCredentials creds);

        public string GetToken(ClaimsPrincipal principal)
            => _tokenHandler.WriteToken(
                new JwtSecurityToken(
                    "obskurnee",
                    "obskurnee",
                    principal.Claims,
                    expires: DateTime.UtcNow.AddDays(190),
                    signingCredentials: _config.SigningCreds));

        public static string GetUserName(string userId) => (_userNames?.ContainsKey(userId ?? "") ?? false) ? _userNames[userId] : "";

        public abstract Task<bool> InitiatePasswordReset(string email);

        public abstract Task<IdentityResult> ResetPassword(string userId, string resetToken, string newPassword);

        public async Task<UserInfo> GetUserByEmail(string email)
        {
            var user = _dbContext.Users.First(bw => bw.Email == email);
            return UserInfo.From(user, await _userManager.GetClaimsAsync(user));
        }
     
        public async Task<UserInfo> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return UserInfo.From(user, await _userManager.GetClaimsAsync(user));
        }

        public async Task<UserInfo> UpdateUserProfile(
            string email,
            string name,
            string phone,
            string goodreadsUrl,
            string aboutMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            _logger.LogInformation("Updating user profile for {userId} ({email})", user.Id, user.Email);
            user.UserName = name;
            user.GoodreadsProfileUrl = goodreadsUrl;
            user.AboutMe = aboutMe;
            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                _logger.LogError("User update failed. IdentityResult: {@identityResult}", identityResult);
                throw new DatastoreException("User update failed");
            }
            if (!(await _userManager.SetPhoneNumberAsync(user, phone)).Succeeded)
            {
                _logger.LogError(
                    "User update succeeded, but phone number update failed. IdentityResult: {@identityResult}",
                    identityResult);
                throw new DatastoreException("User update succeeded, but phone number update failed.");
            }
            _logger.LogInformation("User profile for {userId} ({email}) updated.", user.Id, user.Email);
            LoadUsernameCache();

            return await GetUserByEmail(email);
        }
    }
}