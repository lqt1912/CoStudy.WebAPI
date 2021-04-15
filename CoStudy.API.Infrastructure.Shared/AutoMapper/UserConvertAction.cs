
using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class User Convert Action
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.User, CoStudy.API.Infrastructure.Shared.ViewModels.UserViewModel}" />
    public class UserConvertAction : IMappingAction<User, UserViewModel>
    {
        /// <summary>
        /// The post repository
        /// </summary>
        IPostRepository postRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The follow repository
        /// </summary>
        IFollowRepository followRepository;

        IObjectLevelRepository objectLevelRepository;

        IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserConvertAction"/> class.
        /// </summary>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="followRepository">The follow repository.</param>
        public UserConvertAction(IPostRepository postRepository, IUserRepository userRepository, IFollowRepository followRepository, IObjectLevelRepository objectLevelRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.followRepository = followRepository;
            this.objectLevelRepository = objectLevelRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(User source, UserViewModel destination, ResolutionContext context)
        {
            destination.PostCount = postRepository.GetAll().Where(x => x.AuthorId == source.OId).Count();
            destination.Followers = followRepository.GetAll().Where(x => x.ToId == source.OId).Count();
            destination.Following = followRepository.GetAll().Where(x => x.FromId == source.OId).Count();
            destination.FullName = $"{source.FirstName} {source.LastName}";


            destination.FullAddress = $"{source.Address?.Detail}, {source.Address?.District}, {source.Address?.City}";
            var objectLevels = mapper.Map<IEnumerable<ObjectLevelViewModel>>(objectLevelRepository.GetAll().Where(x => x.ObjectId == source.OId && x.IsActive == true));
          
            destination.Fields.AddRange(objectLevels);
        }
    }
}
