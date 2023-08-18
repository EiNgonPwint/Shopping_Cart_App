using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_Team5.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_Team5.Controllers
{
    public class UserCartController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            string sessionId = Request.Cookies["SessionId"];
            User user = LoginData.GetUserBySession(sessionId);
            List<Cart> cartLists = ProductData.GetUserCart(user.UserId);
            ViewData["cartlist"] = cartLists;
            ViewData["userId"] = user.UserId;
            ViewData["user"]=user;
            return View();
        }
    }
}

