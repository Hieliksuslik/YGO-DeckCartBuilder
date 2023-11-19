using System;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using System.Linq;
using System.Diagnostics;

namespace WebScraper {
    
    class Program {
        public class CardInfo {
        public string SellerSite {get; set; }
        public string Edition {get; set; }
        public string Rarity {get; set; }
        public string Condition {get; set; }
        public string ListingLink {get; set; }
        public string Price {get; set; }

        public CardInfo(IEnumerable<HtmlNode> CardResult){
            SellerSite = CardResult.ElementAt(0).InnerText.Replace("\n", "");
            Edition = CardResult.ElementAt(1).InnerText;
            Rarity = CardResult.ElementAt(2).InnerText;
            Condition = CardResult.ElementAt(3).InnerText;
            ListingLink = CardResult.ElementAt(4).Element("a").Attributes["href"].Value;
            Price = CardResult.ElementAt(5).InnerText;
        }

        override public string ToString(){
            return $"Seller: \t\t{SellerSite}\nEdition: \t\t{Edition}\nRarity: \t\t{Rarity}\nCondition: \t\t{Condition}\nListing Link: \t\t{ListingLink}\nPrice: \t\t\t{Price}";
        }
    }
    
        static int Main(string[] args){
            var html = @"https://yugiohprices.com/card_price?name=Dark%20Magician";
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var cardResult = htmlDoc.DocumentNode
                .SelectSingleNode("//table[@id='other_merchants']")
                .SelectSingleNode("//tr[@id='']")
                .Descendants("p");

            CardInfo card1 = new CardInfo(cardResult);
            Console.WriteLine(card1);

            return 0;
        }
    }
}