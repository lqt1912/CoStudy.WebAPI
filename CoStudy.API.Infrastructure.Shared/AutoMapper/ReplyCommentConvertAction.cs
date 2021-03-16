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
    /// Class Reply Comment Convert Action
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.ReplyComment, CoStudy.API.Infrastructure.Shared.ViewModels.ReplyCommentViewModel}" />
    public class ReplyCommentConvertAction : IMappingAction<ReplyComment, ReplyCommentViewModel>
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
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyCommentConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="upVoteRepository">Up vote repository.</param>
        /// <param name="downVoteRepository">Down vote repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
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

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(ReplyComment source, ReplyCommentViewModel destination, ResolutionContext context)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
            destination.AuthorName = $"{author.FirstName} {author.LastName}";
            destination.AuthorAvatar = author.AvatarHash;



            var listUpVote = upVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
            destination.UpvoteCount = listUpVote.Count();

            destination.IsVoteByCurrent = (listUpVote.FirstOrDefault(x => x.UpVoteBy == currentUser.OId) != null);


            var listDownVote = downVoteRepository.GetAll().Where(x => x.ObjectVoteId == source.OId && x.IsDeleted == false);
            destination.DownvoteCount = listDownVote.Count();
            destination.IsDownVoteByCurrent = listDownVote.FirstOrDefault(x => x.DownVoteBy == currentUser.OId) != null;

        }
    }
}
