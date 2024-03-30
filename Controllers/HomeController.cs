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

        // Load the default Configuration
        var config = Configuration.Default.WithDefaultLoader();

        // Create a new browsing context
        var context = BrowsingContext.New(config);
        
        List<CardInfo> shoppingList = new List<CardInfo>();
        foreach (string name in cardNames)
        {
            string siteLink;
            if (name.Contains('&'))
            {
                string adjustedName = name.Replace("&", "%26").Replace(" ", "+");
                siteLink = @$"https://yugiohprices.com/card_price?name={adjustedName}";
            }
            else
            {
                string adjustedName = name.Replace(" ", "+");
                siteLink = @$"https://yugiohprices.com/card_price?name={adjustedName}";
            }
            
            // Where the http request happens. Returns IDocument which can be queried.
            var document = await context.OpenAsync(siteLink);

            try
            {
                IElement? singleTableRow = document.QuerySelector("table#other_merchants tbody tr");

                if (singleTableRow == null)
                {
                    shoppingList.Add(new CardInfo(name));
                    continue;
                }

                IHtmlCollection<IElement> cardDetails = singleTableRow.QuerySelectorAll("td");
                shoppingList.Add(new CardInfo(cardDetails, name));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }
        
        /*
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
