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



namespace Obskurnee.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
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

        private readonly SignInManager<Bookworm> _signInManager;
        private readonly UserManager<Bookworm> _userManager;

        private static readonly SigningCredentials SigningCreds = new SigningCredentials(Startup.SecurityKey, SecurityAlgorithms.HmacSha256);
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public UserService(
           UserManager<Bookworm> userManager,
           SignInManager<Bookworm> signInManager,
           Database database,
           ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<UserInfo> Register(LoginCredentials creds)
        {
            var user = new Bookworm
            {
                UserName = creds.Email.Substring(0, creds.Email.IndexOf('@')),
                Email = creds.Email,
            };
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                // await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                ReloadCache();
                return UserInfo.FromBookworm(user);
            }
            return null;
        }

        public async Task<IdentityResult> MakeModerator(string email)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            return await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Moderator, "true"));
        }

        public async Task<IdentityResult> MakeAdmin(string email)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            return await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Admin, "true"));
        }

        public async Task<UserInfo> RegisterFirstAdmin(LoginCredentials creds)
        {
            if (GetAllUserIds().Count != 0)
            {
                throw new ForbiddenException("Users already exist");
            }
            var newUser = await Register(creds);
            if (newUser == null)
            {
                return null;
            }
            var isMod = await MakeModerator(newUser.Email);
            var isAdmin = await MakeAdmin(newUser.Email);
            if (isMod.Succeeded && isAdmin.Succeeded)
            {
                return newUser;
            }
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
                    signingCredentials: SigningCreds));

        public static string GetUserName(string userId) => (_users?.ContainsKey(userId ?? "") ?? false) ? _users[userId].Name : null;
    }
}