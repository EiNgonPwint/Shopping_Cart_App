using CA_Team5.Data;
using CA_Team5.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_Team5.Controllers
{
    public class LoginController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Login(IFormCollection form)
        {
           
            // data from client
            string username = form["username"];
            string password = HashPassword.Hashing(form["password"]);
            bool usernamecheck = LoginData.CheckUserName(username);
            bool passwordcheck = LoginData.CheckPassword(username,password);
            if (usernamecheck == false || passwordcheck == false )
            { ViewData["errormessage"] = "Invalid Username or Password."; }
            else
            { ViewData["errormessage"] = null; }
          


            User user = LoginData.GetUserByUsername(username);
            if (user != null)
            {                
                if (user.Password == password)
                {
                    string sessionId = LoginData.AddSession(user.UserId);
                    Response.Cookies.Append("SessionId", sessionId);
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        string currentCookie = "";
                        string productId = "";
                        if (Guid.TryParse(cookie, out _) is true)
                        {
                            productId = Request.Cookies[cookie];
                            Product product = ProductData.GetProductById(productId);
                            int status = ProductData.ToCart(user, product);
                            Response.Cookies.Append(cookie,"0");
                        }
                    }
                    ViewData["user"] = user;
                    return RedirectToAction("Index", "Home");
                }

            }
           
          

            return View("Views/Login/index.cshtml");

        }
    }
}

