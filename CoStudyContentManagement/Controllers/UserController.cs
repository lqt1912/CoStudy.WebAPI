using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyContentManagement.Controllers
{
    /// <summary>
    /// Class UserController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class UserController : Controller
    {
        /// <summary>
        /// Alls the user.
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        public IActionResult AllUser()
        {
            ViewBag.AllUser = "nav-active";
            return View();
        }

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <returns></returns>
        [Route("detail")]
        public IActionResult GetUserById([FromQuery] string email)
        {
            ViewBag.email = email;
            return View();
        }
    }
}
