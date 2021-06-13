using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class FieldGroupController : Controller
    {
        [Route("all")]
        public IActionResult GetAll()
        {
            ViewBag.GetAllFieldGroup = "nav-active";
            return View();
        }
    }
}
