using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class UserAdapter
    {
        public static User FromRequest(AddUserRequest request)
        {
            ObjectId id = ObjectId.GenerateNewId();

            return new User()
            {
                FirstName = request.FisrtName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Avatar = new Image()
                {
                    CreatedDate = DateTime.Now,
                    Discription = "",
                    Id = id,
                    OId = id.ToString(),
                    ImageHash = "https://via.placeholder.com/150",
                    ImageUrl = "https://via.placeholder.com/150",
                    ModifiedDate = DateTime.Now,
                }
            };
        }



        public static Image FromRequest(AddAvatarRequest request, IHttpContextAccessor context)
        {
            // var url = Feature.SaveImageToUrl(request.Image, context);
            return new Image()
            {
                Discription = request.Discription,
                ImageUrl = "",
                ImageHash = request.AvatarHash,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
        }





    }
}
