using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialEvolutionDataCollector.Services;
using SocialEvolutionSensor.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class TimedHostedService : IHostedService, IDisposable
{
    private IConfiguration _config;
    private readonly ILogger _logger;
    private Timer _timer;
    private IDataCollectorService<Call> _callCollectorService;
    private IDataCollectorService<SMS> _smsCollectorService;

    public TimedHostedService(
        IConfiguration configuration,
        IDataCollectorService<Call> callCollectorService,
        IDataCollectorService<SMS> smsCollectorService,
        ILogger<TimedHostedService> logger)
    {
        _config = configuration;
        _callCollectorService = callCollectorService;
        _smsCollectorService = smsCollectorService;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is starting.");
        int period = int.Parse(_config.GetSection("GlobalConsts").GetSection("COLLECTION_CYCLE_PERIOD").Value);
        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(period));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Timed Background Service is working.");
        _callCollectorService.CollectDataAsync();
        _smsCollectorService.CollectDataAsync();

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Background Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}