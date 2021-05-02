using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class PeriodicBackupService : IHostedService, IDisposable
    {
        private readonly ILogger<PeriodicBackupService> _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private const int backupIntervalHours = 1;

        public PeriodicBackupService(
            ILogger<PeriodicBackupService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Periodic Backup Service started. Backup interval: every {hrs} hours",
                backupIntervalHours);
            _timer = new Timer(MakeBackup,
                null,
                TimeSpan.FromHours(backupIntervalHours),
                TimeSpan.FromHours(backupIntervalHours));
            return Task.CompletedTask;
        }

        private void MakeBackup(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            var backups = (BackupService)scope.ServiceProvider.GetService(typeof(BackupService));
            _logger.LogInformation("Starting Periodic backup");
            try
            {
                backups.CreateBackup();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Backup creation failed");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Periodic Backup Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}