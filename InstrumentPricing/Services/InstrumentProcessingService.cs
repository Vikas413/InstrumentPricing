using System;
using System.Threading;
using System.Threading.Tasks;
using InstrumentPricing.Interfaces;
using InstrumentPricing.Models.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class InstrumentProcessingService : BackgroundService
{
    private readonly ILogger<InstrumentProcessingService> logger;
    private readonly IInstrumentReader reader;
    private readonly InstrumentProcessingSettings config;

    public InstrumentProcessingService(
        IInstrumentReader reader,
        ILogger<InstrumentProcessingService> logger,
        IOptions<InstrumentProcessingSettings> settings
        )
    {
        this.logger = logger;
        this.reader = reader;
        this.config = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug($"InstrumentProcessingService is starting.");

        stoppingToken.Register(() =>
            logger.LogDebug($" InstrumentProcessingService background task is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug($"InstrumentProcessingService task doing background work.");

            var count  = reader.ReadInstruments();

            logger.LogDebug($"{count} new entries added");

            await Task.Delay(config.UpdateInterval, stoppingToken);
        }

        logger.LogDebug($"InstrumentProcessingService background task is stopping.");
    }
}
