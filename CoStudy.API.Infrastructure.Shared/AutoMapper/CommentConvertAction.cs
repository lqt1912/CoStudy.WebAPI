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
    /// <summary>
    /// Class Comment Convert Action
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{Comment, CommentViewModel}" />
    public class CommentConvertAction : IMappingAction<Comment, CommentViewModel>
    {
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The reply comment repository
        /// </summary>
        IReplyCommentRepository replyCommentRepository;
        /// <summary>
        /// The comment repository
        /// </summary>
        ICommentRepository commentRepository;
        /// <summary>
        /// Up vote repository
        /// </summary>
        IUpVoteRepository upVoteRepository;
        /// <summary>
        /// Down vote repository
        /// </summary>
        IDownVoteRepository downVoteRepository;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="replyCommentRepository">The reply comment repository.</param>
        /// <param name="commentRepository">The comment repository.</param>
        /// <param name="upVoteRepository">Up vote repository.</param>
        /// <param name="downVoteRepository">Down vote repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public CommentConvertAction(IUserRepository userRepository,
            IReplyCommentRepository replyCommentRepository,
            ICommentRepository commentRepository,
            IUpVoteRepository upVoteRepository,
            IDownVoteRepository downVoteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.commentRepository = commentRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Process(Comment source, CommentViewModel destination, ResolutionContext context)
        {
            try
            {

                User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

                User author = userRepository.GetById(ObjectId.Parse(source.AuthorId));

                if (author == null)
                {
                    throw new Exception("Không tìm thấy author. ");
                }

                destination.AuthorName = $"{author.FirstName} {author.LastName}";
                destination.AuthorAvatar = author.AvatarHash;

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
