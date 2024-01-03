using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System.Diagnostics;
using System.Threading;

namespace Obskurnee.Services;

public class FeedFetcherService : IHostedService, IDisposable
{
    private readonly ILogger<FeedFetcherService> _logger;
    private Timer _timer;
    private bool isRunning = false;
    private readonly object @lock = new object();
    private Stopwatch timer = null;
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
        if (config.GoodreadsFetchIntervalMinutes < 1)
        {
            _logger.LogInformation("Feed Fetcher Service disabled.");
            return Task.CompletedTask;
        }
#if DEMOMODE
                _logger.LogInformation("Feed Fetcher Service disabled in DEMO mode.");
                return Task.CompletedTask;
#endif
        _logger.LogInformation("Feed Fetcher Service started. Fetch interval: {min} minutes",
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
                _logger.LogWarning("Previous job still running. Has been running for {time} minutes ({time_h} hours). Exitting.",
                    timer?.Elapsed.TotalMinutes, timer?.Elapsed.TotalHours);
                return;
            }
            isRunning = true;
        }
        try
        {
            if (timer != null)
            {
                timer.Stop();
            }
            else
            {
                timer = new Stopwatch();
            }
            timer.Restart();
            using var scope = _scopeFactory.CreateScope();
            var reviews = (ReviewService)scope.ServiceProvider.GetService(typeof(ReviewService));
            var userService = (UserServiceBase)scope.ServiceProvider.GetService(typeof(UserServiceBase));
            _logger.LogInformation("Starting RSS fetch");
            foreach (var user in userService.GetAllUsersAsBookworm())
            {
                reviews.FetchReviewUpdates(user).Wait();
            }
            _logger.LogInformation("RSS fetch finished after {time} seconds", timer.Elapsed.TotalSeconds);
            timer.Stop();
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
        _logger.LogInformation("Feed Fetcher Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
