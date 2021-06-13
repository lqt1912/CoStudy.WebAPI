using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using Org.BouncyCastle.Math.Field;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldGroupController : ControllerBase
    {
        IFieldServices fieldService;

        public FieldGroupController(IFieldServices fieldService)
        {
            this.fieldService = fieldService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFieldGroup(FieldGroup request)
        {
            var data = await fieldService.AddFieldGroup(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route(("add-field"))]
        public async Task<IActionResult> AddFieldToGroup(AddFieldToGroupRequest request)
        {
            var data = await fieldService.AddFieldToGroup(request);
            return Ok((new ApiOkResponse(data)));
        }

        [HttpPost]
        [Route("remove-field")]
        public async Task<IActionResult> RemoveFieldFromGroup(AddFieldToGroupRequest request)
        {
            var data = await fieldService.RemoveFieldFromGroup(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("field-group")]
        public IActionResult GetAllFieldGroup(BaseGetAllRequest request)
        {
            var data = fieldService.GetAllFieldGroup(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}
