using Models;
using System.Collections.Generic;

namespace IntegrationApi.Interfaces
{
    public interface IOccasion
    {
        List<Entry> ReturnTheBestOffers();
    }
}