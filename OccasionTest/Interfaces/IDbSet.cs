using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;

namespace PropertiesFinderTests.Interfaces
{
    public interface IDbSet
    {
        Mock<DbSet<Entry>> ReturnData<Entry>(ICollection<Entry> kolekcja) where Entry : class;
    }
}