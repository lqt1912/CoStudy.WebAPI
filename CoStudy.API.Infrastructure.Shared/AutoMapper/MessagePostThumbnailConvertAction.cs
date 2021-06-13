using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class MessagePostThumbnailConvertAction : IMappingAction<MessagePostThumbnail, MessageViewModel>
    {
        IPostRepository postRepository;

        IMapper mapper;

        public MessagePostThumbnailConvertAction(IPostRepository postRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

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
