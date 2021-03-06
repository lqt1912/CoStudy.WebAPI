﻿using AutoMapper;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Shared.ViewModels;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Logging, LoggingViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(dest => dest.CreatedDate.Value.ToString("dd/MM/yyyy")));

            CreateMap<Account, AccountResponse>().ReverseMap();
                
            CreateMap<Account, AuthenticateResponse>();

            CreateMap<RegisterRequest, Account>();

            CreateMap<CreateRequest, Account>();

            CreateMap<UpdateRequest, Account>().ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null)
                        {
                            return false;
                        }

                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop))
                        {
                            return false;
                        }

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null)
                        {
                            return false;
                        }

                        return true;
                    }
                ));

            CreateMap<Post, PostViewModel>().AfterMap<PostConvertAction>();

            CreateMap<Comment, CommentViewModel>().AfterMap<CommentConvertAction>();

            CreateMap<ReplyComment, ReplyCommentViewModel>().AfterMap<ReplyCommentConvertAction>();

            CreateMap<User, UserViewModel>().AfterMap<UserConvertAction>();

            CreateMap<Follow, FollowViewModel>().AfterMap<FollowConvertAction>();

            CreateMap<ConversationMember, ConversationMemberViewModel>().AfterMap<ConversationMemberConvertAction>();

            CreateMap<Conversation, ConversationViewModel>();

            CreateMap<ObjectLevel, ObjectLevelViewModel>().AfterMap<ObjectLevelConvertAction>();

            CreateMap<Level, LevelViewModel>();

            CreateMap<Noftication, NotificationViewModel>().AfterMap<NotificationConvertAction>();

            CreateMap<ReportReason, ReportReasonViewModel>().AfterMap<ReportReasonConvertAction>();

            CreateMap<Report, ReportViewModel>().AfterMap<ReportConvertAction>();

            CreateMap<ConversationItemType, ConversationItemTypeViewModel>();

            CreateMap<MessageText, MessageViewModel>().ForMember(dest => dest.Content, opt => opt.MapFrom(x => x.Content)).AfterMap<MessageConvertAction>();


            CreateMap<MessagePostThumbnail, MessageViewModel>().ForMember(dest => dest.Content, opt => opt.Ignore()).AfterMap<MessageConvertAction>().AfterMap<MessagePostThumbnailConvertAction>();

            CreateMap<MessageImage, MessageViewModel>().ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Image)).AfterMap<MessageConvertAction>();

            CreateMap<MessageMultiMedia, MessageViewModel>().ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.MediaUrl)).AfterMap<MessageConvertAction>();

            CreateMap<FieldGroup, FieldGroupViewModel>().ForMember(dest => dest.Fields, opt => opt.Ignore()).AfterMap<FieldGroupConvertAction>();
           
            CreateMap<Field, FieldViewModel>().AfterMap<FieldConvertAction>();
            
            CreateMap<ViolenceWord, ViolenceWordViewModel>();

            CreateMap<SearchHistory, SearchHistoryViewModel>().AfterMap<SearchHistoryConvertAction>();
        }
    }
}
