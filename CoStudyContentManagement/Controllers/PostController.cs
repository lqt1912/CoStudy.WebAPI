using Microsoft.AspNetCore.Mvc;

namespace CoStudyContentManagement.Controllers
{
    [Route("[controller]")]
    public class PostController : Controller
    {
        [Route("all")]
        public IActionResult GetAllPost()
        {
            ViewBag.GetAllPost = "nav-active";
            return View();
        }

        [Route("detail")]
        public IActionResult PostDetail([FromQuery] string postId)
        {
            ViewBag.postId = postId;
            return View();
        }
    }
}
