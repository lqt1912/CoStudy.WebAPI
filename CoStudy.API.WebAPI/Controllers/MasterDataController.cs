using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Role.Admin)]
    public class MasterDataController : ControllerBase
    {
        IMasterDataServices masterDataServices;

        public MasterDataController(IMasterDataServices masterDataServices)
        {
            this.masterDataServices = masterDataServices;
        }

        [HttpPost("field/all")]
        public IActionResult GetAllFieldFromDatabase(TableRequest request)
        {
            var data = masterDataServices.GetAllField(request);
            return Ok(data);
        }

        [HttpGet("field/{fieldId}")]
        public async Task<IActionResult> GetFieldById(string fieldId)
        {
            var data = await masterDataServices.GetFieldById(fieldId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("level/all")]
        public IActionResult GetAllLevelFromDatabase(TableRequest request)
        {
            var data = masterDataServices.GetAllLevel(request);
            return Ok(data);
        }

        [HttpGet("level/{levelId}")]
        public async Task<IActionResult> GetLevelById(string levelId)
        {
            var data = await masterDataServices.GetLevelById(levelId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("field-group/all")]
        public IActionResult GetAllFieldGroup(TableRequest request)
        {
            var data = masterDataServices.GetAllFieldGroup(request);
            return Ok(data);
        }

        [HttpPut("level/update")]
        public async Task<IActionResult> UpdateLevel([FromBody] Level entity)
        {
            var data = await masterDataServices.UpdateLevel(entity);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("level/add")]
        public async Task<IActionResult> AddLevel([FromBody] Level entity)
        {
            var data = await masterDataServices.AddLevel(entity);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut("field/update")]
        public async Task<IActionResult> UpdateField([FromBody] Field entity)
        {
            var data = await masterDataServices.UpdateField(entity);
            return Ok(new ApiOkResponse(data));
        }
    }
}
