using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using static Common.Constants.NotificationContentTypeConstant;

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
                var filter = Builders<User>.Filter.Eq("email", currentAccount.Email);
                return userRepository.Find(filter);
            }
            else throw new UnauthorizedAccessException("Người dùng chưa đăng nhập. ");

        }

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

        public static string GetTypeName(object obj)
        {
            Type type = obj.GetType();
            return type.FullName;
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

        public static JwtSecurityToken ValidateToken(string token, string secret)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }


}
