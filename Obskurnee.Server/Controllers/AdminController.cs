﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Localization;
using Obskurnee.Server;

namespace Obskurnee.Controllers;

[Authorize(Policy = "ModOnly")]
[Route("api/admin")]
public class AdminController(
    ILogger<AdminController> logger,
    SettingsService settings,
    UserServiceBase users,
    IMailerService mailer,
    BackupService backup,
    IStringLocalizer<NewsletterStrings> newsletterLocalizer,
    Config config) : Controller
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly SettingsService _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    private readonly UserServiceBase _users = users ?? throw new ArgumentNullException(nameof(users));
    private readonly IMailerService _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
    private static readonly Random _random = new();
    private readonly Config _config = config ?? throw new ArgumentNullException(nameof(config));
    private readonly BackupService _backup = backup ?? throw new ArgumentNullException(nameof(backup));
    private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));

    [HttpGet]
    public JsonResult GetAdminInfo() => Json(
        new
        {
            Noticeboard = _settings.GetSettingValue<string>(Setting.Keys.ModNoticeboard),
        });

    [HttpPost]
    [Route("noticeboard")]
    public IActionResult UpdateNoticeboard([FromBody] JObject payload)
    {
        _logger.LogInformation("Updating noticeboard");
        _settings.UpsertSetting(Setting.Keys.ModNoticeboard, payload["text"].ToString());
        return Ok();
    }

    [HttpPost]
    [Route("registeruser")]
    public async Task<IActionResult> CreateUser([FromBody] JObject payload)
    {
        var email = payload["email"].ToString();
        var name = payload.ContainsKey("name")
            ? payload["name"].ToString()
            : null;
        _logger.LogInformation("Adding user {email}", email);
        var password = Enumerable.Range(1, _config.DefaultPasswordMinLength)
            .Aggregate(
            "",
            (pwd, _) => $"{pwd}{_config.PasswordGenerationChars[_random.Next(_config.PasswordGenerationChars.Length)]}");
        if (_config.UseExternalFriendlyPasswordGenerator)
            try
            {
                var passwordSrc = await "https://zble.sk/api/password-gen/"
                    .GetAsync()
                    .ReceiveJson<List<string>>();
                password = string.Join("", passwordSrc).Trim().RemoveDiacritics();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error when attempting to fetch friendly password");
            }

        var (user, err) = await _users.Register(
            new LoginCredentials { Email = email, Password = password },
            name);
        if (user != null)
        {
            await _mailer.SendMail(
                _newsletterLocalizer.Format("newUserSubject", user.Name),
                _newsletterLocalizer.Format("newUserPostBodyMarkdown",
                    _config.BaseUrl,
                    user.Email,
                    password),
                email);
            return Json(user);
        }
        return ValidationProblem("Registration failed: " + err);
    }

    [HttpPost("registerbot")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RegisterBot([FromBody] JObject payload)
    {
        var registrationResult = await _users.RegisterBot(
            new LoginCredentials
            {
                Email = payload["email"].ToString(),
                Password = payload["password"].ToString()
            },
            payload["name"].ToString());
        if (registrationResult.user != null)
        {
            return Json(registrationResult);
        }
        return ValidationProblem($"Registration failed: {registrationResult.error}");
    }

    [HttpGet("bots")]
    [Authorize(Policy = "AdminOnly")]
    public IEnumerable<string> GetBots()
        => _users.GetBots().Select(b => b.Name);

    [HttpPost("makemod/{email}")]
    public async Task MakeModerator(string email)
        => await _users.MakeModerator(email);

    [HttpPost("createbackup")]
    public Task CreateBackup()
        => _backup.CreateBackup();
}
