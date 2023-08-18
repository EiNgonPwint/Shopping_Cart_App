using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CA_Team5.Models;
using CA_Team5.Data;

namespace CA_Team5.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        
        string sessionId = Request.Cookies["SessionId"];
        User user = LoginData.GetUserBySession(sessionId);
        bool count2 = false;
        foreach (var cookie in Request.Cookies.Keys)
        {
            if (cookie != "SessionId")
            {
                count2 = Guid.TryParse(cookie, out _) is true ? true : ProductData.RemoveFromCart(0);

            }
        }
        if (Request.Cookies.Keys.Count == 0)
        {
            ProductData.RemoveFromCart(0);
        }
        if (user?.FirstName == null)
        {
            string guest = Guid.NewGuid().ToString();
            Response.Cookies.Append("SessionId",guest);
            user = null;
            List<Product> products1 = ProductData.GetAllProducts();
            Dictionary<string,int> avgRate1 = RatingData.GetAvg();
            ViewData["avg"] = avgRate1;
            ViewData["products"] = products1;
            return View();
        }
        List<Product> products = ProductData.GetAllProducts();
        Dictionary<string, int> avgRate = RatingData.GetAvg();
        ViewData["avg"] = avgRate;
        ViewData["products"] = products;
        ViewData["user"] = user;

        return View();
        
    }

    public IActionResult ViewCart()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Search(string text)
    {
        string sessionId = Request.Cookies["SessionId"];
        User user = LoginData.GetUserBySession(sessionId);
        List<Product> search = ProductData.GetAllMatches(text);
        Dictionary<string, int> avgRate = RatingData.GetAvg();
     
        ViewData["avg"] = avgRate;
        ViewData["user"] = user;
        ViewData["search"] = search;
        ViewData["searchText"] = text;
        return View();
    }
    public IActionResult AddToCart(string productId)
    {
        int status;
        //string productId = form["ProductId"];
        string sessionId = Request.Cookies["SessionId"];
        User user = LoginData.GetUserBySession(sessionId);
        if (user.UserId is 0)
        {
            Guid guid =Guid.NewGuid();
            Response.Cookies.Append(guid.ToString(), productId);
        }
        Product product = ProductData.GetProductById(productId);
        status=ProductData.ToCart(user,product);
        return status>0 ? Ok(status) : Ok(0);
    }
    public IActionResult RemoveFromCart(string productId)
    {
        int status;
        //string productId = form["ProductId"];
        
        string sessionId = Request.Cookies["SessionId"];
        User user = LoginData.GetUserBySession(sessionId);
        Product product = ProductData.GetProductById(productId);
       
        status = ProductData.FromCart(user, product);
        return status > 0 ? Ok(status) : Content("fail");
    }
    public IActionResult GetProductQty(string productId)
    {
        string sessionId = Request.Cookies["SessionId"];
        User user = LoginData.GetUserBySession(sessionId);
        Product product = ProductData.GetProductById(productId);
        int pdtqty = ProductData.GetProductQty(user, product);
        ViewData["pdtqty"] = pdtqty;
        return Ok(pdtqty);
    }
    public IActionResult GetCount()
    {
    
        string sessionId = Request.Cookies["SessionId"];
        if (sessionId == null)
        {
            int count1 = 0;
            foreach (var cookie in Request.Cookies.Keys)
            {
                count1 += Guid.TryParse(cookie, out _) is true ? 1 : 0;
            }
            return Ok(count1);
        }
        User user = LoginData.GetUserBySession(sessionId);

        int count =  ProductData.GetCount(user.UserId);
        //ViewData["count"] = count;

        return Ok(count);
     
    }

}

