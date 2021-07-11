using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class ViolenceWordController : Controller
    {
        [Route("all")]
        public IActionResult GetAll()
        {
            ViewBag.AllViolenceWord = "nav-active";
            return View();
        }
    }
}
