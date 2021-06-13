using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class ConversationMemberConvertAction : IMappingAction<ConversationMember, ConversationMemberViewModel>
    {
        IUserRepository userRepository;

        public ConversationMemberConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public void Process(ConversationMember source, ConversationMemberViewModel destination, ResolutionContext context)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
