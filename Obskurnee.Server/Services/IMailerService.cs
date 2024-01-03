namespace Obskurnee.Services;

public interface IMailerService
{
    Task SendMail(string subject, string markdownBody, params string[] recipients);
}
