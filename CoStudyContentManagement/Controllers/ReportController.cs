using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class ReportController : Controller
    {
        [Route("all")]
        public IActionResult GetAllReport()
        {
            ViewBag.GetAllReport = "nav-active";
            return View();
        }
    }
}
