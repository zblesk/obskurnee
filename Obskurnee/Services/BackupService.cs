using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class BackupService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<BackupService> _logger;

        public BackupService(
            ILogger<BackupService> logger,
            ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task CreateBackup(string filename = null)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = $"obskurnee.{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.db";
            }
            var dir = Path.Combine(Config.DataFolder, Config.BackupFolder);
            if (!Directory.Exists(dir))
            {
                _logger.LogInformation("Creating Backup directory at {backupDir}", dir);
                Directory.CreateDirectory(dir);
            }
            var path = Path.Combine(dir, filename);
            _logger.LogInformation("Backing up to {fname}", path);
            return _db.Database.ExecuteSqlRawAsync($"VACUUM INTO '{path}';");
        }
    }
}
