using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Flurl;
using Flurl.Http;
using System;
using System.Linq;

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

        public AdminController(
           ILogger<AdminController> logger,
           SettingsService settings,
           UserService users,
           IMailerService mailer)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
            _users = users ?? throw new System.ArgumentNullException(nameof(users));
            _mailer = mailer ?? throw new System.ArgumentNullException(nameof(mailer));
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
        public IActionResult UpdateNoticeboard([FromBody] JsonElement payload)
        {
            _logger.LogInformation("Updating noticeboard");
            _settings.UpsertSetting(Setting.Keys.ModNoticeboard, payload.GetProperty("text").GetString());
            return Ok();
        }

        [HttpPost]
        [Route("mailconfig")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateMailConfig([FromBody] JsonElement mailConfig) => Ok();

        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> AddUser([FromBody] JsonElement payload)
        {
            var email = payload.GetProperty("email").GetString();
            _logger.LogInformation("Adding user {email}", email);
            var password = Enumerable.Range(1, Startup.DefaultPasswordMinLength)
                .Aggregate(
                "", 
                (pwd, _) => $"{pwd}{Startup.PasswordGenerationChars[_random.Next(Startup.PasswordGenerationChars.Length)]}");
            try
            {
                var passwordSrc = await "https://zble.sk/api/password-gen/"
                    .GetAsync()
                    .ReceiveJson<List<string>>();
                password = string.Join("", passwordSrc);
            }
            catch
            {
                // ignore errors
            }

            var user = await _users.Register(new LoginCredentials { Email = email, Password = password });
            if (user != null)
            {
                await _mailer.SendMail(
                    $"Vitaj, {user.Name}", 
                    $"Bola si uspesne zaregistrovana.\nPrihlas sa na <a href='{Startup.BaseUrl}'>{Startup.BaseUrl}</a>\n\nTvoj prihlasovaci mail: {user.Email}\nDefault heslo: {password}\n\nAsi by bolo najlepsie si to heslo zmenit.", 
                    email);
                return Json(user);
            }
            return ValidationProblem("Registration failed");
        }

        [HttpGet("makemod/{email}")]
        public async Task MakeModerator(string email)
            => await _users.MakeModerator(email);
    }
}
