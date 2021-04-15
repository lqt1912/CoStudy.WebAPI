using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudyClientManagement.Controllers
{
    /// <summary>
    /// Class UserController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class UserController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Accounts this instance.
        /// </summary>
        /// <returns></returns>
        [Route("account")]
        public IActionResult Account()
        {
            return View();
        }

        /// <summary>
        /// Users the profile.
        /// </summary>
        /// <returns></returns>
        [Route("user-profile")]
        public IActionResult UserProfile()
        {
            return View();
        }
    }
}
