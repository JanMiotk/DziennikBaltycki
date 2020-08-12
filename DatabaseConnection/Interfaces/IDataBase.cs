using Microsoft.EntityFrameworkCore;
using Models;

namespace DatabaseConnection.Interfaces
{
    public interface IDataBase
    {
        DbSet<Entry> Entries { get; set; }
        DbSet<Log> Logs { get; set; }
    }
}