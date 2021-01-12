using CoStudy.API.Infrastructure.Shared.Services.LocationServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet]
        [Route("province")]
        public IActionResult GetAllProvinces([FromQuery] string name)
        {
            var data = locationService.GetAllProvinces(name);
            return Ok(new ApiOkResponse(data));

        }

        [HttpGet]
        [Route("province/{code}")]
        public async Task<IActionResult> GetProvinceByCode(string code)
        {
            var data =await locationService.GetProvinceByCode(code);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("district/province/{province}")]
        public async Task<IActionResult> GetDistrictByProvince(string province)
        {
            var data = await locationService.GetDistrictByProvince(province);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("district/{code}")]
        public async Task<IActionResult>  GetDistrictByCode(string code)
        {
            var data = await locationService.GetDistrictByCode(code);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("ward/district/{district}")]
        public async Task<IActionResult> GetWardByDistrict(string district)
        {
            var data = await locationService.GetWardByDistrict(district);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("ward/{code}")]
        public async Task<IActionResult> GetWardByCode(string code)
        {
            var data = await locationService.GetWardByCode(code);
            return Ok(new ApiOkResponse(data));
        }

    }
}
