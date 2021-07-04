using System;
using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class NotificationConvertAction : IMappingAction<Noftication, NotificationViewModel>
    {

        IUserRepository userRepository;
        IConfiguration configuration;
        public NotificationConvertAction(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public void Process(Noftication source, NotificationViewModel destination, ResolutionContext context)
        {
            try
            {
                var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
                if (source.Content.Contains("báo cáo"))
                    destination.AuthorAvatar = configuration["AdminAvatar"];
                else destination.AuthorAvatar = author?.AvatarHash;

                destination.AuthorName = $"{author?.FirstName} {author?.LastName}";
                destination.NotificationType = (Application.FCM.PushedNotificationType)source.ObjectType;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
