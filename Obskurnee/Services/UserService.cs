using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Obskurnee.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using Microsoft.Extensions.Localization;

namespace Obskurnee.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly Database _db;

        private readonly SignInManager<Bookworm> _signInManager;
        private readonly UserManager<Bookworm> _userManager;

        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly IMailerService _mailer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ReviewService _reviews;
        private readonly Config _config;
        private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
        private readonly ApplicationDbContext _dbContext;
        private static IReadOnlyDictionary<string, string> _userNames = new Dictionary<string, string>();

        private NewsletterService Newsletter { get => (NewsletterService)_serviceProvider.GetService(typeof(NewsletterService)); }

        public UserService(
           UserManager<Bookworm> userManager,
           SignInManager<Bookworm> signInManager,
           Database database,
           ILogger<UserService> logger,
           IMailerService mailer,
           IServiceProvider serviceProvider,
           ReviewService reviews,
           IStringLocalizer<NewsletterStrings> newsletterLocalizer,
           Config config,
           ApplicationDbContext dbContext)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _reviews = reviews ?? throw new ArgumentNullException(nameof(reviews));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
            _dbContext = dbContext;
        }

        public IEnumerable<UserInfo> GetAllUsers(bool includeCurrentlyReading = false)
        {
            var users = GetAllUsers();
            users.Wait();
            foreach (var user in users.Result)
            {
                if (includeCurrentlyReading)
                {
                    user.CurrentlyReading = _reviews.GetCurrentlyReading(user.UserId);
                }
                yield return user;
            }
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

        public async Task<UserInfo> Register(LoginCredentials creds)
        {
            var user = new Bookworm
            {
                UserName = creds.Email.Substring(0, creds.Email.IndexOf('@')),
                Email = creds.Email,
            };
            _logger.LogInformation("Registering user {email}", user.Email);
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created");
                LoadUsernameCache();
                Newsletter.Subscribe(user.Id, Newsletters.BasicEvents);
                _db.Flush();
                return UserInfo.From(user);
            }
            return null;
        }

        public async Task<IdentityResult> MakeModerator(string email)
        {
            _logger.LogInformation("Making moderator of {email}", email);
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            var identityResult = await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Moderator, "true"));
            return identityResult;
        }

        public async Task<IdentityResult> MakeAdmin(string email)
        {
            _logger.LogInformation("Making admin of {email}", email);
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            var identityResult = await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Admin, "true"));
            return identityResult;
        }

        public async Task<UserInfo> RegisterFirstAdmin(LoginCredentials creds)
        {
            if (GetAllUserIds().Count != 0)
            {
                throw new ForbiddenException("Users already exist");
            }
            _logger.LogInformation("Creating first admin with {email}", creds.Email);
            var newUser = await Register(creds);
            if (newUser == null)
            {
                _logger.LogError("First admin creation failed");
                throw new Exception("First admin creation failed");
            }
            var isMod = await MakeModerator(newUser.Email);
            var isAdmin = await MakeAdmin(newUser.Email);
            if (isMod.Succeeded && isAdmin.Succeeded)
            {
                return newUser;
            }
            _logger.LogError("First admin creation failed. \nMod sucess: {@isMod} \n\nAdmin success: {@isAdmin}", isMod, isAdmin);
            throw new Exception("Failed to make admin");
        }

        public async Task<bool> ValidateLogin(LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            if (user == null)
            {
                return false;
            }
            var signInRes = await _signInManager.CheckPasswordSignInAsync(user, creds.Password, lockoutOnFailure: false);
            return signInRes.Succeeded;
        }

        public async Task<ClaimsPrincipal> GetPrincipal(LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        public string GetToken(ClaimsPrincipal principal)
            => _tokenHandler.WriteToken(
                new JwtSecurityToken(
                    "obskurnee",
                    "obskurnee",
                    principal.Claims,
                    expires: DateTime.UtcNow.AddDays(190),
                    signingCredentials: _config.SigningCreds));

        public static string GetUserName(string userId) => (_userNames?.ContainsKey(userId ?? "") ?? false) ? _userNames[userId] : "";

        public async Task<bool> InitiatePasswordReset(string email)
        {
            _logger.LogInformation("Initiating password reset for {email}"  , email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)   
            {
                return false;
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = string.Format("{0}/passwordreset/{1}/{2}",
                                            _config.BaseUrl,
                                            HttpUtility.UrlEncode(user.Id),
                                            HttpUtility.UrlEncode(resetToken));
            var subject = _newsletterLocalizer["passwordResetSubject"];
            var body = _newsletterLocalizer.Format("passwordResetBody", callbackUrl);
            await _mailer.SendMail(subject, body, user.Email);
            _logger.LogWarning("reset hesla body {b}", body);
            return true;
        }

        public async Task<IdentityResult> ResetPassword(string userId, string resetToken, string newPassword)
        {
            _logger.LogInformation("Resetting password for user {userId}", userId);
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }

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