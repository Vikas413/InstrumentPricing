using System;
using System.Collections;
using System.Collections.Generic;
using InstrumentPricing.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InstrumentPricing.InstrumentCache
{
    public class InstrumentCache : IEnumerable<PricingHistory>
    { 
        private ILogger<InstrumentCache> logger;
        private Dictionary<string, PricingHistory> instruments;
        public InstrumentCache(ILogger<InstrumentCache> logger)
        {
            this.logger = logger;
            this.instruments = new Dictionary<string, PricingHistory>();
        }

        public int Count {
            get { return this.instruments.Count; }
             }

        public void Add(string name, double value)
        {
            if(!this.instruments.ContainsKey(name))
            {
                this.instruments.Add(name, new PricingHistory(name, 10, 5));
            }

            this[name]?.Add(value);
        }

        public PricingHistory this[string name]{
            get {
                if (this.instruments.ContainsKey(name))
                    return this.instruments[name];
                else
                    return null;
            }
        }

        public IEnumerator<PricingHistory> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
