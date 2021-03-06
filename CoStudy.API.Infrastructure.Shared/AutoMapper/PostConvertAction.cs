﻿using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class PostConvertAction : IMappingAction<Post, PostViewModel>
    {
        IUserRepository userRepository;
        IUpVoteRepository upVoteRepository;
        IDownVoteRepository downVoteRepository;
        ICommentRepository commentRepository;
        IHttpContextAccessor httpContextAccessor;
        IObjectLevelRepository objectLevelRepository;
        IMapper mapper;
        IClientGroupRepository clientGroupRepository;
        public PostConvertAction(IUserRepository userRepository,
                              IUpVoteRepository upVoteRepository,
                              IDownVoteRepository downVoteRepository,
                              ICommentRepository commentRepository,
                              IHttpContextAccessor httpContextAccessor,
                              IObjectLevelRepository objectLevelRepository,
                              IMapper mapper, 
                              IClientGroupRepository clientGroupRepository)
        {
            this.userRepository = userRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
            this.commentRepository = commentRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.objectLevelRepository = objectLevelRepository;
            this.mapper = mapper;
            this.clientGroupRepository = clientGroupRepository;
        }

        public void Process(Post source, PostViewModel destination, ResolutionContext context)
        {
            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

                if (currentUser == null)
                {
                    throw new UnauthorizedAccessException("Vui lòng đăng nhập. ");
                }

                var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));

                destination.AuthorName = $"{author?.FirstName} {author?.LastName}";
                destination.AuthorAvatar = author?.AvatarHash;
                destination.AuthorEmail = author?.Email;
                destination.CommentCount = commentRepository.GetAll()
                    .Where(x => x.Status == ItemStatus.Active && x.PostId == source.OId).Count();

                var listUpVote = upVoteRepository.GetAll()
                    .Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
                destination.Upvote = listUpVote.Count();

                destination.IsVoteByCurrent = (listUpVote.FirstOrDefault(x => x.UpVoteBy == currentUser.OId) != null);


                var listDownVote = downVoteRepository.GetAll()
                    .Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
                destination.Downvote = listDownVote.Count();
                destination.IsDownVoteByCurrent =
                    listDownVote.FirstOrDefault(x => x.DownVoteBy == currentUser.OId) != null;

                var objectLevels = objectLevelRepository.GetAll().Where(x => x.ObjectId == source.OId);
                var postObjecLevel = mapper.Map<IEnumerable<ObjectLevelViewModel>>(objectLevels);

                destination.IsSaveByCurrent = currentUser.PostSaved.Any(x => x == source.OId);

                destination.Field = postObjecLevel;

                switch (source.PostType)
                {
                    case Common.Constants.PostType.Question:
                        destination.PostTypeName = "Câu hỏi";
                        break;
                    case Common.Constants.PostType.Sharing:
                        destination.PostTypeName = "Chia sẻ";
                        break;
                    default:
                        destination.PostTypeName = "Câu hỏi";
                        break;
                }

                for (int i = 0; i < source.StringContents.Count(); i++)
                {
                    if (source.StringContents[i].Content == null)
                        destination.StringContents[i].Content = String.Empty;
                }

                var clientgroups = clientGroupRepository.GetAll().FirstOrDefault(x => x.Name == source.OId);
                if(clientgroups!=null)
                {
                    if (clientgroups.UserIds.Any(x => x == currentUser.OId))
                    {
                        if (!currentUser.TurnOfNotification.Any(a => a == source.OId))
                            destination.IsNotifyByCurrent = true;
                        else destination.IsNotifyByCurrent = false;
                    }
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }
    }
}
