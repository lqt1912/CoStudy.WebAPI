using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class ConversationMemberConvertAction
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.ConversationMember, CoStudy.API.Infrastructure.Shared.ViewModels.ConversationMemberViewModel}" />
    public class ConversationMemberConvertAction : IMappingAction<ConversationMember, ConversationMemberViewModel>
    {
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationMemberConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public ConversationMemberConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(ConversationMember source, ConversationMemberViewModel destination, ResolutionContext context)
        {

            var user = userRepository.GetById(ObjectId.Parse(source.MemberId));
            var joinBy = userRepository.GetById(ObjectId.Parse(source.JoinBy));

            if (user == null || joinBy == null)
            {
                throw new Exception("Không tìm thấy người dùng hợp lệ. ");
            }

            destination.MemberName = $"{user?.FirstName} {user?.LastName}";
            destination.MemberAvatar = user?.AvatarHash;
            destination.JoinByName = $"{joinBy?.FirstName} {joinBy?.LastName}";

        }
    }
}
