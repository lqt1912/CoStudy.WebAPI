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
    /// <summary>
    /// Class MessageTextConvertAction.
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.MessageText, CoStudy.API.Infrastructure.Shared.ViewModels.MessageTextViewModel}" />
    public class MessageTextConvertAction : IMappingAction<MessageText, MessageViewModel>
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTextConvertAction"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userRepository">The user repository.</param>
        public MessageTextConvertAction(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(MessageText source, MessageViewModel destination, ResolutionContext context)
        {
            User receiver = Feature.CurrentUser(httpContextAccessor, userRepository);

            User sender = userRepository.GetById(ObjectId.Parse(source.SenderId));

            if (receiver == null || sender == null)
                throw new Exception("Không tìm thấy người dùng gửi tin phù hợp. ");


            destination.SenderName = $"{sender.FirstName} {sender.LastName}";
            destination.SenderAvatar = sender.AvatarHash;

            destination.ReceiverAvatar = receiver.AvatarHash;
            destination.ReceiverName = $"{receiver.FirstName} {receiver.LastName}";
        }
    }
}
