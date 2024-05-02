using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Server;
using Obskurnee.ViewModels;
using System.Security.Claims;

namespace Obskurnee.Services;

public class DemoUserService : UserServiceBase
{
    private readonly IStringLocalizer<Strings> _localizer;
    private readonly SignInManager<Bookworm> _signInManager;

    public DemoUserService(
        ILogger<UserServiceBase> logger,
        UserManager<Bookworm> userManager,
        SignInManager<Bookworm> signInManager,
        IStringLocalizer<Strings> localizer,
        ApplicationDbContext dbContext,
        Config config) : base(userManager, logger, dbContext, localizer, config)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public override async Task<ClaimsPrincipal> GetPrincipal(LoginCredentials creds)
        => await _signInManager.CreateUserPrincipalAsync(
            await _userManager.FindByEmailAsync("janka@fake.mail"));

    public override Task<bool> InitiatePasswordReset(string email)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<IdentityResult> MakeAdmin(string email)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<IdentityResult> MakeModerator(string email)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<(UserInfo? user, string? error)> Register(LoginCredentials creds, string? defaultName = null)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<(UserInfo? user, string? error)> RegisterBot(LoginCredentials creds, string? defaultName = null)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<(UserInfo? user, string error)> RegisterFirstAdmin(LoginCredentials creds)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<IdentityResult> ResetPassword(
        string userId,
        string resetToken,
        string newPassword)
        => throw new ForbiddenException(_localizer["forbiddenInDemo"]);

    public override Task<bool> ValidateLogin(LoginCredentials creds)
        => Task.FromResult(true);
}
