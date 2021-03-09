using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Obskurnee.Models
{
    public class Config
    {
        public readonly string DataFolder = "data";
        public readonly string ImageFolder = "images";
        private string _key;
        private string _baseUrl;

        public static Config Current;

        public string SymmetricSecurityKey
        {
            get => _key;
            set
            {
                _key = value;
                SecurityKey = new SymmetricSecurityKey(
                    Encoding.Default.GetBytes(_key));
                SigningCreds = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            }
        }

        public string Urls { get; set; }
        public SymmetricSecurityKey SecurityKey { get; private set; }
        public string BaseUrl { get => (_baseUrl ?? "").Trim().TrimEnd('/'); set => _baseUrl = value; }
        public int DefaultPasswordMinLength { get; set; }
        public string PasswordGenerationChars { get; set; }
        public string GoodreadsRssBaseUrl { get; set; }
        public string GoodreadsProfielUrlPrevix { get; set; }
        public string GlobalCulture { get; set; }

        public SigningCredentials SigningCreds { get; private set; }
    }
}