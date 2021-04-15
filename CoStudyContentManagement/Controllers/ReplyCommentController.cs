using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    /// <summary>
    /// Class ReplyCommentController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class ReplyCommentController : Controller
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        public IActionResult GetAll()
        {
            ViewBag.GetAllReply = "nav-active";
            return View();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="replyCommentId">The reply comment identifier.</param>
        /// <returns></returns>
        public IActionResult GetById(string replyCommentId)
        {
            ViewBag.replyCommentId = replyCommentId;
            return View();
        }

    }
}
