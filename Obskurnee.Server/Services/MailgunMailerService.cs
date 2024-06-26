﻿using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace Obskurnee.Services;

public class MailgunMailerService(
    ILogger<MailgunMailerService> logger,
    IConfiguration config) : IMailerService
{
    private readonly ILogger<MailgunMailerService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));

    public async Task SendMail(string subject, string markdownBody, params string[] recipients)
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
                    text = markdownBody,
                    subject = subject,
                    html = markdownBody.RenderMarkdown(),
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
