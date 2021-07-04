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
    public class CommentConvertAction : IMappingAction<Comment, CommentViewModel>
    {
        IUserRepository userRepository;
        IReplyCommentRepository replyCommentRepository;
        ICommentRepository commentRepository;
        IUpVoteRepository upVoteRepository;
        IDownVoteRepository downVoteRepository;
        IHttpContextAccessor httpContextAccessor;
        private IPostRepository postRepository;

        public CommentConvertAction(IUserRepository userRepository,
            IReplyCommentRepository replyCommentRepository,
            ICommentRepository commentRepository,
            IUpVoteRepository upVoteRepository,
            IDownVoteRepository downVoteRepository,
            IHttpContextAccessor httpContextAccessor, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.commentRepository = commentRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.postRepository = postRepository;
        }

        public void Process(Comment source, CommentViewModel destination, ResolutionContext context)
        {
            try
            {
                User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

                User author = userRepository.GetById(ObjectId.Parse(source.AuthorId));

                destination.AuthorName = $"{author?.FirstName} {author?.LastName}";
                destination.AuthorAvatar = author?.AvatarHash;
                destination.AuthorEmail = author?.Email;
                destination.RepliesCount = replyCommentRepository.GetAll().Where(x => x.Status == ItemStatus.Active && x.ParentId == source.OId).Count();

                IQueryable<UpVote> listUpVote = upVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
                destination.UpvoteCount = listUpVote.Count();

                destination.IsVoteByCurrent = (listUpVote.FirstOrDefault(x => x.UpVoteBy == currentUser.OId) != null);

                IQueryable<DownVote> listDownVote = downVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
                destination.DownvoteCount = listDownVote.Count();
                destination.IsDownVoteByCurrent = listDownVote.FirstOrDefault(x => x.DownVoteBy == currentUser.OId) != null;
            }
            catch (Exception)
            {
                Console.WriteLine("Lỗi convert action ");
            }
        }
    }
}