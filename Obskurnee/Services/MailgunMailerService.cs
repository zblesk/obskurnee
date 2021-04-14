using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace Obskurnee.Services
{
    public class MailgunMailerService : IMailerService
    {
        private readonly ILogger<MailgunMailerService> _logger;
        private readonly IConfiguration _config;

        public MailgunMailerService(
            ILogger<MailgunMailerService> logger,
            IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task SendMail(string subject, string body, params string[] recipients)
        {
            _logger.LogInformation("Sending mail '{subject}' to {@recipients}", subject, recipients);
            var mailConfig = _config
                .GetSection(MailgunConfig.ConfigName)
                .Get<MailgunConfig>();
            var response = await mailConfig.EndpointUri
                .AppendPathSegments(mailConfig.SenderDomainName, "messages")
                .WithBasicAuth(mailConfig.ApiUsername, mailConfig.ApiKey)
                .PostMultipartAsync(mp =>
                {
                    mp.AddStringParts(new
                    {
                        from = mailConfig.SenderEmail,
                        subject = subject,
                        text = body,
                    });
                    foreach (var mail in recipients)
                    {
                        mp.AddString("to", mail);
                    }
                });
            _logger.Log(
                response.StatusCode == 200 ? LogLevel.Information : LogLevel.Error,
                "Mail sending status: {status}",
                response.StatusCode);
        }
    }
}
