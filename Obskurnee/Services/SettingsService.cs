using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class SettingsService
    {
        private readonly ILogger<SettingsService> _logger;
        private readonly Database _db;

        public SettingsService(
            ILogger<SettingsService> logger,
            Database database)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
        }

        public bool UpsertSetting(Setting setting) => _db.Settings.Upsert(setting);

        public dynamic GetSetting(string key) => _db.Settings.FindById(key)?.Value;

        public T GetSettingValue<T>(string key)
        {
            var setting = _db.Settings.FindById(key);
            return (T)Convert.ChangeType(setting, typeof(T));
        }
    }
}
