using CoStudy.API.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoStudy.API.WebAPI.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public Account Account => (Account)HttpContext.Items["Account"];
    }
}
