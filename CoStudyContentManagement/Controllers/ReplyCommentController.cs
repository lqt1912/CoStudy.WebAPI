using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class ReplyCommentController : Controller
    {
        [Route("all")]
        public IActionResult GetAll()
        {
            ViewBag.GetAllReply = "nav-active";
            return View();
        }

        [Route("detail")]
        public IActionResult GetById([FromQuery]string replyCommentId)
        {
            ViewBag.replyCommentId = replyCommentId;
            return View();
        }

    }
}
