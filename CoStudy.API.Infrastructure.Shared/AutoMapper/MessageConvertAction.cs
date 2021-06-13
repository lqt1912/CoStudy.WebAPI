using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class MessageConvertAction : IMappingAction<MessageBase, MessageViewModel>
    {
        IHttpContextAccessor httpContextAccessor;
        IUserRepository userRepository;

        public MessageConvertAction(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public void Process(MessageBase source, MessageViewModel destination, ResolutionContext context)
        {
            try
            {
                User receiver = Feature.CurrentUser(httpContextAccessor, userRepository);

                User sender = userRepository.GetById(ObjectId.Parse(source.SenderId));

                if (receiver == null || sender == null)
                {
                    throw new Exception("Không tìm thấy người dùng gửi tin phù hợp. ");
                }

                destination.SenderName = $"{sender.FirstName} {sender.LastName}";
                destination.SenderAvatar = sender.AvatarHash;

                destination.ReceiverAvatar = receiver.AvatarHash;
                destination.ReceiverName = $"{receiver.FirstName} {receiver.LastName}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
