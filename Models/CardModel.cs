using AngleSharp.Dom;
using HtmlAgilityPack;

namespace MvcMovie.Models;

public class CardInfo {
        private string? _listingLink;
        public string CardName {get; set; }
        public string SellerSite {get; set; }
        public string Edition {get; set; }
        public string Rarity {get; set; }
        public string Condition {get; set; }
        public string? ListingLink {
            get => _listingLink;
            set{
                if(value == null){
                    _listingLink = "Missing Link";
                }
                else
                    _listingLink = value;
            } 
        }
        public string Price {get; set; }
        public bool HasListing {get; set;}

        public CardInfo(IHtmlCollection<IElement> cardDetails, string cardName){
            CardName = cardName;
            SellerSite = cardDetails[0].TextContent;
            Edition = cardDetails[1].TextContent;
            Rarity = cardDetails[2].TextContent;
            Condition = cardDetails[3].TextContent;
            ListingLink = cardDetails[4]!.QuerySelector("a")!.GetAttribute("href")!.ToString();
            Price = cardDetails[5].TextContent;
            HasListing = true;
        }

        public CardInfo(string cardName){
            CardName = cardName;
            SellerSite = "N/A";
            Edition = "N/A";
            Rarity = "N/A";
            Condition = "N/A";
            ListingLink = "No Listing";
            Price = "$0.00";
            HasListing = false;
        }
}