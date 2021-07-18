using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    [Route("binh-luan")]
    public class CommentController : Controller
    {
        [Route("danh-sach-binh-luan")]
        public IActionResult GetAll()
        {
            ViewBag.GetAllComment = "nav-active";
            return View();
        }

        [Route("chi-tiet")]
        public IActionResult GetById([FromQuery] string commentId)
        {
            ViewBag.commentId = commentId;
            return View();
        }
    }
}
