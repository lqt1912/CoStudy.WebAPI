
using System;
using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class UserConvertAction : IMappingAction<User, UserViewModel>
    {
        IPostRepository postRepository;
        IUserRepository userRepository;
        IFollowRepository followRepository;

        IObjectLevelRepository objectLevelRepository;

        IMapper mapper;

        public UserConvertAction(IPostRepository postRepository, IUserRepository userRepository, IFollowRepository followRepository, IObjectLevelRepository objectLevelRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.followRepository = followRepository;
            this.objectLevelRepository = objectLevelRepository;
            this.mapper = mapper;
        }

        public void Process(User source, UserViewModel destination, ResolutionContext context)
        {
            try
            {
                destination.PostCount = postRepository.GetAll().Count(x => x.AuthorId == source.OId && x.Status == ItemStatus.Active);
                destination.Followers = followRepository.GetAll().Count(x => x.ToId == source.OId);
                destination.Following = followRepository.GetAll().Count(x => x.FromId == source.OId);
                destination.FullName = $"{source.FirstName} {source.LastName}";


                destination.FullAddress = $"{source.Address?.Detail}, {source.Address?.District}, {source.Address?.City}";
                if (string.IsNullOrEmpty(source.AvatarHash))
                {
                    destination.AvatarHash = source.Avatar?.ImageHash;
                }

                var objectLevels = mapper.Map<IEnumerable<ObjectLevelViewModel>>(objectLevelRepository.GetAll().Where(x => x.ObjectId == source.OId && x.IsActive == true));

                destination.Fields.AddRange(objectLevels);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
