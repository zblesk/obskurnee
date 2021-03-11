using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Config _config;
        private UserService _userService;
        private ReviewService _reviews;
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
            using (var scope = _scopeFactory.CreateScope())
            {
                _userService = (UserService)scope.ServiceProvider.GetService(typeof(UserService));
                _config = (Config)scope.ServiceProvider.GetService(typeof(Config));
                _reviews = (ReviewService)scope.ServiceProvider.GetService(typeof(ReviewService));
                _logger.LogInformation("Feed fetcher service started. Fetch interval: {min} minutes",
                    _config.GoodreadsFetchIntervalMinutes);

                _timer = new Timer(FetchFeeds,
                    null,
                    TimeSpan.FromMinutes(3), // let the app startup finish first
                    TimeSpan.FromMinutes(_config.GoodreadsFetchIntervalMinutes));
                return Task.CompletedTask;
            }
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
                _logger.LogInformation("Starting RSS fetch");
                foreach (var user in _userService.GetAllUsersAsBookworm())
                {
                    _reviews.FetchReviewUpdates(user);
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
