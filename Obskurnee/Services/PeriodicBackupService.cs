using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
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
                Config.Current.PeriodicBackupIntervalHours);
            _timer = new Timer(MakeBackup,
                null,
                TimeSpan.FromHours(Config.Current.PeriodicBackupIntervalHours),
                TimeSpan.FromHours(Config.Current.PeriodicBackupIntervalHours));
            return Task.CompletedTask;
        }

        private void MakeBackup(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            var backups = (BackupService)scope.ServiceProvider.GetService(typeof(BackupService));
            _logger.LogInformation("Starting Periodic backup");
            try
            {
                backups.CreateBackup().Wait();
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