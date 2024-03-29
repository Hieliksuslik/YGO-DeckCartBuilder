using System.Diagnostics;
using System.Globalization;
using AngleSharp;
using AngleSharp.Dom;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<ViewResult> ShoppingList(IFormFile postedFile)
    {
        if (postedFile == null || postedFile.Length == 0)
        {
           // TODO
        }
        else if (Path.GetExtension(postedFile.FileName) != ".csv")
        {
            //TODO
        }

        List<string> cardNames = new List<string>();
        using (TextFieldParser csvParser = new TextFieldParser(postedFile.OpenReadStream()))
        {
            csvParser.SetDelimiters(new string[] { "," });
            //csvParser.HasFieldsEnclosedInQuotes = true;

            //TODO: Likely not the best way to do this in a using
            while (!csvParser.EndOfData)
            {
                cardNames.AddRange(csvParser.ReadFields());
            }
        }

        List<string> cardLinks = new List<string>();

        foreach (string name in cardNames)
        {
            if (name.Contains("&"))
            {
                string adjustedName = name.Replace("&", "%26").Replace(" ", "+");
                cardLinks.Add(@$"https://yugiohprices.com/card_price?name={adjustedName}");
            }
            else
            {
                string adjustedName = name.Replace(" ", "%20");
                cardLinks.Add(@$"https://yugiohprices.com/card_price?name={adjustedName}");
            }
        }

        List<CardInfo> shoppingList = new List<CardInfo>();
        
        //TODO: Refactor from AgilityPack to AngleSharp, and try to solve carrying over the name.
        /*
            Have the entire process iterate sequentially instead of building
            a list of links?

            Original process:
            Generate a list of links using the imported card names.
            Iterate through the list of new links, building the shopping cart as it goes.
            Issue: Current implementation prevented the ability to show exactly which cards didn't have data from
            primary source.
        */

        // Load the default Configuration
        var config = Configuration.Default.WithDefaultLoader();

        // Create a new browsing context
        var context = BrowsingContext.New(config);

        // Where the http request happens. Returns IDocument which can be queried.
        var document = await context.OpenAsync("https://yugiohprices.com/card_price?name=Arias+the+Labrynth+Butler");

        /*
            Get the first table labeled "other_merchant".
            Get the tbody of it.
            Get the first tr.
            Then a list of all td elements of that tr.
            This results in still getting the cheapest card, but without relying on xpath.
        */
        try
        {
            IElement? singleTableRow = document.QuerySelector("table#other_merchants tbody tr");

            if (singleTableRow == null)
            {
                Console.WriteLine("Missing card listing!");
            }

            Console.WriteLine();

            //CardInfo card1 = new CardInfo(singleTableRow);
            //Console.WriteLine(card1.ToString());

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        };

        /*
        HtmlWeb web = new HtmlWeb();

        
        foreach(string link in cardLinks) {
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
        */
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
