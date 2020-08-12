using Models;
using System.Collections.Generic;
using PropertiesFinderTests.Interfaces;

namespace PropertiesFinderTests.Models
{
    public class BestOccasionModel : IBestOccasionModel
    {

        public List<Entry> TheLowestPriceFromEveryCity()
        {
            return new List<Entry>
            {
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 100000,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 50000,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 9999999,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },

            };
        }

        public List<Entry> NoResault()
        {
            return new List<Entry>
            {
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.SOPOT

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDYNIA

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.WEJHEROWO

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 0,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.RUMIA

                    }
                },

            };
        }

        public List<Entry> OneResult()
        {
            return new List<Entry>
            {
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 100000,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 30023,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDANSK

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 5670,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDYNIA

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 9840,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.GDYNIA

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 4590,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.RUMIA

                    }
                },
                new Entry
                {
                    PropertyPrice = new PropertyPrice
                    {
                        PricePerMeter = 7860,
                    },
                    PropertyAddress = new PropertyAddress
                    {
                        City = PolishCity.RUMIA

                    }
                },

            };
        }
    }
}
