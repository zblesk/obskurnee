using Obskurnee.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace Obskurnee.ViewModels
{
    public class BotInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool LoginEnabled { get; set; }
    }
}
