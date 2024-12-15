using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using drunkShop.Models;
using drunkShop.Data;
using drunkShop.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using drunkShop.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace drunkShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;
    private List<ShoppingCart> shoppingCartList;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(ILogger<HomeController> logger, AppDbContext db, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _db = db;
    }


    public IActionResult Index()
    {
        HomeVM homeVm = new HomeVM()
        {
            Products = _db.Product
            .Include(u => u.Category)
            .Include(u => u.ApplicationType),
            Categories = _db.Category
        };
        return View(homeVm);
    }


    public IActionResult Details(int id)
    {
        var shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) ?? new List<ShoppingCart>();

        DetailsVM detailsVM = new DetailsVM()
        {
            Product = _db.Product
                .Include(u => u.Category)
                .Include(u => u.ApplicationType)
                .FirstOrDefault(u => u.Id == id),
            ExistsInCard = shoppingCartList.Any(item => item.ProductId == id) 
        };

        return View(detailsVM);
    }



    [HttpPost, ActionName("Details")]
    public IActionResult DetailsPost(int id)
    {
        var shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) ?? new List<ShoppingCart>();

        if (!shoppingCartList.Any(item => item.ProductId == id))
        {
            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
        }

        return RedirectToAction(nameof(Index));
    }




    public IActionResult RemoveFromCard(int id)
    {
        var shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) ?? new List<ShoppingCart>();

        var removedItem = shoppingCartList.SingleOrDefault(r => r.ProductId == id);

        if (removedItem != null)
        {
            shoppingCartList.Remove(removedItem);
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
        }

        return RedirectToAction(nameof(Index)); 
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

