using System;
using System.Collections.Generic;
using InstrumentPricing.Models;

namespace InstrumentPricing.InstrumentCache
{
    public class PricingHistory
    {
        private string name;
        private int size;
        private List<double> prices;
        private int index;
        private int defaultAveragePricesCount;
        public double defaultAverageTotal;

        public PricingHistory(string intsrumentName, int size)
        {
            this.name = intsrumentName;
            this.size = size;
            this.prices = new List<double>();
            this.index = 0;
        }

        public PricingHistory(string intsrumentName, int size, int defaultAveragePricesCount): this(intsrumentName, size)
        {
            this.defaultAveragePricesCount = defaultAveragePricesCount;
        }

        public int Count { get { return this.index; } }
        public int Size
        {
            get
            {
                return this.size;
            }
        }

        public double First
        {
            get
            { return prices[0]; }
        }


        public double Last { get {
                if (this.size == this.prices.Count)
                    return this.prices[this.size -1];
                else
                    return this.prices[this.prices.Count - 1];
            }
        }

        public PriceDirection Direction { get; private set; }
        public double DefaultAverage { get; private set; }
        public double[] Prices { get { return this.prices.ToArray(); } }

        public void Add(double val)
        {
            double lastVal;
            this.SetDefaultAverage(val);
            if (this.index < this.Size)
            {
                if (index == 0)
                {
                    
                    lastVal = val;
                }
                else
                {
                    lastVal = prices[index - 1];
                    
                }

                prices.Add(val);
                index++;
            }
            else
            {
                lastVal = prices[this.size - 1];
                prices.RemoveAt(0);
                prices.Add(val);
            }

            this.Direction = lastVal > val ? PriceDirection.Down : lastVal < val ? PriceDirection.Up : PriceDirection.NoChange;
            
        }

        private void SetDefaultAverage(double val)
        {
            if (this.defaultAveragePricesCount > 0)
            {
                if (this.index < this.Size)
                {
                    if (index == 0)
                    {
                        this.DefaultAverage = val;
                        this.defaultAverageTotal = val;

                    }
                    else if (index < this.defaultAveragePricesCount)
                    {
                        this.defaultAverageTotal += val;
                        this.DefaultAverage = this.defaultAverageTotal / (index + 1);
                    }
                    else if(index == this.defaultAveragePricesCount)
                    {
                        //this.defaultAverageTotal -= this.prices[this.size - this.defaultAveragePricesCount - 1];
                        this.defaultAverageTotal = this.defaultAverageTotal + val - prices[0];
                        this.DefaultAverage = this.defaultAverageTotal / this.defaultAveragePricesCount;
                    }
                    else
                    { 
                        this.defaultAverageTotal = this.defaultAverageTotal + val - prices[index - defaultAveragePricesCount];
                        this.DefaultAverage = this.defaultAverageTotal / this.defaultAveragePricesCount;
                    }

                }
                else
                {
                    this.defaultAverageTotal -= this.prices[this.defaultAveragePricesCount - 1];
                    this.defaultAverageTotal += val;
                    this.DefaultAverage = this.defaultAverageTotal / this.defaultAveragePricesCount;
                }
            }
        }
    }
}
