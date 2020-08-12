using Microsoft.EntityFrameworkCore;
using Moq;
using PropertiesFinderTests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertiesFinderTests.Models
{
    public class DbSet : IDbSet
    {
        public Mock<DbSet<Entry>> ReturnData<Entry>(ICollection<Entry> collection) where Entry : class
        {
            var record = new Mock<DbSet<Entry>>();
            record.As<IQueryable<Entry>>().Setup(m => m.Provider).Returns(collection.AsQueryable().Provider);
            record.As<IQueryable<Entry>>().Setup(m => m.Expression).Returns(collection.AsQueryable().Expression);
            record.As<IQueryable<Entry>>().Setup(m => m.ElementType).Returns(collection.AsQueryable().ElementType);
            record.As<IQueryable<Entry>>().Setup(m => m.GetEnumerator()).Returns(collection.AsQueryable().GetEnumerator());
            record.Setup(m => m.Add(It.IsAny<Entry>())).Callback<Entry>(collection.Add);
            return record;
        }
    }
}
