using Microsoft.Extensions.Logging;
using Obskurnee.Models;

namespace Obskurnee.Services;

public class SettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly ApplicationDbContext _db;

    public SettingsService(
        ILogger<SettingsService> logger,
        ApplicationDbContext database)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = database ?? throw new ArgumentNullException(nameof(database));
    }

    public void UpsertSetting(string key, string value)
    {
        _logger.LogInformation("Updating setting '{key}'", key);
        var existing = _db.Settings.FirstOrDefault(s => s.Key == key);
        if (existing == null)
        {
            _db.Settings.Add(new Setting
            {
                Key = key,
                Value = value,
                LastChange = DateTime.UtcNow
            });
        }
        else
        {
            existing.Value = value;
            existing.LastChange = DateTime.UtcNow;
            _db.Settings.Update(existing);
        }
        _db.SaveChanges();
    }

    public string GetSetting(string key) => _db.Settings.FirstOrDefault(s => s.Key == key)?.Value;

    public T GetSettingValue<T>(string key)
    {
        var setting = _db.Settings.FirstOrDefault(s => s.Key == key);
        if (setting == null)
        {
            return default(T);
        }
        if (typeof(T).IsEnum)
        {
            return (T)Enum.Parse(typeof(T), setting.Value);
        }
        return (T)Convert.ChangeType(setting.Value, typeof(T));
    }
}
