using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyClientManagement.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("account")]
        public IActionResult Account()
        {
            return View();
        }

        [Route("user-profile")]
        public IActionResult UserProfile()
        {
            return View();
        }
    }
}
