using DatabaseConnection.Interfaces;
using IntegrationApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationApi.Models
{
    public class Occasion : IOccasion
    {
        private IDataBase _dataBase { get; }
        public Occasion(IDataBase dataBase)
        {
            _dataBase = dataBase;
        }
        public List<Entry> ReturnTheBestOffers()
        {
            return _dataBase.Entries
           .Include(x => x.OfferDetails).ThenInclude(od => od.SellerContact)
           .Include(x => x.PropertyPrice)
           .Include(x => x.PropertyDetails)
           .Include(x => x.PropertyAddress)
           .Include(x => x.PropertyFeatures)
           .Where(x => x.PropertyPrice.PricePerMeter != 0)
           .OrderBy(x => x.PropertyPrice.PricePerMeter)
           .AsEnumerable()
           .GroupBy(x => x.PropertyAddress.City)
           .Select(x => x.First())
           .ToList();
        }

    }
}
