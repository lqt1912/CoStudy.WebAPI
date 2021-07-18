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
    public class FollowConvertAction : IMappingAction<Follow, FollowViewModel>
    {
        IUserRepository userRepository;

        IFollowRepository followRepository;

        IHttpContextAccessor httpContextAccessor;

        public FollowConvertAction(IUserRepository userRepository,
      IFollowRepository followRepository,
      IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.followRepository = followRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Process(Follow source, FollowViewModel destination, ResolutionContext context)
        {
            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

                var fromUser = userRepository.GetById(ObjectId.Parse(source.FromId));
                var toUser = userRepository.GetById(ObjectId.Parse(source.ToId));

                if (fromUser == null || toUser == null)
                {
                    throw new Exception("Không tìm thấy người theo dõi phù hợp. ");
                }

                if (fromUser.OId == currentUser.OId)
                {
                    destination.IsFollowByCurrent = true;
                }
                else
                {
                    destination.IsFollowByCurrent = false;
                }

                destination.FromAvatar = fromUser.AvatarHash;
                destination.FromName = $"{fromUser.FirstName} {fromUser.LastName}";

                destination.ToAvatar = toUser.AvatarHash;
                destination.ToName = $"{toUser.FirstName} {toUser.LastName}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
    }
}
