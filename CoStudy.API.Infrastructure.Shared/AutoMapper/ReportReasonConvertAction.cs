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
    /// Class ReportReasonComnvertAction
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.ReportReason, CoStudy.API.Infrastructure.Shared.ViewModels.ReportReasonViewModel}" />
    public class ReportReasonConvertAction : IMappingAction<ReportReason, ReportReasonViewModel>
    {

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportReasonConvertAction"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public ReportReasonConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(ReportReason source, ReportReasonViewModel destination, ResolutionContext context)
        {
            var user = userRepository.GetById(ObjectId.Parse(source.CreatedBy));

            destination.CreatedByName = $"{user?.FirstName} {user?.LastName}";
        }
    }
}
