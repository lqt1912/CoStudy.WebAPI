using System;
using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class ReportReasonConvertAction : IMappingAction<ReportReason, ReportReasonViewModel>
    {

        IUserRepository userRepository;

        public ReportReasonConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Process(ReportReason source, ReportReasonViewModel destination, ResolutionContext context)
        {
            try
            {
                var user = userRepository.GetById(ObjectId.Parse(source.CreatedBy));

                destination.CreatedByName = $"{user?.FirstName} {user?.LastName}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
          
        }
    }
}
