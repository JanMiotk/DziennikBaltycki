using HtmlAgilityPack;
using Models;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IDziennikBaltyckiService
    {
        int? CheckHowManyMeters(string whatAreWeLookingFor, ref string opis);
        HtmlNode DownloadDocument(string url);
        List<string> DownloadPropertyLinks(List<string> websiteLinks);
        List<string> DownloadWebsiteLinks();
        PolishCity ReturnCityName(string convertedCity);
        Dictionary<string, string> ReturnDataCollection(HtmlNode htmlBody);
        string ReturnDescription(HtmlNode htmlBody);
        int? ReturnIndoorParking(Dictionary<string, string> dataCollection, string opis);
        OfferDetails ReturnOfferDetails(HtmlNode htmlbody, string url);
        int? ReturnOutdoorParking(Dictionary<string, string> dataCollection);
        int ReturnPagesAmount(HtmlNode htmlbody);
        PropertyAddress ReturnPropertyAddress(Dictionary<string, string> dataCollection);
        PropertyDetails ReturnPropertyDetails(Dictionary<string, string> dataCollection);
        PropertyFeatures ReturnPropertyFeatures(HtmlNode htmlbody, Dictionary<string, string> zbiorDanych);
        PropertyPrice ReturnPropertyPrice(HtmlNode htmlbody, Dictionary<string, string> zbiorDanych);
        string ReturnSingleHousingAdvertisment(HtmlNode link);
    }
}