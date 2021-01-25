using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class Mailer
    {
        public async Task SendMail(string subject, string body, params string[] recipients)
        {
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
