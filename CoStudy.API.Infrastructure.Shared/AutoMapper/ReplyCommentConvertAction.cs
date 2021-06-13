using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Linq;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class ReplyCommentConvertAction : IMappingAction<ReplyComment, ReplyCommentViewModel>
    {

        IUserRepository userRepository;
        IUpVoteRepository upVoteRepository;
        IDownVoteRepository downVoteRepository;
        IHttpContextAccessor httpContextAccessor;

        public ReplyCommentConvertAction(IUserRepository userRepository,
     IUpVoteRepository upVoteRepository,
     IDownVoteRepository downVoteRepository,
     IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Process(ReplyComment source, ReplyCommentViewModel destination, ResolutionContext context)
        {
            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                if (currentUser == null)
                    throw new Exception("Người dùng vui lòng đăng nhập. ");

                var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
                if (author == null)
                {
                    throw new Exception("Không tìm thấy author. ");
                }

                destination.AuthorName = $"{author.FirstName} {author.LastName}";
                destination.AuthorAvatar = author.AvatarHash;

                var listUpVote = upVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
                destination.UpvoteCount = listUpVote.Count();

                destination.IsVoteByCurrent = (listUpVote.FirstOrDefault(x => x.UpVoteBy == currentUser.OId) != null);


                var listDownVote = downVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
                destination.DownvoteCount = listDownVote.Count();
                destination.IsDownVoteByCurrent = listDownVote.FirstOrDefault(x => x.DownVoteBy == currentUser.OId) != null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
