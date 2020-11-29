using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class UserAdapter
    {
        public static User FromRequest(AddUserRequest request)
        {
            return new User()
            {
                FirstName = request.FisrtName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static AddUserResponse ToResponse(User user)
        {
            return new AddUserResponse()
            {
                Id =user.Id,
                FisrtName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }

        public static Image FromRequest(AddAvatarRequest request, IHttpContextAccessor context)
        {
            var url = Feature.SaveImageToUrl(request.Image, context);
            return new Image()
            {
                Discription = request.Discription,
                ImageUrl = url,
                Base64String = "updating",
                OriginalWidth = 1,
                OriginalHeight = 1,
                CompressRatio = 1.0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
        }

        public static AddAvatarResponse ToResponse(Image image, string userId)
        {
            return new AddAvatarResponse()
            {
                UserId = userId,
                Id = image.Id,
                Discription = image.Discription,
                ImageUrl = image.ImageUrl,
                Base64String = image.Base64String,
                OriginalWidth = image.OriginalWidth,
                OriginalHeight = image.OriginalHeight,
                CompressRatio = image.CompressRatio,
                CreatedDate = image.CreatedDate,
                ModifiedDate = image.ModifiedDate
            };
        }

        public static List<AdditionalInfo> FromRequest(AddAdditionalInfoRequest request)
        {
            var result = new List<AdditionalInfo>();
            foreach (var item in request.AdditionalInfos)
            {
                result.Add(
                    new AdditionalInfo() { 
                    InfoValue = item.InfoValue,
                    InfoType = item.InfoType,
                    CreatedDate =DateTime.Now,
                    ModifiedDate = DateTime.Now
                    });
            }
            return request.AdditionalInfos;
        }

        public static AddAdditionalInfoResponse ToResponse(List<AdditionalInfo> info, string id)
        {
            return new AddAdditionalInfoResponse()
            { 
                UserId =id,
                AdditionalInfos =info
            };
        }

    }
}
