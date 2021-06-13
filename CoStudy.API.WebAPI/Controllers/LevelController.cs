using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        ILevelService levelService;

        public LevelController(ILevelService levelService)
        {
            this.levelService = levelService;
        }

        [HttpPost, Route("level/add")]
        public async Task<IActionResult> AddLevel(IEnumerable<Level> level)
        {
            var data = await levelService.AddLevel(level);
            return Ok(data);
        }
        [HttpGet, Route("level/all")]
        public IActionResult GetAllLevel([FromQuery] BaseGetAllRequest request)
        {
            var data = levelService.GetAllLevel(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet, Route("level/{id}")]
        public async Task<IActionResult> ById(string id)
        {
            var data = await levelService.GetById(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost, Route("objectlevel/add")]
        public async Task<IActionResult> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels)
        {
            var data = await levelService.AddObjectLevel(objectLevels);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet, Route("objectlevel/{objectId}")]
        public async Task<IActionResult> GetLevelByObjectId(string objectId)
        {
            var data = await levelService.GetLevelByObject(objectId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost, Route("user/field/add")]
        public async Task<IActionResult> AddFieldsForUser(UserAddFieldRequest request)
        {
            var data = await levelService.AddOrUpdateUserFields(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet, Route("user/all/{id}")]
        public async Task<IActionResult> GetFieldsOfUser(string id)
        {
            var data = await levelService.GetFieldsOfUser(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost, Route("add-point")]
        public async Task<IActionResult> AddPoint(AddPointRequest request)
        {
            var data = await levelService.AddPoint(request);
            return Ok(new ApiOkResponse(data));
        }


        [HttpPost, Route("reset-field")]
        public async Task<IActionResult> ResetFieldOfUser(UserResetFieldRequest request)
        {
            var data = await levelService.ResetField(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("leader-board")]
        public async Task<IActionResult> GetLeaderBoard([FromQuery] LeaderBoardRequest request)
        {
            var data = await levelService.GetLeaderBoard(request);
            return Ok(new ApiOkResponse(data));
        }


    }
}
