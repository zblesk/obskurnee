using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

namespace Obskurnee.Models;

public class Config
{
    public const string DataFolder = "data";
    public const string BackupFolder = "backup";
    public static readonly string[] SupportedLanguages = new[] { "sk", "en", "cs" };
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
    public string MailerType { get; set; }
    public int DefaultPasswordMinLength { get; set; }
    public string PasswordGenerationChars { get; set; }
    public string GoodreadsRssBaseUrl { get; set; }
    public string GoodreadsProfielUrlPrevix { get; set; }
    public string DefaultCulture { get; set; }
    public int GoodreadsFetchIntervalMinutes { get; set; }
    public bool UseExternalFriendlyPasswordGenerator { get; set; }
    public string SiteName { get; set; }
    public bool EnablePeriodicBackup { get; set; }
    public int PeriodicBackupIntervalHours { get; set; }

    public CultureInfo DefaultCultureInfo => new CultureInfo(DefaultCulture);

    public SigningCredentials SigningCreds { get; private set; }
}
