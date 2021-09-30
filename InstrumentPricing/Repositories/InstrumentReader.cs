using System;
using System.IO;
using InstrumentPricing.Interfaces;
using InstrumentPricing.Models.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InstrumentPricing.Repositories
{
    public class InstrumentReader : IInstrumentReader, IDisposable
    {
        private readonly InstrumentCache.InstrumentCache cache;
        private ILogger<InstrumentReader> logger;
        private InstrumentInputs config;
        private FileStream file;

        public InstrumentReader(InstrumentCache.InstrumentCache cache, IOptions<InstrumentInputs> settings, ILogger<InstrumentReader> logger)
        {
            this.cache = cache;
            this.logger = logger;
            this.cache = cache;
            this.config = settings.Value;
            
            
        }

        public void Dispose()
        {
           //todo:release resources if any.
        }

        public int ReadInstruments()
        {
            int totalRecords = 0;
            this.logger.LogDebug($"Reading file from {config.Path}");
            try
            {
                //todo: if file not changed do not read````
                using (this.file = new FileStream(this.config.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    this.file.Seek(0, 0);
                    var reader = new StreamReader(this.file);
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var instrument = line.Split(":");
                        double val;
                        if (!string.IsNullOrWhiteSpace(instrument[0]) && double.TryParse(instrument[1], out val))
                        {
                            this.cache.Add(instrument[0].Trim(), val);
                            totalRecords++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error while reading file", ex);
            }
            finally
            {

            }

            logger.LogInformation($"data file exists : {File.Exists(config.Path)}");

            return totalRecords;
        }
    }
}
