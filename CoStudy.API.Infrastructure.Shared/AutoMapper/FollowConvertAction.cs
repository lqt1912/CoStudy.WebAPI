using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class FollowConvertAction : IMappingAction<Follow, FollowViewModel>
    {
        IUserRepository userRepository;

        public FollowConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Process(Follow source, FollowViewModel destination, ResolutionContext context)
        {
            var fromUser = userRepository.GetById(ObjectId.Parse(source.FromId));
            var toUser = userRepository.GetById(ObjectId.Parse(source.ToId));

            if (fromUser == null || toUser == null)
                throw new Exception("Không tìm thấy người theo dõi phù hợp. ");

            destination.FromAvatar = fromUser.AvatarHash;
            destination.FromName = $"{fromUser.FirstName} {fromUser.LastName}";

            destination.ToAvatar = toUser.AvatarHash;
            destination.ToName = $"{toUser.FirstName} {toUser.LastName}";
            
        }
    }
}
