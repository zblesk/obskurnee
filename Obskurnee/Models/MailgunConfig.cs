using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class MailgunConfig
    {
        public const string ConfigName = "Mailgun";

        public string EndpointUri { get; set; }
        public string ApiKey { get; set; }
        public string ApiUsername { get; set; }
        public string SenderDomainName { get; set; }
        public string SenderEmail { get; set; }
    }
}
