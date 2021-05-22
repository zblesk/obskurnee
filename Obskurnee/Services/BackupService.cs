using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Serilog;
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
        private readonly Serilog.ILogger _logger;

        public BackupService(
            Serilog.ILogger logger,
            ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public BackupService(
            ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = Log.Logger;
        }

        public Task CreateBackup(string filename = null)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = $"obskurnee.{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.db";
            }
            var dir = Path.Combine(Config.DataFolder, Config.BackupFolder);
            if (!Directory.Exists(dir))
            {
                _logger.Information("Creating Backup directory at {backupDir}", dir);
                Directory.CreateDirectory(dir);
            }
            var path = Path.Combine(dir, filename);
            _logger.Information("Backing up to {fname}", path);
            return _db.Database.ExecuteSqlRawAsync($"VACUUM INTO '{path}';");
        }
    }
}
