using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class MailgunMailerService : IMailerService
    {
        private readonly ILogger<MailgunMailerService> _logger;
        private readonly SettingsService _settings;

        public MailgunMailerService(
            ILogger<MailgunMailerService> logger,
            SettingsService settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task SendMail(string subject, string body, params string[] recipients)
        {
            _logger.LogInformation("Sending mail '{subject}' to {@recipients}", subject, recipients);

            var settings = _settings.GetSetting(Setting.Keys.MailgunSettings);
            if (settings == null)
            {
                _logger.LogError("Attempting to send mail; mail settings not found!");
                return;
            }


            //RestClient client = new RestClient();
            //client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            //client.Authenticator =
            //    new HttpBasicAuthenticator("api",
            //                                "YOUR_API_KEY");
            //RestRequest request = new RestRequest();
            //request.AddParameter("domain", "YOUR_DOMAIN_NAME", ParameterType.UrlSegment);
            //request.Resource = "{domain}/messages";
            //request.AddParameter("from", "Excited User <mailgun@YOUR_DOMAIN_NAME>");
            //request.AddParameter("to", "bar@example.com");
            //request.AddParameter("to", "YOU@YOUR_DOMAIN_NAME");
            //request.AddParameter("subject", "Hello");
            //request.AddParameter("text", "Testing some Mailgun awesomness!");
            //request.Method = Method.POST;
            //return client.Execute(request);

        }
    }
}
