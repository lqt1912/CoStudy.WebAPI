using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        [Route("all")]
        public IActionResult AllUser()
        {
            ViewBag.AllUser = "nav-active";
            return View();
        }

        [Route("detail")]
        public IActionResult GetUserById([FromQuery] string email)
        {
            ViewBag.email = email;
            return View();
        }
    }
}
