using DziennikBaltycki.DziennikBaltycki;
using PropertiesFinderTests.Models;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Utilities;

namespace PropertiesFinderTests
{
    [TestFixture]
    public class DziennikBaltyckiServiceTests
    {
        private IPage _page;

        private DziennikBaltyckiService _dziennikBaltyckiService;

        [SetUp]
        public void SetUp()
        {
            _page = new Page();
            _dziennikBaltyckiService = new DziennikBaltyckiService();
        }
        [Test]
        public void ReturnOfferDetails_CorrectParsing_CorrectData()
        {
            // Arrange

            var model = _page.ReturnDictionary();

            // Act 

            var result = _dziennikBaltyckiService.ReturnPropertyDetails(model);

            //Assert

            Assert.AreEqual(result.Area, 135);
            Assert.AreEqual(result.NumberOfRooms, 7);
            Assert.AreEqual(result.FloorNumber, 2);
            Assert.AreEqual(result.YearOfConstruction, 2019);

        }
        [Test]
        public void ReturnOfferDetails_NoSearchingElements_DefaultValues()
        {
            // Arrange

            var htmlBody = _page.ReturnEmptyDocument().DocumentNode.SelectSingleNode("//body");
            // Act 

            var result = _dziennikBaltyckiService.ReturnOfferDetails(htmlBody, null);

            //Assert

            Assert.AreEqual(result.SellerContact.Telephone, "brak informacji");
            Assert.AreEqual(result.SellerContact.Name, "brak informacji");

        }
        [Test]
        public void ReturnDescription_DescriptionAsExpected_CorrectResult()
        {
            // Arrange

            var model = _page.ReturnDescription();

            var htmlBody = model.DocumentNode.SelectSingleNode("//body");

            // Act 

            var result = _dziennikBaltyckiService.ReturnDescription(htmlBody);

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result, @"Jesli ci szwankuje zdrowie mozesz wezwac pogotowie ratownicy tak
            sie spiesza ze grabarze az sie ciesza Zrobia zastrzyk z pavilonu
            i nie wrocisz juz do domu");

        }
        [Test]
        public void ReturnIndoorParking_DescriptionDataCorrectlyRead_ValuesAreEqual()
        {
            // Arrange

            var model = _page.ReturnIndoorParking();

            // Act 

            var rezultat = _dziennikBaltyckiService.ReturnIndoorParking(new Dictionary<string, string>(), model);

            //Assert

            Assert.IsNotNull(rezultat);
            Assert.AreEqual(rezultat, 1);
        }
        [Test]
        public void CheckHowManyMeters_CorrectlyReadDataFromDescription_ValueIsCorrect()
        {
            // Arrange

            var model = _page.ZwocDanneDoOdczytaniaPowierzchniOgrodu();

            // Act 

            var rezultat = _dziennikBaltyckiService.CheckHowManyMeters("ogród", ref model);

            //Assert

            Assert.IsNotNull(rezultat);
            Assert.AreEqual(rezultat, 300);
        }
    }
}

