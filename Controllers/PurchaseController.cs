using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_Team5.Data;
using CA_Team5.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_Team5.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(int userid)
        {
            string sessionId = Request.Cookies["SessionId"];
            User user = LoginData.GetUserBySession(sessionId);
            List<Cart> cartLists = ProductData.GetCartById(user.UserId);
            ProductData.AddToPurchase(cartLists, userid);
            ProductData.RemoveFromCart(userid);
            List<Product> products = ProductData.GetAllProducts();
            List<PurchaseHistory> purchasehistory = PurchaseData.GetPurchaseHistoryByCustomerId(user.UserId);
            List<Product> distinctproduct = PurchaseData.Distinct(user.UserId);
           
            ViewData["user"] = user;
            ViewData["products"] = products;
            ViewData["PurchaseHistory"] = purchasehistory;
            ViewData["DistinctProducts"] = distinctproduct;

            return View();
        }
        public IActionResult Checkout(int userid)
        {
            string sessionId = Request.Cookies["SessionId"];
            User user = LoginData.GetUserBySession(sessionId);
            List<Cart> cartLists = ProductData.GetCartById(user.UserId);
            ProductData.AddToPurchase(cartLists, userid);
            ProductData.RemoveFromCart(userid);
            return View("Views/Purchase/Index.cshtml");
        }

    }
}

