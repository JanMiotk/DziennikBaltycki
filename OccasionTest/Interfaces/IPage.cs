using HtmlAgilityPack;
using System.Collections.Generic;

namespace PropertiesFinderTests.Models
{
    public interface IPage
    {
        string ZwocDanneDoOdczytaniaPowierzchniOgrodu();
        string ReturnIndoorParking();
        HtmlDocument ReturnDescription();
        HtmlDocument ReturnEmptyDocument();
        Dictionary<string, string> ReturnDictionary();
    }
}