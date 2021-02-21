using CoStudy.API.Application.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.WebAPI.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        IConfiguration configuration;

        public ImageController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("image")]
        public IActionResult GetImage([FromQuery] string url, int? posiX, int? posiY, int? width, int? height, bool? flipX, bool? flipY, float? rotate)
        {
            System.Tuple<byte[], string> data = new ImageExcute().GetImageExtensionNullable(url, posiX, posiY, width, height, flipX, flipY, rotate, configuration);
            return File(data.Item1, data.Item2);
        }
    }
}
