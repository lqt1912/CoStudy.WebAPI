using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.FileRequest
{
    /// <summary>
    /// Class file upload request
    /// </summary>
    public class FileUploadRequest
    {
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public string Folder { get; set; }
        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
