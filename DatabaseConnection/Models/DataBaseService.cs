using DatabaseConnection;
using IntegrationApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationApi.Models
{
    public class DataBaseService : IDataBaseService
    {
        private DataBase _dataBase;
        public List<Entry> InsertIntoDataBase(int pageNumber)
        {
            List<Entry> offers = ReturnSpecificOffers(pageNumber);

            using (_dataBase = new DataBase())
            {
                foreach (var offer in offers)
                {
                    _dataBase.Entries.Add(offer);
                }
                _dataBase.SaveChanges();
            }
            return offers;
        }
        public List<Entry> ReturnSpecificOffers(int pageNumber)
        {
            List<Entry> offers = ReturnOffersList();
            int od = 20 * pageNumber;
            List<Entry> chosenOffers = new List<Entry>();
            for (int i = od; i < od + 20; i++)
            {
                chosenOffers.Add(offers[i]);
            }
            return chosenOffers;
        }
        public List<Entry> ReturnOffersList()
        {
            var path = ReturnPath();
            Dump offers;
            using (StreamReader document = new StreamReader(path))
            {
                offers = JsonConvert.DeserializeObject<Dump>(document.ReadToEnd());
            }
            return offers.Entries.ToList();
        }
        public string ReturnPath()
        {
            string path = @"..\DziennikBaltycki\bin\Debug\netcoreapp3.1\Dziennik Bałtycki";
            string path2 = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));
            string[] files = Directory.GetFiles(path2);
            return $"{path}{files[0].Split("Dziennik Bałtycki")[1]}";
        }
        public void WriteLogs(string value)
        {
            using (_dataBase = new DataBase())
            {
                _dataBase.Logs.Add(new Log
                {
                    Time = DateTime.Now,
                    Content = value
                });
                _dataBase.SaveChanges();
            }
        }
        public List<Entry> ReturnRecords(int recordsLimit, int whichPage)
        {
            using (_dataBase = new DataBase())
            {
                return _dataBase.Entries.Skip((recordsLimit * whichPage) - recordsLimit).Take(recordsLimit)
                   .Include(x => x.OfferDetails).ThenInclude(od => od.SellerContact)
                   .Include(x => x.PropertyPrice)
                   .Include(x => x.PropertyDetails)
                   .Include(x => x.PropertyAddress)
                   .Include(x => x.PropertyFeatures)
                   .ToList();
            }
        }
        public List<Entry> ReturnRecords()
        {
            using (_dataBase = new DataBase())
            {
                return _dataBase.Entries
                   .Include(x => x.OfferDetails).ThenInclude(od => od.SellerContact)
                   .Include(x => x.PropertyPrice)
                   .Include(x => x.PropertyDetails)
                   .Include(x => x.PropertyAddress)
                   .Include(x => x.PropertyFeatures)
                   .ToList();
            }
        }
        public Entry ReturnRecord(int id)
        {
            using (_dataBase = new DataBase())
            {
                return _dataBase.Entries
                   .Where(x => x.ID == id)
                   .Include(x => x.OfferDetails).ThenInclude(od => od.SellerContact)
                   .Include(x => x.PropertyPrice)
                   .Include(x => x.PropertyDetails)
                   .Include(x => x.PropertyAddress)
                   .Include(x => x.PropertyFeatures)
                   .Single();
            }
        }
        public bool UpdateRecords(Entry rekord)
        {
            bool isExist = IsExist(rekord.ID);

            if (isExist)
            {
                using (_dataBase = new DataBase())
                {
                    _dataBase.Entries.Update(rekord);
                    _dataBase.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsExist(int id)
        {
            using (_dataBase = new DataBase())
            {
                if (_dataBase.Entries.FirstOrDefault(x => x.ID == id) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
