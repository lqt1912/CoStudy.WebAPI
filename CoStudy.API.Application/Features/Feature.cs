using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;

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


            string scheme = accessor.HttpContext.Request.Scheme;
            HostString host = accessor.HttpContext.Request.Host;
            PathString pathBase = accessor.HttpContext.Request.PathBase;
            string imageFolder = @"UserAvatar/";
            string location = $"{scheme}://{host}{pathBase}/{imageFolder}";

            string target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserAvatar\");
            Directory.CreateDirectory(target);

            string extension = Path.GetExtension(image.FileName);
            Guid name = Guid.NewGuid();
            string fileName = name + extension;
            string filePath = Path.Combine(target, fileName);

            image.CopyTo(new FileStream(filePath, FileMode.Create));

            return location + fileName;
        }

        public static string SaveImage(IFormFile image, IHttpContextAccessor accessor, string folder)
        {
            if (image == null)
            {
                return "";
            }


            string scheme = accessor.HttpContext.Request.Scheme;
            HostString host = accessor.HttpContext.Request.Host;
            PathString pathBase = accessor.HttpContext.Request.PathBase;
            string imageFolder = $"{folder}/";
            string location = $"{scheme}://{host}{pathBase}/{imageFolder}";

            string target = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}\\");

            Directory.CreateDirectory(target);

            string extension = Path.GetExtension(image.FileName);
            Guid name = Guid.NewGuid();
            string fileName = name + extension;
            string filePath = Path.Combine(target, fileName);

            image.CopyTo(new FileStream(filePath, FileMode.Create));

            return location + fileName;
        }



        public static string GetHostUrl(IHttpContextAccessor httpContextAccessor)
        {
            string scheme = httpContextAccessor.HttpContext.Request.Scheme;
            HostString host = httpContextAccessor.HttpContext.Request.Host;
            PathString pathBase = httpContextAccessor.HttpContext.Request.PathBase;
            string location = $"{scheme}://{host}{pathBase}";
            return location;
        }

        public static User CurrentUser(IHttpContextAccessor _httpContextAccessor, IUserRepository userRepository)
        {
            Account currentAccount = (Account)_httpContextAccessor.HttpContext.Items["Account"];

            if (currentAccount != null)
            {

                //var cacheduser = CacheHelper.GetValue($"CurrentUser-{currentAccount.Email}") as User;
                //if (cacheduser != null)
                //    return cacheduser;
                FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", currentAccount.Email);
                return userRepository.Find(filter);
            }
            else return null;


        }

        public static bool IsEqual(List<string> listA, List<string> listB)
        {
            if (listA.Count == listB.Count)
            {
                foreach (string item in listB)
                {
                    if (!listA.Contains(item))
                        return false;
                }
                return true;
            }
            else return false;

        }

    }
}
