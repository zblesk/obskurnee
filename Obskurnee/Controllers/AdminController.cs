using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using System.Collections.Generic;

using System.Threading.Tasks;
using Flurl.Http;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Obskurnee.Controllers
{
    [Authorize(Policy = "ModOnly")]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly SettingsService _settings;
        private readonly UserService _users;
        private readonly IMailerService _mailer;
        private static readonly Random _random = new Random();
        private readonly Config _config;

        public AdminController(
            ILogger<AdminController> logger,
            SettingsService settings,
            UserService users,
            IMailerService mailer,
            Config config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpGet]
        public JsonResult GetAdminInfo() => Json(
            new
            {
                Noticeboard = _settings.GetSettingValue<string>(Setting.Keys.ModNoticeboard),
                MailSettings = _settings.GetSetting(Setting.Keys.MailgunSettings),
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
        [Route("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] JObject payload)
        {
            var email = payload["email"].ToString();
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

            var user = await _users.Register(new LoginCredentials { Email = email, Password = password });
            if (user != null)
            {
                await _mailer.SendMail(
                    $"Vitaj, {user.Name}",
                    $"Bola si uspesne zaregistrovana.\nPrihlas sa na <a href='{_config.BaseUrl}'>{_config.BaseUrl}</a>\n\nTvoj prihlasovaci mail: {user.Email}\nDefault heslo: {password}\n\nAsi by bolo najlepsie si to heslo zmenit.",
                    email);
                return Json(user);
            }
            return ValidationProblem("Registration failed");
        }

        [HttpPost("makemod/{email}")]
        public async Task MakeModerator(string email)
            => await _users.MakeModerator(email);
    }
}
