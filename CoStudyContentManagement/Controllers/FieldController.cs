using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class FieldController : Controller
    {
        [Route("all")]
        public IActionResult GetAllField()
        {
            ViewBag.GetAllField = "nav-active";
            return View();
        }
    }
}
