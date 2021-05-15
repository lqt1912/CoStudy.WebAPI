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
    /// Class FollowConvertAction
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.Follow, CoStudy.API.Infrastructure.Shared.ViewModels.FollowViewModel}" />
    public class FollowConvertAction : IMappingAction<Follow, FollowViewModel>
    {
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The follow repository
        /// </summary>
        IFollowRepository followRepository;

        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="followRepository">The follow repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public FollowConvertAction(IUserRepository userRepository,
            IFollowRepository followRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.followRepository = followRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        /// <exception cref="Exception">Không tìm thấy người theo dõi phù hợp.</exception>
        public void Process(Follow source, FollowViewModel destination, ResolutionContext context)
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
    }
}
