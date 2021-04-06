using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class ReportConvertAction
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.Report, CoStudy.API.Infrastructure.Shared.ViewModels.ReportViewModel}" />
    public class ReportConvertAction : IMappingAction<Report, ReportViewModel>
    {

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The report reason repository
        /// </summary>
        IReportReasonRepository reportReasonRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="reportReasonRepository">The report reason repository.</param>
        public ReportConvertAction(IUserRepository userRepository, IReportReasonRepository reportReasonRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.reportReasonRepository = reportReasonRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Process(Report source, ReportViewModel destination, ResolutionContext context)
        {
            var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
            if (author == null)
                throw new Exception("Không tìm thấy user. ");

            destination.AuthorName = $"{author.FirstName} {author.LastName}";
            destination.AuthorAvatar = author.AvatarHash;

            var reportReasons = new List<ReportReason>();

            foreach (var reportReasonId in source.Reason)
            {
                var reportReason = reportReasonRepository.GetById(ObjectId.Parse(reportReasonId));
                if (reportReason != null)
                    reportReasons.Add(reportReason);
            }
            destination.ReportReason = new List<ReportReasonViewModel>();
            destination.ReportReason.AddRange(mapper.Map<List<ReportReasonViewModel>>(reportReasons));

            if (source.IsApproved == true)
                destination.ApproveStatusName = "Đã duyệt";
            else destination.ApproveStatusName = "Chưa duyệt";

            if (!string.IsNullOrEmpty(source.ApprovedBy))
            {


                var approver = userRepository.GetById(ObjectId.Parse(source.ApprovedBy));
                if (approver != null)
                    destination.ApprovedByName = $"{approver.FirstName} {approver.LastName}";
            }
        }
    }
}
