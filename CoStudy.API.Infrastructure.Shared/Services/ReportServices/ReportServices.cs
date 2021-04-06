using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.ReportRequest;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Report Service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IReportServices" />
    public class ReportServices : IReportServices
    {
        /// <summary>
        /// The report repository
        /// </summary>
        IReportRepository reportRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The post repository
        /// </summary>
        IPostRepository postRepository;

        /// <summary>
        /// The comment repository
        /// </summary>
        ICommentRepository commentRepository;

        /// <summary>
        /// The reply comment repository
        /// </summary>
        IReplyCommentRepository replyCommentRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServices"/> class.
        /// </summary>
        /// <param name="reportRepository">The report repository.</param>
        public ReportServices(IReportRepository reportRepository, 
            IMapper mapper, IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor,
            IPostRepository postRepository, 
            ICommentRepository commentRepository, 
            IReplyCommentRepository replyCommentRepository)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
        }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<ReportViewModel> Add(Report entity)
        {
            var data = new Report()
            {
                CreatedDate = DateTime.Now,
                AuthorId = entity.AuthorId,
                IsApproved = false,
                ModifiedDate = DateTime.Now,
                Reason = entity.Reason,
                ObjectId = entity.ObjectId,
            };
            await reportRepository.AddAsync(data);
            return mapper.Map<ReportViewModel>(data);
        }

        /// <summary>
        /// Posts the report.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<ReportViewModel> PostReport(CreatePostReportRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var post = new Post();

            var data = new Report()
            {
                AuthorId = currentUser.OId,
                Reason = request.Reason.ToList(),
                ObjectId = request.PostId,
                ObjectType = Feature.GetTypeName(post)
            };

            await reportRepository.AddAsync(data);

            return mapper.Map<ReportViewModel>(data);

        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<ReportViewModel> GetAll(BaseGetAllRequest request)
        {
            var data = reportRepository.GetAll();
            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return mapper.Map<IEnumerable<ReportViewModel>>(data);
        }

        /// <summary>
        /// Approves the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ReportViewModel>> Approve(IEnumerable<string> ids)
        {
            var dataToApprove = new List<Report>();
            foreach (var item in ids)
            {
                var rp = await reportRepository.GetByIdAsync(ObjectId.Parse(item));
                if (rp != null)
                    dataToApprove.Add(rp);
            }

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            foreach (var item in dataToApprove)
            {
                item.IsApproved = true;
                item.ModifiedDate = DateTime.Now;
                item.ApprovedBy = currentUser.OId;
                item.ApproveDate = DateTime.Now;
                await reportRepository.UpdateAsync(item, item.Id);


                if(item.ObjectType.Contains("Post"))
                {

                }


            }
            return mapper.Map<IEnumerable<ReportViewModel>>( dataToApprove);
        }

        /// <summary>
        /// Comments the report.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ReportViewModel> CommentReport(CreateCommentReportRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comment = new Comment();

            var data = new Report()
            {
                AuthorId = currentUser.OId,
                Reason = request.Reason.ToList(),
                ObjectId = request.CommentId,
                ObjectType = Feature.GetTypeName(comment)
            };

            await reportRepository.AddAsync(data);

            return mapper.Map<ReportViewModel>(data);
        }

        public async Task<ReportViewModel> ReplyReport(CreateReplyReportRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comment = new ReplyComment();

            var data = new Report()
            {
                AuthorId = currentUser.OId,
                Reason = request.Reason.ToList(),
                ObjectId = request.ReplyId,
                ObjectType = Feature.GetTypeName(comment)
            };

            await reportRepository.AddAsync(data);

            return mapper.Map<ReportViewModel>(data);
        }
    }
}
