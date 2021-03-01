using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Application
{
    public interface IFileHelper
    {
        Task<IEnumerable<string>> UploadFile(string folder, IEnumerable<IFormFile> files);
    }
}
