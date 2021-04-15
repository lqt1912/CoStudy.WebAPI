using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    /// <summary>
    /// class CommentController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class CommentController : Controller
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        public IActionResult GetAll()
        {
            ViewBag.GetAllComment = "nav-active";
            return View();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        [Route("detail")]
        public IActionResult GetById([FromQuery] string commentId)
        {
            ViewBag.commentId = commentId;
            return View();
        }
    }
}
