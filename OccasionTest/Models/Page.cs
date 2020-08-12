using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropertiesFinderTests.Models
{
    public class Page : IPage
    {
        public Dictionary<string, string> ReturnDictionary()
        {
            return new Dictionary<string, string>()
            {
                {"powierzchnia" , "135" },
                {"liczbaPokoi" , "7" },
                {"pietro" , "2" },
                {"rokBudowy" , "2019" },
            };
        }
        public HtmlDocument ReturnEmptyDocument()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(@"<body> </body>");

            return htmlDocument;
        }
        public HtmlDocument ReturnDescription()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(@"<body> <div class='description__rolled ql-container'>Jesli ci szwankuje zdrowie mozesz wezwac pogotowie ratownicy tak
            sie spiesza ze grabarze az sie ciesza Zrobia zastrzyk z pavilonu
            i nie wrocisz juz do domu</div> </body>");

            return htmlDocument;
        }
        public string ReturnIndoorParking()
        {
            return "Była plama, nie ma bluzki, bo ten proszek byl od ruskich gArAż";
        }
        public string ZwocDanneDoOdczytaniaPowierzchniOgrodu()
        {
            return @"Ten jest najszlachetniejszy
                w całym ludzkim tłumie
                kto oprócz łez swoich
                łzy innych rozumie ogród 300 m";
        }
    }
}
