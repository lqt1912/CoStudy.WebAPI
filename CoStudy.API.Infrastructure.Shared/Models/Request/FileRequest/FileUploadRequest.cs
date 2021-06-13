using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class FileUploadRequest
    {
              public string Folder { get; set; }
              public IEnumerable<IFormFile> Files { get; set; }
    }
}
