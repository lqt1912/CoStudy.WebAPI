using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class CommentController : Controller
    {
        [Route("all")]
        public IActionResult GetAll()
        {
            ViewBag.GetAllComment = "nav-active";
            return View();
        }

        [Route("detail")]
        public IActionResult GetById([FromQuery] string commentId)
        {
            ViewBag.commentId = commentId;
            return View();
        }
    }
}
