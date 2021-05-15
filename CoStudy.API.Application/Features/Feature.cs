using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static Common.Constants.NotificationContentTypeConstant;

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
        public static bool IsEqual(List<ConversationMember> listA, List<ConversationMember> listB)
        {
            if (listA.Count == listB.Count)
            {
                foreach (var _b in listB)
                {
                    if (listA.AsQueryable().FirstOrDefault(x => x.MemberId == _b.MemberId) == null)
                        return false;
                }
                return true;
            }
            else return false;

        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string GetTypeName(object obj)
        {
            Type type = obj.GetType();
            return type.FullName;
        }

        /// <summary>
        /// Builds the content of the notify.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="creatorName">Name of the creator.</param>
        /// <param name="authorName">Name of the author.</param>
        /// <returns></returns>
        public static string BuildNotifyContent(ContentType actionName, string creatorName, string authorName)
        {

            switch (actionName)
            {
                case ContentType.ADD_POST_NOTIFY:
                    return $"{creatorName}{ADD_POST_NOTIFY}{authorName}";

                case ContentType.DOWNVOTE_COMMENT_NOTIFY:
                    return $"{creatorName}{DOWNVOTE_COMMENT_NOTIFY}{authorName}";

                case ContentType.DOWNVOTE_POST_NOTIFY:
                    return $"{creatorName}{DOWNVOTE_POST_NOTIFY}{authorName}";

                case ContentType.DOWNVOTE_REPLY_NOTIFY:
                    return $"{creatorName}{DOWNVOTE_REPLY_NOTIFY}{authorName}";

                case ContentType.UPVOTE_COMMENT_NOTIFY:
                    return $"{creatorName}{UPVOTE_COMMENT_NOTIFY}{authorName}";

                case ContentType.UPVOTE_POST_NOTIFY:
                    return $"{creatorName}{UPVOTE_POST_NOTIFY}{authorName}";

                case ContentType.UPVOTE_REPLY_NOTIFY:
                    return $"{creatorName}{UPVOTE_REPLY_NOTIFY}{authorName}";

                case ContentType.COMMENT_NOTIFY:
                    return $"{creatorName}{COMMENT_NOTIFY}{authorName}";

                case ContentType.REPLY_COMMENT_NOTIFY:
                    return $"{creatorName}{REPLY_COMMENT_NOTIFY}{authorName}";
                default:
                    return "Có thông báo mới. ";
            }
        }

        public static double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public static double ParseDouble(string strNumber)
        {
            try
            {
                return double.Parse(strNumber, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
