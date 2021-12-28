using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Obskurnee.Services
{
    public sealed class UserService : UserServiceBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly SignInManager<Bookworm> _signInManager;
        private readonly IStringLocalizer<Strings> _localizer;
        private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMailerService _mailer;
        private readonly MatrixService _matrix;

        private NewsletterService Newsletter { get => (NewsletterService)_serviceProvider.GetService(typeof(NewsletterService)); }

        public UserService(
           ILogger<UserService> logger,
           SignInManager<Bookworm> signInManager,
           IStringLocalizer<Strings> localizer,
           IStringLocalizer<NewsletterStrings> newsletterLocalizer,
           IMailerService mailer,
           IServiceProvider serviceProvider,
           UserManager<Bookworm> userManager,
           MatrixService matrix,
           ApplicationDbContext dbContext,
           Config config) : base(userManager, logger, dbContext, localizer, config)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _matrix = matrix;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public override async Task<UserInfo> Register(
            LoginCredentials creds,
            string defaultName = null)
        {
            Bookworm user = MakeNamedBookworm(creds, defaultName, isBot: false);
            _logger.LogInformation("Registering user {email}", user.Email);
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created");
                LoadUsernameCache();
                await Newsletter.Subscribe(user.Id, Newsletters.BasicEvents);
                _ = SendDefaultLanguageNotification(user)
                    .ContinueWith(t => _logger.LogError(t.Exception, "Sending Matrix notification failed"),
                        TaskContinuationOptions.OnlyOnFaulted);
                return UserInfo.From(user);
            }
            return null;
        }

        public override async Task<UserInfo> RegisterBot(
            LoginCredentials creds,
            string defaultName = null)
        {
            Bookworm user = MakeNamedBookworm(creds, defaultName, isBot: true);
            _logger.LogInformation("Registering bot {email}", user.Email);
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Bot created");
                await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Bot, "true"));
                return UserInfo.From(user);
            }
            return null;
        }

        private Bookworm MakeNamedBookworm(LoginCredentials creds, string defaultName, bool isBot)
        {
            if (string.IsNullOrWhiteSpace(defaultName))
            {
                defaultName = creds.Email.Substring(0, creds.Email.IndexOf('@'));
            }
            var user = new Bookworm
            {
                UserName = defaultName,
                Email = creds.Email,
                Language = _config.DefaultCulture,
                IsBot = isBot,
                LoginEnabled = true,
            };
            return user;
        }

        private async Task SendDefaultLanguageNotification(Bookworm user)
        {
            var origCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = _config.DefaultCultureInfo;
            await _matrix.SendMessage(
                _newsletterLocalizer.Format("newUserAdded",
                user.UserName,
                $"{_config.BaseUrl}/my/{user.Email}"));
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = origCulture;
        }

        public override async Task<IdentityResult> MakeModerator(string email)
        {
            _logger.LogInformation("Making moderator of {email}", email);
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            var identityResult = await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Moderator, "true"));
            return identityResult;
        }

        public override async Task<IdentityResult> MakeAdmin(string email)
        {
            _logger.LogInformation("Making admin of {email}", email);
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            var identityResult = await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Admin, "true"));
            return identityResult;
        }

        public override async Task<UserInfo> RegisterFirstAdmin(LoginCredentials creds)
        {
            if (GetAllUserIds().Count != 0)
            {
                throw new ForbiddenException(_localizer["usersAlreadyExist"]);
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

        public override async Task<bool> ValidateLogin(LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            if (user == null)
            {
                return false;
            }
            var signInRes = await _signInManager.CheckPasswordSignInAsync(user, creds.Password, lockoutOnFailure: false);
            return signInRes.Succeeded;
        }

        public override async Task<ClaimsPrincipal> GetPrincipal(LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        public override async Task<bool> InitiatePasswordReset(string email)
        {
            _logger.LogInformation("Initiating password reset for {email}", email);
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
            var subject = _newsletterLocalizer.Format("passwordResetSubject");
            var body = _newsletterLocalizer.Format("passwordResetBody", callbackUrl);
            await _mailer.SendMail(subject, body, user.Email);
            return true;
        }

        public override async Task<IdentityResult> ResetPassword(string userId, string resetToken, string newPassword)
        {
            _logger.LogInformation("Resetting password for user {userId}", userId);
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }
    }
}