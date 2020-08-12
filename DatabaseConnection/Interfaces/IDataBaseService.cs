using Models;
using System.Collections.Generic;

namespace IntegrationApi.Interfaces
{
    public interface IDataBaseService
    {
        bool UpdateRecords(Entry rekord);
        bool IsExist(int id);
        List<Entry> InsertIntoDataBase(int nrStrony);
        void WriteLogs(string value);
        List<Entry> ReturnOffersList();
        Entry ReturnRecord(int id);
        List<Entry> ReturnRecords();
        List<Entry> ReturnRecords(int limitRekordow, int ktoraStrona);
        string ReturnPath();
        List<Entry> ReturnSpecificOffers(int nrStrony);
    }
}