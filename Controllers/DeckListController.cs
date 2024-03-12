using System.Globalization;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class DeckListController : Controller{
    public IActionResult Index(){
        return View();
    }

    public IActionResult ShoppingList(IFormFile postedFile){
        if(postedFile == null || postedFile.Length == 0){
            return BadRequest("No file selected for upload");
        }
        else if(Path.GetExtension(postedFile.FileName) != ".csv"){
            return BadRequest("Incorrect filetype, choose a csv file.");
        }

        List<string> cardNames = new List<string>();
        using(TextFieldParser csvParser = new TextFieldParser(postedFile.OpenReadStream())){
            csvParser.SetDelimiters(new string[] { "," });
            //csvParser.HasFieldsEnclosedInQuotes = true;

            //TODO: Likely not the best way to do this in a using
            while(!csvParser.EndOfData){
                cardNames.AddRange(csvParser.ReadFields());
            }
        }

        foreach(string name in cardNames){
            Console.WriteLine($"{name}\n");
        }

        List<string> cardLinks = new List<string>();

        foreach (string name in cardNames){
            if(name.Contains("&")){
                string adjustedName = name.Replace("&", "%26").Replace(" ", "+");
                cardLinks.Add(@$"https://yugiohprices.com/card_price?name={adjustedName}");
            }
            else {
                string adjustedName = name.Replace(" ", "%20");
                cardLinks.Add(@$"https://yugiohprices.com/card_price?name={adjustedName}");
            }
        }

        List<CardInfo> shoppingList = new List<CardInfo>();

        HtmlWeb web = new HtmlWeb();

        foreach(string link in cardLinks) {
            Console.WriteLine(link + "\n");
            var htmlDoc = web.Load(link);
            string cardTitle = "";


            try {
                cardTitle = htmlDoc.DocumentNode
                    .SelectSingleNode("//h1[@id='item_name']").InnerText;

                var cardResult = htmlDoc.DocumentNode
                    .SelectSingleNode("//table[@id='other_merchants']")
                    .SelectSingleNode("//tr[@id='']")
                    .Descendants("p");

                shoppingList.Add(new CardInfo(cardResult, cardTitle));
            } catch (Exception ex){
                Console.WriteLine(ex.Message);

                shoppingList.Add(new CardInfo(cardTitle));

                continue;
            }
        }
        decimal sum = 0.00M;

        foreach(var card in shoppingList){
            sum += Decimal.Parse(card.Price, NumberStyles.AllowCurrencySymbol | NumberStyles.Currency, new CultureInfo("en-US"));
        }

        ViewBag.ShoppingList = shoppingList;
        ViewBag.FinalPrice = sum; 
        return View();
    }
}