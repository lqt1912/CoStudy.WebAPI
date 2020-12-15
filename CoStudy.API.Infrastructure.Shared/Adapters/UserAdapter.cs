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

        public static Field FromRequest(AddFieldRequest request, IHttpContextAccessor context)
        {

            var url = Feature.SaveImageToUrl(request.Image, context);
            var image = new Image()
            {
                ImageUrl = url,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Discription = request.UserField
            
            };
            return new Field()
            {
                Value= request.UserField,
                ThumbnailImage=image,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static AddFieldResponse ToResponse(Field field, string id)
        {
            return new AddFieldResponse()
            {
                UserId = id,
                Value= field.Value,
                Image=field.ThumbnailImage.ImageUrl
            };
        }

        public static GetUserByIdResponse ToResponse1(User user1)
        {
            var result = new GetUserByIdResponse()
            {
                UserId = user1.Id.ToString(),
                FirstName = user1.FirstName,
                LastName = user1.LastName,
                DateOfBirth = user1.DateOfBirth,
                Email = user1.Email,
                PhoneNumber = user1.PhoneNumber,
                Address = user1.Address,
                Avatar = user1.Avatar,
                Status = user1.Status,
                CreatedDate = user1.CreatedDate,
                ModifiedDate = user1.ModifiedDate,
                Posts = user1.Posts,
                Noftications = user1.Noftications,
                Followers = user1.Followers,
                Following = user1.Following,
                AdditionalInfos = user1.AdditionalInfos,
                Fortes = user1.Fortes
            };
            return result;
        }
    }
}
