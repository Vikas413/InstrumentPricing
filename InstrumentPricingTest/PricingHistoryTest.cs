using System;
using InstrumentPricing.InstrumentCache;
using InstrumentPricing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InstrumentPricingTest
{
    [TestClass]
    public class PricingHistoryTest
    {
        [TestMethod]
        public void PricingHistorySizecheck()
        {
            var history = new PricingHistory("Oil", 10);
            Assert.AreEqual(history.Size, 10);

        }
            [TestMethod]
        public void PricingHistoryShouldAddNewValue()
        {
            var history = new PricingHistory("Oil", 10);
            history.Add(5);
            Assert.AreEqual(1, history.Count);
        }
        //[TestMethod]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        //public void PricingHJistoryShouldNotKeepMoreValuesThenIntendedFor()
        //{
        //    var history = new PricingHistory("Oil", 2);
        //    history.Add(5);
        //    history.Add(2);
        //    history.Add(7);
        //}

        [TestMethod]
        public void PricingHJistoryShouldReplaceOldValue()
        {
            var history = new PricingHistory("Oil", 3);
            history.Add(5);
            history.Add(2);
            history.Add(7);
            history.Add(10);
            Assert.AreEqual(history.First, 2);
            Assert.AreEqual(history.Last, 10);
        }

        [TestMethod]
        public void PricingHJistoryShouldShoudProvidCorrectDirection()
        {
            var history = new PricingHistory("Oil", 3);
            history.Add(5);
            history.Add(2);
            history.Add(7);
            history.Add(10);
            Assert.AreEqual(history.First, 2);
            Assert.AreEqual(history.Last, 10);
            Assert.AreEqual(history.Direction, PriceDirection.Up);
            history.Add(2);
            Assert.AreEqual(history.First, 7);
            Assert.AreEqual(history.Last, 2);
            Assert.AreEqual(history.Direction, PriceDirection.Down);
        }

        [TestMethod]
        public void PricingHJistoryShouldShoudProvidCorrectDefaultAverage()
        {
            var history = new PricingHistory("Oil", 5, 3);
            history.Add(5);
            history.Add(2);
            Assert.AreEqual(history.Direction, PriceDirection.Down);
            Assert.AreEqual(history.DefaultAverage, 3.5);
            history.Add(8);
            Assert.AreEqual(history.DefaultAverage, 5);

            history.Add(8);
            
            Assert.AreEqual(history.DefaultAverage, 6);

            history.Add(8);
            Assert.AreEqual(history.DefaultAverage, 8);
            history.Add(11);
            Assert.AreEqual(history.First, 2);
            Assert.AreEqual(history.Last, 11);
            Assert.AreEqual(history.Direction, PriceDirection.Up);
            Assert.AreEqual(history.DefaultAverage, 9);
            history.Add(2);
            Assert.AreEqual(history.First, 8);
            Assert.AreEqual(history.Last, 2);
            Assert.AreEqual(history.Direction, PriceDirection.Down);
            Assert.AreEqual(history.DefaultAverage, 7);
        }



    }
}
