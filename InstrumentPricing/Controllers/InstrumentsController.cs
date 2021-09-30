using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InstrumentPricing.Controllers
{
    [ApiController]
    [Route("info/api/v1")]
    public class InstrumentsController : ControllerBase
    {
        private ILogger<InstrumentsController> logger;
        private InstrumentCache.InstrumentCache cache;

        public InstrumentsController(ILogger<InstrumentsController> logger, InstrumentPricing.InstrumentCache.InstrumentCache cache)
        {
            this.logger = logger;
            this.cache = cache;
        }
        
        [HttpGet, Route("instrument/{name}")]
        public dynamic Instrument(string name)
        {
            if(this.cache[name] != null)
            {
                var instrument = this.cache[name];
                return new
                {
                    Name = name,
                    Price = instrument.Last,
                    Average = instrument.DefaultAverage,
                    Direction = instrument.Direction.ToString()
                }; 
            }

            return null;
        }

        [HttpGet, Route("instrument/{name}/history")]
        public double[] InstrumentHistory(string name)
        {
            if (this.cache[name] != null)
            {
                var instrument = this.cache[name];
                return instrument.Prices;
            }

            return null;
        }
    }
}
