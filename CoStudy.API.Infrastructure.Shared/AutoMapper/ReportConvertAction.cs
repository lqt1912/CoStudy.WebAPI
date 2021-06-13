using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class ReportConvertAction : IMappingAction<Report, ReportViewModel>
    {

        IUserRepository userRepository;

        IReportReasonRepository reportReasonRepository;

        IMapper mapper;

        public ReportConvertAction(IUserRepository userRepository, IReportReasonRepository reportReasonRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.reportReasonRepository = reportReasonRepository;
            this.mapper = mapper;
        }

        public void Process(Report source, ReportViewModel destination, ResolutionContext context)
        {
            try
            {
                var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
                if (author == null)
                {
                    throw new Exception("Không tìm thấy user. ");
                }

                destination.AuthorName = $"{author.FirstName} {author.LastName}";
                destination.AuthorAvatar = author.AvatarHash;
                destination.AuthorEmail = author.Email;

                var reportReasons = new List<ReportReason>();
                foreach (var reportReasonId in source.Reason)
                {
                    var reportReason = reportReasonRepository.GetById(ObjectId.Parse(reportReasonId));
                    if (reportReason != null)
                    {
                        reportReasons.Add(reportReason);
                    }
                }
                destination.ReportReason = new List<ReportReasonViewModel>();
                destination.ReportReason.AddRange(mapper.Map<List<ReportReasonViewModel>>(reportReasons));

                if (source.IsApproved == true)
                {
                    destination.ApproveStatusName = "Đã duyệt";
                }
                else
                {
                    destination.ApproveStatusName = "Chưa duyệt";
                }

                if (!string.IsNullOrEmpty(source.ApprovedBy))
                {
                    var approver = userRepository.GetById(ObjectId.Parse(source.ApprovedBy));
                    if (approver != null)
                    {
                        destination.ApprovedByName = $"{approver.FirstName} {approver.LastName}";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
    }
}
