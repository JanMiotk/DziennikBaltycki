using DatabaseConnection.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DatabaseConnection
{
    public class DataBase : DbContext, IDataBase
    {
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Log> Logs { get; set; }

        private readonly string _connectionString;

        public DataBase()
        {
            using (var file = new StreamReader("appsettings.json"))
            {
                var json = file.ReadToEnd();
                var config = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
                _connectionString = config["ConnectionStrings"]["Sql"];
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
