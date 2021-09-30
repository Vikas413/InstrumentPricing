using System;
using InstrumentPricing.InstrumentCache;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InstrumentPricingTest
{
    [TestClass]
    public class InstrumentCacheTest
    {
        private ILogger<InstrumentCache> logger;

        [TestInitialize]
        public void Init()
        {
            this.logger = new Mock<ILogger<InstrumentCache>>().Object;
        }
        [TestMethod]
        public void CreateInstrumentCacheAndThereShouldBeNoInstruments()
        {
            var cache = new InstrumentCache(this.logger);
            Assert.AreEqual(cache.Count, 0);

        }

        [TestMethod]
        public void AddInstrumnetandCheckCount()
        {
            var cache = new InstrumentCache(this.logger);
            cache.Add("Oil", 3);
            Assert.AreEqual(cache.Count, 1);

        }

        [TestMethod]
        public void AddInstrumnetandPriceHistoryAndCheck()
        {
            var cache = new InstrumentCache(this.logger);
            cache.Add("Oil", 5);
            Assert.AreEqual(cache.Count, 1);
            cache.Add("Oil", 3);
            cache.Add("Oil", 1);
            Assert.AreEqual(cache["Oil"]?.DefaultAverage, 3);

            cache.Add("VOD.l", 5);
            cache.Add("VOD.l", 5);
            cache.Add("VOD.l", 5);
            cache.Add("VOD.l", 5);
            Assert.AreEqual(cache.Count, 2);
            Assert.AreEqual(cache["VOD.l"]?.DefaultAverage, 5);
        }
    }
}
