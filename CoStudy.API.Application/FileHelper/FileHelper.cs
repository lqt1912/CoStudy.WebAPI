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
    public class FileHelper : IFileHelper
    {
        IConfiguration configuration;
        FirebaseStorage firebaseStorage;

        public FileHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            firebaseStorage = new FirebaseStorage(configuration["FirebaseBlob"]);
        }

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
