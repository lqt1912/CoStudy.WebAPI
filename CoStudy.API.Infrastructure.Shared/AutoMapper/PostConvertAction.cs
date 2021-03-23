using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class Post Convert Action
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.Post, CoStudy.API.Infrastructure.Shared.ViewModels.PostViewModel}" />
    public class PostConvertAction : IMappingAction<Post, PostViewModel>
    {
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// Up vote repository
        /// </summary>
        IUpVoteRepository upVoteRepository;
        /// <summary>
        /// Down vote repository
        /// </summary>
        IDownVoteRepository downVoteRepository;
        /// <summary>
        /// The comment repository
        /// </summary>
        ICommentRepository commentRepository;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The object level repository
        /// </summary>
        IObjectLevelRepository objectLevelRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="PostConvertAction" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="upVoteRepository">Up vote repository.</param>
        /// <param name="downVoteRepository">Down vote repository.</param>
        /// <param name="commentRepository">The comment repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="objectLevelRepository">The object level repository.</param>
        /// <param name="mapper">The mapper.</param>
        public PostConvertAction(IUserRepository userRepository,
            IUpVoteRepository upVoteRepository,
            IDownVoteRepository downVoteRepository,
            ICommentRepository commentRepository,
            IHttpContextAccessor httpContextAccessor, IObjectLevelRepository objectLevelRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.upVoteRepository = upVoteRepository;
            this.downVoteRepository = downVoteRepository;
            this.commentRepository = commentRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.objectLevelRepository = objectLevelRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(Post source, PostViewModel destination, ResolutionContext context)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            
            var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
            if (author == null)
                throw new Exception("Không tim thấy author phù hợp. ");


            destination.AuthorName = $"{author?.FirstName} {author?.LastName}";
            destination.AuthorAvatar = author?.AvatarHash;

            destination.CommentCount = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active && x.PostId == source.OId).Count();


            var listUpVote = upVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
            destination.Upvote = listUpVote.Count();

            destination.IsVoteByCurrent = (listUpVote.FirstOrDefault(x => x.UpVoteBy == currentUser.OId) != null);


            var listDownVote = downVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
            destination.Downvote = listDownVote.Count();
            destination.IsDownVoteByCurrent = listDownVote.FirstOrDefault(x => x.DownVoteBy == currentUser.OId) != null;

            var objectLevels = objectLevelRepository.GetAll().Where(x => x.ObjectId == source.OId);
            var postObjecLevel = mapper.Map<IEnumerable<ObjectLevelViewModel>>(objectLevels);

            destination.Field = postObjecLevel;

        }
    }
}
