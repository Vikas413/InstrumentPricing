using InstrumentPricing.InstrumentCache;
using InstrumentPricing.Models.Config;
using InstrumentPricing.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InstrumentPricingTest
{
    [TestClass]
    public class InsrumentReaderTest
    {
        private IOptions<InstrumentInputs> settings;
        private InstrumentCache cache;
        private InstrumentReader reader;

        [TestInitialize]
        public void Init()
        {
            settings = Options.Create(new InstrumentInputs() { Path = "/Users/vikas/Projects/InstrumentPricing/Input/data1.txt" });
            cache = new InstrumentCache(new Mock<ILogger<InstrumentCache>>().Object);
            reader = new InstrumentReader(cache, settings, new Mock<ILogger<InstrumentReader>>().Object);
        }

        [TestMethod]
        public void ReadInstruments()
        {
            var count = this.reader.ReadInstruments();
            
            Assert.IsTrue(count > 0);
            Assert.AreEqual(this.cache.Count, 5);
        }

        [TestMethod]
        public void UpdateFileAndCheckInstrumentCountAndAverage()
        {
            string[] content = new string[] { "A:5", "A:5", "A:5", "A:5", "A:5" };
            var count = this.reader.ReadInstruments();

            Assert.IsTrue(count > 0);
            Assert.AreEqual(this.cache.Count, 5);
            System.IO.File.WriteAllLines(this.settings.Value.Path, content);
            count = this.reader.ReadInstruments();
            Assert.AreEqual(5, count);
            Assert.AreEqual(6, cache.Count);
            Assert.AreEqual(5, cache["A"].DefaultAverage);
        }

        [TestCleanup]
        public void TearDown()
        {
            this.reader.Dispose();
        }
    }
}
