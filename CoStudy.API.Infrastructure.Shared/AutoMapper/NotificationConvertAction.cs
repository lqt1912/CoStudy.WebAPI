using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class NotificationConvertAction : IMappingAction<Noftication, NotificationViewModel>
    {

        IUserRepository userRepository;
        public NotificationConvertAction(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Process(Noftication source, NotificationViewModel destination, ResolutionContext context)
        {
            var author = userRepository.GetById(ObjectId.Parse(source.AuthorId));
            destination.AuthorAvatar = author?.AvatarHash;
            destination.AuthorName = $"{author?.FirstName} {author?.LastName}";
        }
    }
}
