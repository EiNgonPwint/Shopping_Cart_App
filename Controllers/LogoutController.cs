using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_Team5.Data;
using Microsoft.AspNetCore.Mvc;


namespace CA_Team5.Controllers
{
    public class LogoutController : Controller
    {
       
       
        public IActionResult Index()
        {
           
            if (Request.Cookies["SessionId"] != null)
            {

                LoginData.RemoveSession(Request.Cookies["SessionId"]);
                Response.Cookies.Delete("SessionId");
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
                ProductData.RemoveFromCart(0);
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}

