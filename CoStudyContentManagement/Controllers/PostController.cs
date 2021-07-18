using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    /// <summary>
    /// Class PostController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("bai-viet")]
    public class PostController : Controller
    {
        /// <summary>
        /// Gets all post.
        /// </summary>
        /// <returns></returns>
        [Route("danh-sach-bai-viet")]
        public IActionResult GetAllPost()
        {
            ViewBag.GetAllPost = "nav-active";
            return View();
        }

        /// <summary>
        /// Posts the detail.
        /// </summary>
        /// <returns></returns>
        [Route("chi-tiet")]
        public IActionResult PostDetail([FromQuery] string postId)
        {
            ViewBag.postId = postId;
            return View();
        }
    }
}
