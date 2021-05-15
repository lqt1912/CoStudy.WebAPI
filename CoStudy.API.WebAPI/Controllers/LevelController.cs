using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// The LevelController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        /// <summary>
        /// The level service
        /// </summary>
        ILevelService levelService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelController"/> class.
        /// </summary>
        /// <param name="levelService">The level service.</param>
        public LevelController(ILevelService levelService)
        {
            this.levelService = levelService;
        }

        /// <summary>
        /// Adds the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        [HttpPost, Route("level/add")]
        public async Task<IActionResult> AddLevel(IEnumerable<Level> level)
        {
            var data = await levelService.AddLevel(level);
            return Ok(data);
        }
        /// <summary>
        /// Gets all level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet, Route("level/all")]
        public IActionResult GetAllLevel([FromQuery] BaseGetAllRequest request)
        {
            var data = levelService.GetAllLevel(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Bies the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet, Route("level/{id}")]
        public async Task<IActionResult> ById(string id)
        {
            var data = await levelService.GetById(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the object level.
        /// </summary>
        /// <param name="objectLevels">The object levels.</param>
        /// <returns></returns>
        [HttpPost, Route("objectlevel/add")]
        public async Task<IActionResult> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels)
        {
            var data = await levelService.AddObjectLevel(objectLevels);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the level by object identifier.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns></returns>
        [HttpGet, Route("objectlevel/{objectId}")]
        public async Task<IActionResult> GetLevelByObjectId(string objectId)
        {
            var data = await levelService.GetLevelByObject(objectId);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the fields for user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost, Route("user/field/add")]
        public async Task<IActionResult> AddFieldsForUser(UserAddFieldRequest request)
        {
            var data = await levelService.AddOrUpdateUserFields(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the fields of user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet, Route("user/all/{id}")]
        public async Task<IActionResult> GetFieldsOfUser(string id)
        {
            var data = await levelService.GetFieldsOfUser(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost, Route("add-point")]
        public async Task<IActionResult> AddPoint(AddPointRequest request)
        {
            var data = await levelService.AddPoint(request);
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Resets the field of user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost, Route("reset-field")]
        public async Task<IActionResult> ResetFieldOfUser(UserResetFieldRequest request)
        {
            var data = await levelService.ResetField(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the leader board.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet("leader-board")]
        public async Task<IActionResult> GetLeaderBoard([FromQuery] BaseGetAllRequest request)
        {
            var data = await levelService.GetLeaderBoard(request);
            return Ok(new ApiOkResponse(data));
        }


    }
}
