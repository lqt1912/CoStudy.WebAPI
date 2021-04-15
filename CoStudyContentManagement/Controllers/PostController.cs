using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    /// <summary>
    /// Class PostController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class PostController : Controller
    {
        /// <summary>
        /// Gets all post.
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        public IActionResult GetAllPost()
        {
            ViewBag.GetAllPost = "nav-active";
            return View();
        }

        /// <summary>
        /// Posts the detail.
        /// </summary>
        /// <returns></returns>
        [Route("detail")]
        public IActionResult PostDetail()
        {
            return View();
        }
    }
}
