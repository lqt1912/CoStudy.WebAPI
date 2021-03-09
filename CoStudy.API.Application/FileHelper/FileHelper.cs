using CoStudy.API.Application.Features;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Application
{
    /// <summary>
    /// class FileHelper
    /// </summary>
    /// <seealso cref="CoStudy.API.Application.IFileHelper" />
    public class FileHelper : IFileHelper
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// The firebase storage
        /// </summary>
        FirebaseStorage firebaseStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHelper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FileHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            firebaseStorage = new FirebaseStorage(configuration["FirebaseBlob"]);
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> UploadFile(string folder, IEnumerable<IFormFile> files)
        {
            var result = new List<string>();
            foreach (var file in files)
            {
                var stream = file.OpenReadStream();
                var task = firebaseStorage.Child(folder).Child(FileNameGenerator.NameGenerator(folder)).PutAsync(stream);
                var url = await task ;
                result.Add(url);
            }
            return result;
        }

    }
}
