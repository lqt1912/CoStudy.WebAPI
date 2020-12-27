using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Web;

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

        public static string SaveImage(IFormFile image, IHttpContextAccessor accessor, string folder)
        {
            if (image == null)
            {
                return "";
            }


            var scheme = accessor.HttpContext.Request.Scheme;
            var host = accessor.HttpContext.Request.Host;
            var pathBase = accessor.HttpContext.Request.PathBase;
            var imageFolder = $"{folder}/";
            var location = $"{scheme}://{host}{pathBase}/{imageFolder}";

            var target = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}\\");

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

        public static User CurrentUser(IHttpContextAccessor _httpContextAccessor, IUserRepository userRepository)
        {
            var currentAccount = (Account)_httpContextAccessor.HttpContext.Items["Account"];

            if (currentAccount != null)
            {

                var cacheduser = CacheHelper.GetValue($"CurrentUser-{currentAccount.Email}") as User;
                if (cacheduser != null)
                    return cacheduser;
                var filter = Builders<User>.Filter.Eq("email", currentAccount.Email);
                return  userRepository.Find(filter);
            }
            else return null;


        }

      
    }
}
