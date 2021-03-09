using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Features
{
    /// <summary>
    /// FileNameGenerator
    /// </summary>
    public static class FileNameGenerator
    {
        /// <summary>
        /// Names the generator.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public static string NameGenerator(string folderName)
        {
            var fileName = $"{folderName}_{Guid.NewGuid()}";
            return fileName;
        }
    }
}
