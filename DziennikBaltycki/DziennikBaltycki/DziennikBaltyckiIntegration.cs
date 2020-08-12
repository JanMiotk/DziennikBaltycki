using HtmlAgilityPack;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DziennikBaltycki.DziennikBaltycki
{

    public class DziennikBaltyckiIntegration : IWebSiteIntegration
    {
        public WebPage WebPage { get; }
        public IDumpsRepository DumpsRepository { get; }
        public IEqualityComparer<Entry> EntriesComparer { get; }
        public IDziennikBaltyckiService DziennikBaltyckiService { get; }
        public DziennikBaltyckiIntegration() { }
        public DziennikBaltyckiIntegration(IDumpsRepository dumpsRepository,
            IEqualityComparer<Entry> equalityComparer, IDziennikBaltyckiService dziennikBaltyckiService)
        {
            DumpsRepository = dumpsRepository;
            EntriesComparer = equalityComparer;
            DziennikBaltyckiService = dziennikBaltyckiService;
            WebPage = new WebPage
            {
                Url = "https://dziennikbaltycki.pl/ogloszenia/12261,8433,fm,pk.html",
                Name = "Dziennik Bałtycki",
                WebPageFeatures = new WebPageFeatures
                {
                    HomeSale = true,
                    HomeRental = true,
                    HouseSale = true,
                    HouseRental = true
                }
            };
        }
        public Dump GenerateDump()
        {
            List<string> pageLinks = DziennikBaltyckiService.DownloadWebsiteLinks();
            List<string> propertyLinks = DziennikBaltyckiService.DownloadPropertyLinks(pageLinks);
            List<Entry> property = new List<Entry>();

            foreach (var url in propertyLinks)
            {
                HtmlNode htmlbody = DziennikBaltyckiService.DownloadDocument(url);

                if (htmlbody == null)
                {
                    continue;
                }
                Console.WriteLine(url);
                Dictionary<string, string> dataCollection = DziennikBaltyckiService.ReturnDataCollection(htmlbody);

                property.Add(new Entry
                {
                    OfferDetails = DziennikBaltyckiService.ReturnOfferDetails(htmlbody, url),
                    PropertyPrice = DziennikBaltyckiService.ReturnPropertyPrice(htmlbody, dataCollection),
                    PropertyDetails = DziennikBaltyckiService.ReturnPropertyDetails(dataCollection),
                    PropertyAddress = DziennikBaltyckiService.ReturnPropertyAddress(dataCollection),
                    PropertyFeatures = DziennikBaltyckiService.ReturnPropertyFeatures(htmlbody, dataCollection),
                    RawDescription = DziennikBaltyckiService.ReturnDescription(htmlbody),
                });
            }
            return new Dump
            {
                WebPage = WebPage,
                DateTime = DateTime.Now,
                Entries = property
            };
        }
    }
}


