using System;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace WebScraper {
    
    class Program {
        public class CardInfo {
        public string CardName {get; set; }
        public string SellerSite {get; set; }
        public string Edition {get; set; }
        public string Rarity {get; set; }
        public string Condition {get; set; }
        public string ListingLink {get; set; }
        public string Price {get; set; }

        public CardInfo(IEnumerable<HtmlNode> CardResult, string cardname){
            CardName = cardname;
            SellerSite = CardResult.ElementAt(0).InnerText.Replace("\n", "");
            Edition = CardResult.ElementAt(1).InnerText;
            Rarity = CardResult.ElementAt(2).InnerText;
            Condition = CardResult.ElementAt(3).InnerText;
            ListingLink = CardResult.ElementAt(4).Element("a").Attributes["href"].Value;
            Price = CardResult.ElementAt(5).InnerText;
        }

        override public string ToString(){
            return $"CardName: \t\t\t{CardName}\nSeller: \t\t\t{SellerSite}\nEdition: \t\t\t{Edition}\nRarity: \t\t\t{Rarity}\nCondition: \t\t\t{Condition}\nListing Link: \t\t{ListingLink}\nPrice: \t\t\t\t{Price}";
        }
    }
    
        static int Main(string[] args){
            HtmlWeb web = new HtmlWeb();

            string[] cardList = File.ReadAllLines("./MockData.txt");
            List<CardInfo> shoppingList = new List<CardInfo>();

            foreach (string cardName in cardList){
                Thread.Sleep(100);
                string html = @$"https://yugiohprices.com/card_price?name={cardName.Replace(" ", "%20")}";
                var htmlDoc = web.Load(html);

                var cardResult = htmlDoc.DocumentNode
                .SelectSingleNode("//table[@id='other_merchants']")
                .SelectSingleNode("//tr[@id='']")
                .Descendants("p");

                shoppingList.Add(new CardInfo(cardResult, cardName));
            }
            
            using(TextWriter writer = new StreamWriter("./ShoppingFile.txt")){
                foreach (var item in shoppingList){
                    writer.WriteLine(item);
                }
            }
            
            // File.WriteAllLines("./ShoppingFile.txt", (IEnumerable<string>)shoppingList);

            return 0;
        }
    }
}