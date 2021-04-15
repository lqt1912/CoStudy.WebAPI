using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    /// <summary>
    /// Class LoggingController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class LoggingController : Controller
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAll()
        {
            ViewBag.GetAll = "nav-active";
            return View();
        }

        /// <summary>
        /// Details this instance.
        /// </summary>
        /// <returns></returns>
        [Route("detail/{id}")]
        public IActionResult Detail(string id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
