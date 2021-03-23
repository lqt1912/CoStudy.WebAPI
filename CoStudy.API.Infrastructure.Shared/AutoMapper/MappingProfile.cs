using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class MappingProfile
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile" /> class.
        /// </summary>
        public MappingProfile()
        {

            CreateMap<Logging, LoggingViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(dest => dest.CreatedDate.Value.ToString("dd/MM/yyyy")));

            CreateMap<Account, AccountResponse>().ReverseMap();

            CreateMap<Account, AuthenticateResponse>();

            CreateMap<RegisterRequest, Account>();

            CreateMap<CreateRequest, Account>();

            CreateMap<UpdateRequest, Account>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                        return true;
                    }
                ));
            CreateMap<Post, PostViewModel>().AfterMap<PostConvertAction>();
            CreateMap<Comment, CommentViewModel>().AfterMap<CommentConvertAction>();
            CreateMap<ReplyComment, ReplyCommentViewModel>().AfterMap<ReplyCommentConvertAction>();
            CreateMap<User, UserViewModel>().AfterMap<UserConvertAction>();
            CreateMap<Follow, FollowViewModel>().AfterMap<FollowConvertAction>();


            CreateMap<Message, MessageViewModel>().AfterMap<MessageConvertAction>();
            CreateMap<ConversationMember, ConversationMemberViewModel>().AfterMap<ConversationMemberConvertAction>();
            CreateMap<Conversation, ConversationViewModel>();
            CreateMap<ObjectLevel, ObjectLevelViewModel>().AfterMap<ObjectLevelConvertAction>();

            CreateMap<Level, LevelViewModel>();
        }

    }

   

}
