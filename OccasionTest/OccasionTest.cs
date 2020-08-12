using IntegrationApi.Models;
using PropertiesFinderTests.Models;
using Models;
using NUnit.Framework;
using System.Collections.Generic;
using DatabaseConnection.Interfaces;
using Moq;
using PropertiesFinderTests.Interfaces;

namespace PropertiesFinderTests
{
    [TestFixture]
    public class OccasionTest
    {
        private Occasion _occasion;

        private Mock<IDataBase> _mock;

        private IBestOccasionModel _bestOcassion;

        private IDbSet _dbSet;

        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IDataBase>();
            _bestOcassion = new BestOccasionModel();
            _dbSet = new DbSet();
            _occasion = new Occasion(_mock.Object);
        }
        [Test]
        public void ReturnTheBestOffers_CheckOneCity_TheLowestPriceFromEveryCity()
        {
            // Arrange

            var offers = _bestOcassion.TheLowestPriceFromEveryCity();

            var data = _dbSet.ReturnData(offers);

            _mock.Setup(x => x.Entries).Returns(data.Object);

            // Act 

            var result = _occasion.ReturnTheBestOffers();

            //Assert

            Assert.AreEqual(result[0].PropertyPrice.PricePerMeter, 50000);
            Assert.AreEqual(result[0].PropertyAddress.City.ToString(), "GDANSK");

        }
        [Test]
        public void ReturnTheBestOffers_CheckingBehaviorForPricesEqual0_NoResult()
        {
            // Arrange

            var offers = _bestOcassion.NoResault();

            var data = _dbSet.ReturnData(offers);

            _mock.Setup(x => x.Entries).Returns(data.Object);

            // Act 

            var result = _occasion.ReturnTheBestOffers();

            //Assert

            Assert.IsEmpty(result);

        }
        [Test]
        public void ReturnTheBestOffers_CheckingBehaviorForEmptyCollection_NoResultForEmptyCollection()
        {
            // Arrange

            List<Entry> offers = new List<Entry>();

            var data = _dbSet.ReturnData(offers);

            _mock.Setup(x => x.Entries).Returns(data.Object);

            // Act  

            var result = _occasion.ReturnTheBestOffers();

            //Assert

            Assert.IsEmpty(result);
        }
        [Test]
        public void ReturnTheBestOffers_EveryCityGenerateOneResult_OneResult()
        {
            // Arrange

            var offers = _bestOcassion.OneResult();

            var data = _dbSet.ReturnData(offers);

            _mock.Setup(x => x.Entries).Returns(data.Object);

            // Act 

            var result = _occasion.ReturnTheBestOffers();

            //Assert

            Assert.AreEqual(result.Count, 3);
        }
    }
}
