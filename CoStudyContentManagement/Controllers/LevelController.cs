using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class LevelController : Controller
    {
        [Route("all")]
        public IActionResult GetAllLevel()
        {
            ViewBag.GetAllLevel = "nav-active";
            return View();
        }
    }
}
