using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        IFieldServices fieldService;

        public FieldController(IFieldServices fieldService)
        {
            this.fieldService = fieldService;
        }

        [HttpPost]
        public async Task<IActionResult> AddField(Field request)
        {
            var data = await fieldService.AddField(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        public IActionResult GetAllField([FromQuery] BaseGetAllRequest request)
        {
            var data = fieldService.GetAll(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("group-id/{groupId}")]
        public async Task<IActionResult> GetFieldByGroupId(string groupId)
        {
            var data = await fieldService.GetFieldByGroupId(groupId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteteField(string id)
        {
            var data = await fieldService.Delete(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("add-to-group")]
        public async Task<IActionResult> AddFieldToGroup(AddFieldToGroupRequest request)
        {
            var data = await fieldService.AddFieldToGroup(request);
            return Ok(new ApiOkResponse(data));
        }

    }
}
