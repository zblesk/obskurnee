using Microsoft.Extensions.Logging;

namespace Obskurnee.Services;

public class FakeMailerService(
    ILogger<FakeMailerService> logger) : IMailerService
{
    private readonly ILogger<FakeMailerService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task SendMail(string subject, string markdownBody, params string[] recipients)
    {
        _logger.LogWarning("No mailer configured. Not sending newsletter:\nto: {@recipients}\nsubj: {subject}\n{body}\n\n", recipients, subject, markdownBody);
        return Task.CompletedTask;
    }
}
