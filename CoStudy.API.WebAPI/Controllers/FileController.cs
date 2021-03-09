using CoStudy.API.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.FileRequest;
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
    public class FileController : ControllerBase
    {

        IFileHelper fileHelper;

        public FileController(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        [HttpPost, Route("file-upload")]
        public async Task<IActionResult> FileUpload([FromForm]FileUploadRequest request)
        {
            var data = await fileHelper.UploadFile(request.Folder, request.Files);
            return Ok(new ApiOkResponse(data));
        }
    }
}
