using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class FeedFetcherService : IHostedService, IDisposable
    {
        private readonly ILogger<FeedFetcherService> _logger;
        private Timer _timer;
        private bool isRunning = false;
        private readonly object @lock = new object();
        private readonly IServiceScopeFactory _scopeFactory;

        public FeedFetcherService(
            ILogger<FeedFetcherService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var config = (Config)scope.ServiceProvider.GetService(typeof(Config));
            _logger.LogInformation("Feed fetcher service started. Fetch interval: {min} minutes",
                config.GoodreadsFetchIntervalMinutes);
            _timer = new Timer(FetchFeeds,
                null,
                TimeSpan.FromMinutes(3), // let the app startup finish first
                TimeSpan.FromMinutes(config.GoodreadsFetchIntervalMinutes));
            return Task.CompletedTask;
        }

        private void FetchFeeds(object state)
        {
            lock (@lock)
            {
                if (isRunning)
                {
                    _logger.LogWarning("Previous job still running. Exitting.");
                    return;
                }
                isRunning = true;
            }
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var reviews = (ReviewService)scope.ServiceProvider.GetService(typeof(ReviewService));
                var userService = (UserService)scope.ServiceProvider.GetService(typeof(UserService));
                _logger.LogInformation("Starting RSS fetch");
                foreach (var user in userService.GetAllUsersAsBookworm())
                {
                    reviews.FetchReviewUpdates(user).Wait();
                }
            }
            finally
            {
                lock (@lock)
                {
                    isRunning = false;
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
