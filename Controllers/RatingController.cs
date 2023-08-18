using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_Team5.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_Team5.Controllers
{
    public class RatingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string id)
        {
            string sessionId = Request.Cookies["SessionId"];
            User user = LoginData.GetUserBySession(sessionId);

            bool status = false;
            status = RatingData.SetRateDb(user.UserId, id);

            return status ? Content("success") : Content("fail");
        }
    }
}

