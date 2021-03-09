using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Application
{
    /// <summary>
    /// File Helper
    /// </summary>
    public interface IFileHelper
    {
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> UploadFile(string folder, IEnumerable<IFormFile> files);
    }
}
