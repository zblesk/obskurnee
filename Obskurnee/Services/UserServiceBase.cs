using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        private readonly IStringLocalizer<Strings> _localizer;

        public UserServiceBase(
           UserManager<Bookworm> userManager,
           ILogger<UserServiceBase> logger,
           ApplicationDbContext dbContext,
           IStringLocalizer<Strings> localizer,
           Config config)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            var result = new List<UserInfo>();
            foreach (var user in _dbContext.UsersExceptBots)
            {
                var claims = await _userManager.GetRolesAsync(user);
                result.Add(UserInfo.From(user, claims));
            }
            return result;
        }

        public int GetAllActiveUserCount()
            => _dbContext.ActiveUsers.Count();

        public void LoadUsernameCache()
        {
            _userNames = (from user in _dbContext.UsersExceptBots
                          select new { user.Id, user.UserName })
                         .ToDictionary(
                            u => u.Id,
                            u => u.UserName);
        }

        public List<Bookworm> GetAllUsersAsBookworm()
            => _dbContext.UsersExceptBots.ToList();

        public IList<string> GetAllUserIds()
        {
            return _dbContext.Users.Select(u => u.Id).ToList();
        }

        public abstract Task<UserInfo> Register(LoginCredentials creds, string defaultName = null);

        public abstract Task<(UserInfo user, string error)> RegisterBot(LoginCredentials creds, string defaultName = null);

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
            return UserInfo.From(user, await _userManager.GetRolesAsync(user));
        }

        public async Task<UserInfo> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return UserInfo.From(user, await _userManager.GetRolesAsync(user));
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
                throw new DatastoreException(_localizer["userUpdateFailed"]);
            }
            if (!(await _userManager.SetPhoneNumberAsync(user, phone)).Succeeded)
            {
                _logger.LogError(
                    "User update succeeded, but phone number update failed. IdentityResult: {@identityResult}",
                    identityResult);
                throw new DatastoreException(_localizer["userUpdateFailed"]);
            }
            _logger.LogInformation("User profile for {userId} ({email}) updated.", user.Id, user.Email);
            LoadUsernameCache();

            return await GetUserByEmail(email);
        }

        public Task<string> GetUserLanguage(string userId)
            => (from u in _dbContext.Users.AsNoTracking()
                where u.Id == userId
                select u.Language)
                .FirstOrDefaultAsync();

        public async Task SetUserLanguage(string userId, string language)
        {
            if (!Config.SupportedLanguages.Contains(language))
            {
                throw new ValidationException(_localizer.Format("unsupportedLanguage", language));
            }
            var user = await _dbContext.Users.FindAsync(userId);
            user.Language = language;
            _dbContext.Update(user);
            _logger.LogInformation("Changing {userId}'s language to {lang}", userId, language);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetUserAvatar(string userId, byte[] image, string extension)
        {
            _logger.LogInformation("Updating avatar for {userId}", userId);
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                var user = await _dbContext.Users.Where(user => user.Id == userId)
                                 .FirstAsync();
                var fname = $"{user.UserName.Replace(" ", "")}.{DateTime.Now.ToString("yyyyMMdd-hhmmss")}{extension}";
                _dbContext.Images.Add(new StoredImage
                {
                    FileName = fname,
                    FileContents = image,
                    Extension = extension,
                    Kind = StoredImage.ImageKind.ProfilePic,
                });
                user.AvatarUrl = Helpers.MakeImageRelativePath(fname);
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setting avatar failed");
                _dbContext.Database.RollbackTransaction();
            }
        }

        public IEnumerable<BotInfo> GetBots()
            => from user in _dbContext.Users
               where user.IsBot
               select new BotInfo
               {
                   Name = user.UserName,
                   Email = user.Email,
                   LoginEnabled = user.LoginEnabled
               };
    }
}