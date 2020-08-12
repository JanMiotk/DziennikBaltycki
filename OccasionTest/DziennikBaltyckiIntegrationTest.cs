using DziennikBaltycki.DziennikBaltycki;
using PropertiesFinderTests.Models;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace PropertiesFinderTests
{
    [TestFixture]
    public class DziennikBaltyckiIntegrationTest
    {
        private Mock<IDziennikBaltyckiService> _mock;

        private DziennikBaltyckiIntegration _dziennikBaltyckiIntegration;

        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IDziennikBaltyckiService>();
            _dziennikBaltyckiIntegration = new DziennikBaltyckiIntegration(null, null, _mock.Object);
        }
        [Test]
        public void GenerateDump_EmptyPage_ResultIsEmpty()
        {
            // Arrange

            HtmlNode htmlbody = null;

            _mock.Setup(x => x.DownloadWebsiteLinks()).Returns(new List<string> { null });
            _mock.Setup(x => x.DownloadPropertyLinks(new List<string> { null })).Returns(new List<string> { "www.nieIstnieje.pl" });
            _mock.Setup(x => x.DownloadDocument("www.nieIstnieje.pl")).Returns(htmlbody);

            // Act 

            var result = _dziennikBaltyckiIntegration.GenerateDump();

            //Assert

            Assert.AreEqual(result.Entries.ToList().Count(), 0);
        }
    }
}

