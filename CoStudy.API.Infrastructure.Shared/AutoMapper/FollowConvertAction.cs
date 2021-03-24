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
        /// Initializes a new instance of the <see cref="FollowConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public FollowConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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
