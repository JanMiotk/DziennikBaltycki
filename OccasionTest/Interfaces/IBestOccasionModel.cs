using Models;
using System.Collections.Generic;

namespace PropertiesFinderTests.Interfaces
{
    public interface IBestOccasionModel
    {
        List<Entry> NoResault();
        List<Entry> OneResult();
        List<Entry> TheLowestPriceFromEveryCity();
    }
}