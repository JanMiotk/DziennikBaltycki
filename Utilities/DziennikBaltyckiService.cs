using HtmlAgilityPack;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public class DziennikBaltyckiService : IDziennikBaltyckiService
    {
        private WebClient _client { get; set; }
        private HtmlAttribute _attribute { get; set; }
        public HtmlNode DownloadDocument(string url)
        {
            HtmlDocument document = new HtmlDocument();
            using (_client = new WebClient())
            {
                var content = _client.DownloadString($@"{url}");
                document.LoadHtml(content);
            }
            return document.DocumentNode.SelectSingleNode("//body");
        }
        public int ReturnPagesAmount(HtmlNode htmlbody)
        {
            var pagesAmount = htmlbody.SelectSingleNode("//a[@class=\"strzalka ostatniaStrz\"]");
            var pattern = new Regex(@"(.)(\w+)(.)(\d+)(.)(\d+)(.)(\d+),n,fm,pk.html");
            _attribute = pagesAmount.Attributes["href"];
            Match returnedPattern = pattern.Match(_attribute.Value);

            return Convert.ToInt32(returnedPattern.Groups[4].Value);
        }
        public string ReturnSingleHousingAdvertisment(HtmlNode link)
        {
            _attribute = link.Attributes["href"];

            if (_attribute != null)
            {
                var element = link.SelectSingleNode("//header");
                var price = element.Element("p");

                if (price != null)
                {
                    string @new = price.InnerText.Split(",")[0].Replace(" ", string.Empty);

                    if (Convert.ToInt32(@new) < 500)
                    {
                        return null;
                    }
                    if (_attribute.Value.ToLower().Contains(@"pomieszczenia-biurowe-na-wynajem") || _attribute.Value.ToLower().Contains(@"lokal")
                        || _attribute.Value.ToLower().Contains(@"hala") || _attribute.Value.ToLower().Contains(@"dzialka"))
                    {
                        return null;
                    }
                }
                return _attribute.Value;
            }
            return null;
        }
        public List<string> DownloadWebsiteLinks()
        {
            HtmlNode htmlBody = DownloadDocument("https://dziennikbaltycki.pl/ogloszenia/12261,8433,fm,pk.html");
            var section = htmlBody.SelectSingleNode("//section[@id='ogloszenia-miasta']");
            List<string> websiteLinks = new List<string>();
            websiteLinks.Add(@"12261,8433,n,fm,pk.html");

            foreach (var link in section.Descendants("li"))
            {
                _attribute = link.FirstChild.Attributes["href"];

                if (_attribute != null)
                {
                    string linkPart = _attribute.Value.Split("/")[4];
                    string address = $@"{linkPart.Split(",")[0]},{linkPart.Split(",")[1]},n,fm,pk.html";
                    websiteLinks.Add(address);
                }
            }
            return websiteLinks;
        }

        //Odwiedzam 3 strony w każdym z 10 miast i wyciągam po 5 ofert, jeśli pasują do kryteriów. Spowodowane jest to ogromną ilością ofert 
        public List<string> DownloadPropertyLinks(List<string> websiteLinks)
        {
            string url = @"https://dziennikbaltycki.pl/ogloszenia/";
            int citiesAmount = 0;

            List<string> housingLinks = new List<string>();

            foreach (var link in websiteLinks)
            {
                int cityPagesAmount = 1;
                int pagesAmount = ReturnPagesAmount(DownloadDocument($"{url}{link}"));

                for (int i = 1; i <= pagesAmount; i++)
                {
                    HtmlNode htmlBody = DownloadDocument($"{url}{i},{link}");
                    var section = htmlBody.SelectSingleNode("//section[@id='lista-ogloszen']/ul");
                    int linksAmount = 1;

                    foreach (var linkA in section.Descendants("a"))
                    {
                        string property = ReturnSingleHousingAdvertisment(linkA);
                        if (property != null)
                            housingLinks.Add(_attribute.Value);
                        if (linksAmount == 5)
                        {
                            break;
                        }
                        linksAmount++;
                    }
                    if (cityPagesAmount == 3)
                    {
                        break;
                    }
                    cityPagesAmount++;
                }
                if (citiesAmount == 10)
                {
                    break;
                }
                citiesAmount++;
            }
            return housingLinks;
        }
        public PolishCity ReturnCityName(string convertedCity)
        {
            return (PolishCity)Enum.Parse(typeof(PolishCity), convertedCity);
        }
        public PropertyAddress ReturnPropertyAddress(Dictionary<string, string> dataCollection)
        {
            string normalization = dataCollection["miasto"].Normalize(NormalizationForm.FormD);
            string convertedCity = new string(normalization.Where(c => c < 128).ToArray());

            if (Enum.IsDefined(typeof(PolishCity), convertedCity))
            {
                return new PropertyAddress
                {
                    City = ReturnCityName(convertedCity),
                    District = dataCollection["dzielnica"].Trim(),
                    StreetName = dataCollection["nazwaUlicy"].Trim(),
                    //Brak danych na stronie
                    DetailedAddress = "Brak danych"
                };
            }
            else
            {
                return new PropertyAddress
                {
                    District = dataCollection["dzielnica"].Trim(),
                    StreetName = dataCollection["nazwaUlicy"].Trim(),
                    //Brak danych na stronie
                    DetailedAddress = "Brak danych"
                };
            }

        }
        public int? CheckHowManyMeters(string whatAreWeLookingFor, ref string opis)
        {
            int startPossition = opis.LastIndexOf(whatAreWeLookingFor) + whatAreWeLookingFor.Length + 1;
            string checkingArea = opis.Substring(startPossition, opis.Length - startPossition > 10 ? 10 : opis.Length - startPossition);

            if (checkingArea.Any(x => char.IsDigit(x)))
            {
                bool stop = false;
                string temp = null;
                foreach (var mark in checkingArea)
                {
                    if (char.IsDigit(mark))
                    {
                        temp += mark;
                        stop = true;
                    }
                    if (stop == true && mark == 'm')
                    {
                        break;
                    }
                }
                return Convert.ToInt32(temp);
            }
            else
            {
                return null;
            }
        }
        public OfferDetails ReturnOfferDetails(HtmlNode htmlbody, string url)
        {
            var phone = htmlbody.SelectSingleNode("//a[@class='phoneButton__button']");
            var seller = htmlbody.SelectSingleNode("//div[@class='offerOwner__details']/h3[@class='offerOwner__person ']");
            var whatPrice = htmlbody.SelectSingleNode("//span[@class='priceInfo__value']");
            var bruttoPrice = whatPrice != null && whatPrice.InnerText.Any(x => char.IsDigit(x)) ? Convert.ToDecimal(whatPrice.InnerText.Split("zł")[0].Replace(" ", string.Empty)) : 0;

            OfferKind sale = bruttoPrice == 0 || bruttoPrice > 10000 ? OfferKind.SALE : OfferKind.RENTAL;

            return new OfferDetails
            {
                Url = url,
                CreationDateTime = DateTime.Now,
                LastUpdateDateTime = DateTime.Now,
                OfferKind = sale,
                SellerContact = new SellerContact
                {
                    Telephone = phone != null ? phone.Attributes["data-full-phone-number"].Value.Trim() : "brak informacji",
                    Name = seller != null ? seller.InnerText.Trim() : "brak informacji"
                },
                IsStillValid = true
            };
        }
        public PropertyPrice ReturnPropertyPrice(HtmlNode htmlbody, Dictionary<string, string> zbiorDanych)
        {
            var price = htmlbody.SelectSingleNode("//span[@class='priceInfo__value']");
            var pricePerMeter = htmlbody.SelectSingleNode("//span[@class='priceInfo__additional']");

            return new PropertyPrice
            {
                TotalGrossPrice = price != null && price.InnerText.Any(x => char.IsDigit(x)) ? Convert.ToDecimal(price.InnerText.Split("zł")[0].Replace(" ", string.Empty)) : 0,
                PricePerMeter = pricePerMeter != null ? Convert.ToDecimal(pricePerMeter.InnerText.Split("zł")[0].Replace(",", ".").Replace(" ", string.Empty).Replace("\n", string.Empty), new CultureInfo("en-US")) : 0,
                ResidentalRent = zbiorDanych.ContainsKey("oplaty") ? Convert.ToDecimal(zbiorDanych["oplaty"]) : 0
            };
        }
        public PropertyDetails ReturnPropertyDetails(Dictionary<string, string> dataCollection)
        {
            return new PropertyDetails
            {
                Area = dataCollection.ContainsKey("powierzchnia") ? dataCollection["powierzchnia"] != null ? Convert.ToDecimal(dataCollection["powierzchnia"], new CultureInfo("en-US")) : 0 : 0,
                NumberOfRooms = dataCollection.ContainsKey("liczbaPokoi") ? dataCollection["liczbaPokoi"].All(x => char.IsDigit(x)) ? Convert.ToInt32(dataCollection["liczbaPokoi"]) :
                dataCollection["liczbaPokoi"].Any(x => char.IsDigit(x)) ? Convert.ToInt32(dataCollection["liczbaPokoi"].FirstOrDefault(x => char.IsDigit(x)).ToString()) : 0 : 0,
                FloorNumber = dataCollection.ContainsKey("pietro") && dataCollection["pietro"].All(x => char.IsDigit(x)) ? Convert.ToInt32(dataCollection["pietro"]) : 0,
                YearOfConstruction = dataCollection.ContainsKey("rokBudowy") ? Convert.ToInt32(dataCollection["rokBudowy"]) : 0,
            };
        }
        public int? ReturnOutdoorParking(Dictionary<string, string> dataCollection)
        {
            return dataCollection.ContainsKey("miejsceParkingowe") ? !dataCollection["miejsceParkingowe"].Contains("podziemn") && !dataCollection["miejsceParkingowe"].Contains("garaż") && !dataCollection["miejsceParkingowe"].Contains("brak") ?
                dataCollection.ContainsKey("liczbaMiejscParkingowych") ? Convert.ToInt32(dataCollection["liczbaMiejscParkingowych"]) : 1 : 0 : 0;
        }
        public int? ReturnIndoorParking(Dictionary<string, string> dataCollection, string opis)
        {
            return dataCollection.ContainsKey("miejsceParkingowe") ? dataCollection["miejsceParkingowe"].Contains("podziemn") || dataCollection["miejsceParkingowe"].Contains("garaż") ?
                dataCollection.ContainsKey("liczbaMiejscParkingowych") ? Convert.ToInt32(dataCollection["liczbaMiejscParkingowych"]) : 1 : 0 : opis.ToLower().Contains("podziemn") || opis.ToLower().Contains("garaż") ? 1 : 0;
        }
        public PropertyFeatures ReturnPropertyFeatures(HtmlNode htmlbody, Dictionary<string, string> zbiorDanych)
        {
            var opis = ReturnDescription(htmlbody);

            return new PropertyFeatures
            {
                GardenArea = opis.Contains("ogród") ? CheckHowManyMeters("ogród", ref opis) : null,
                Balconies = opis.Contains("balkon") ? 1 : 0,
                BasementArea = opis.Contains("piwnica") ? CheckHowManyMeters("piwnica", ref opis) : opis.Contains("Komórka lokatorska") ? CheckHowManyMeters("piwnica", ref opis) : null,
                OutdoorParkingPlaces = ReturnOutdoorParking(zbiorDanych),
                IndoorParkingPlaces = ReturnIndoorParking(zbiorDanych, opis)
            };
        }
        public Dictionary<string, string> ReturnDataCollection(HtmlNode htmlBody)
        {
            var parametersList = htmlBody.SelectSingleNode("//ul[@class='parameters__rolled']");
            var adress = htmlBody.SelectSingleNode("//h1[@class='sticker__title']");
            Dictionary<string, string> dataCollection = new Dictionary<string, string>();

            foreach (var parametr in parametersList.Descendants("li"))
            {
                if (parametr.Element("span") != null && parametr.Element("b") != null)
                {
                    var spanContent = parametr.Element("span").InnerText;
                    var bContent = parametr.Element("b").InnerText;

                    if (spanContent.ToLower().Contains("lokalizacja"))
                    {
                        var data = parametr.Descendants("a").ToList();

                        dataCollection.Add("nazwaUlicy", adress.InnerText.Contains("ul.") ? adress.InnerText.Split("ul.")[1] : "Brak informacji");

                        dataCollection.Add("miasto", data[0].InnerHtml.Replace(" ", "_").ToUpper());

                        dataCollection.Add("dzielnica", data.Count == 3 ? data[1].InnerText : "brak informacji");
                    }
                    if (spanContent.ToLower().Contains("powierzchnia w m2"))
                    {
                        dataCollection.Add("powierzchnia", bContent.Split("m")[0].Replace(",", ".").Replace(" ", string.Empty));
                    }
                    if (spanContent.ToLower().Contains("liczba pokoi"))
                    {
                        dataCollection.Add("liczbaPokoi", bContent);
                    }
                    if (spanContent.ToLower().Contains("piętro"))
                    {
                        dataCollection.Add("pietro", bContent);
                    }
                    if (spanContent.ToLower().Contains("opłaty"))
                    {
                        dataCollection.Add("oplaty", bContent.Split("zł")[0]);
                    }
                    if (spanContent.ToLower().Contains("rok budowy"))
                    {
                        dataCollection.Add("rokBudowy", bContent);
                    }
                    if (spanContent.ToLower().Contains("miejsce parkingowe"))
                    {
                        dataCollection.Add("miejsceParkingowe", bContent);
                    }
                    if (spanContent.ToLower().Contains("liczba miejsc parkingowych"))
                    {
                        dataCollection.Add("liczbaMiejscParkingowych", bContent);
                    }
                }
            }

            return dataCollection;
        }
        public string ReturnDescription(HtmlNode htmlBody)
        {
            var description = htmlBody.SelectSingleNode("//div[@class='description__rolled ql-container']");
            return description != null ? description.InnerText.Trim() : null;
        }

    }
}
