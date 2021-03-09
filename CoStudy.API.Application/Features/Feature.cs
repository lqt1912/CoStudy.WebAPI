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
    /// <summary>
    /// Some External feature
    /// </summary>
    public static class Feature
    {

        /// <summary>
        /// Saves the image to URL.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="accessor">The accessor.</param>
        /// <returns></returns>
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

            string target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserAvatar\");
            Directory.CreateDirectory(target);

            string extension = Path.GetExtension(image.FileName);
            Guid name = Guid.NewGuid();
            string fileName = name + extension;
            string filePath = Path.Combine(target, fileName);

            image.CopyTo(new FileStream(filePath, FileMode.Create));

            return location + fileName;
        }

        /// <summary>
        /// Saves the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="accessor">The accessor.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
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



        /// <summary>
        /// Gets the host URL.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <returns></returns>
        public static string GetHostUrl(IHttpContextAccessor httpContextAccessor)
        {
            string scheme = httpContextAccessor.HttpContext.Request.Scheme;
            HostString host = httpContextAccessor.HttpContext.Request.Host;
            PathString pathBase = httpContextAccessor.HttpContext.Request.PathBase;
            string location = $"{scheme}://{host}{pathBase}";
            return location;
        }

        /// <summary>
        /// Currents the user.
        /// </summary>
        /// <param name="_httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <returns></returns>
        public static User CurrentUser(IHttpContextAccessor _httpContextAccessor, IUserRepository userRepository)
        {
            Account currentAccount = (Account)_httpContextAccessor.HttpContext.Items["Account"];

            if (currentAccount != null)
            {
                var filter = Builders<User>.Filter.Eq("email", currentAccount.Email);
                return userRepository.Find(filter);
            }
            else return null;


        }


        /// <summary>
        /// Determines whether the specified list a is equal.
        /// </summary>
        /// <param name="listA">The list a.</param>
        /// <param name="listB">The list b.</param>
        /// <returns>
        ///   <c>true</c> if the specified list a is equal; otherwise, <c>false</c>.
        /// </returns>
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
