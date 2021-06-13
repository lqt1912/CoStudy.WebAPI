using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    public class LoggingController : Controller
    {
        public IActionResult GetAll()
        {
            ViewBag.GetAll = "nav-active";
            return View();
        }

        [Route("detail/{id}")]
        public IActionResult Detail(string id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
