using HtmlAgilityPack;

namespace MvcMovie.Models;

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

        public CardInfo(string cardname, string sellerSite, string edition, string rarity, string condition, string listinglink, string price){
            CardName = cardname;
            SellerSite = sellerSite;
            Edition = edition;
            Rarity = rarity;
            Condition = condition;
            ListingLink = listinglink;
            Price = price;
        }
}