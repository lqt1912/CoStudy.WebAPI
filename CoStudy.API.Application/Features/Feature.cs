using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoStudy.API.Application.Features
{
    public static class Feature
    {
        public static string SaveImageToUrl(IFormFile image, IHttpContextAccessor accessor)
        {
            if (image == null)
            {
                return "";
            }


            var scheme = accessor.HttpContext.Request.Scheme;
            var host = accessor.HttpContext.Request.Host;
            var pathBase = accessor.HttpContext.Request.PathBase;
            var imageFolder = @"UserAvatar/";
            var location = $"{scheme}://{host}{pathBase}/{imageFolder}";

            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserAvatar\");
            Directory.CreateDirectory(target);

            var extension = Path.GetExtension(image.FileName);
            var name = Guid.NewGuid();
            var fileName = name + extension;
            var filePath = Path.Combine(target, fileName);

            image.CopyTo(new FileStream(filePath, FileMode.Create));

            return location + fileName;
        }

        public static string GetHostUrl(IHttpContextAccessor httpContextAccessor)
        {
            var scheme = httpContextAccessor.HttpContext.Request.Scheme;
            var host = httpContextAccessor.HttpContext.Request.Host;
            var pathBase = httpContextAccessor.HttpContext.Request.PathBase;
            var location = $"{scheme}://{host}{pathBase}";
            return location;
        }

    }
}
