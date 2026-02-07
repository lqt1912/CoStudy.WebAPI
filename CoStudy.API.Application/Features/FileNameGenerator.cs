using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Features
{
    public static class FileNameGenerator
    {
        public static string NameGenerator(string folderName)
        {
            var fileName = $"{folderName}_{Guid.NewGuid()}";
            return fileName;
        }
    }
}
