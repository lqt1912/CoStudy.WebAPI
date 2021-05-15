using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class FieldController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        /// <summary>
        /// The field service
        /// </summary>
        IFieldServices fieldService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldController"/> class.
        /// </summary>
        /// <param name="fieldService">The field service.</param>
        public FieldController(IFieldServices fieldService)
        {
            this.fieldService = fieldService;
        }

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddField(Field request)
        {
            var data = await fieldService.AddField(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets all field.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllField([FromQuery] BaseGetAllRequest request)
        {
            var data = fieldService.GetAll(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the field by group identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        [HttpGet("group-id/{groupId}")]
        public async Task<IActionResult> GetFieldByGroupId(string groupId)
        {
            var data = await fieldService.GetFieldByGroupId(groupId);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Detetes the field.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteteField(string id)
        {
            var data = await fieldService.Delete(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the field to group.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("add-to-group")]
        public async Task<IActionResult> AddFieldToGroup(AddFieldToGroupRequest request)
        {
            var data = await fieldService.AddFieldToGroup(request);
            return Ok(new ApiOkResponse(data));
        }

    }
}
