using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class MessagePostThumbnailConvertAction
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.MessagePostThumbnail, CoStudy.API.Infrastructure.Shared.ViewModels.MessageViewModel}" />
    public class MessagePostThumbnailConvertAction : IMappingAction<MessagePostThumbnail, MessageViewModel>
    {
        /// <summary>
        /// The post repository
        /// </summary>
        IPostRepository postRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePostThumbnailConvertAction"/> class.
        /// </summary>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="mapper">The mapper.</param>
        public MessagePostThumbnailConvertAction(IPostRepository postRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(MessagePostThumbnail source, MessageViewModel destination, ResolutionContext context)
        {
            try
            {
                var post = postRepository.GetById(ObjectId.Parse(source.PostId));
                var postViewModel = mapper.Map<PostViewModel>(post);
                destination.Content = postViewModel;
            }
            catch (Exception)
            {
                //Do nothing 
            }
        }
    }
}
