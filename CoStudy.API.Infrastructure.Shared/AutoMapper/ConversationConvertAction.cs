using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class ConversationConvertAction : IMappingAction<Conversation, ConversationViewModel>
    {

        IUserRepository userRepository;
        IMapper mapper;
        public ConversationConvertAction(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public void Process(Conversation source, ConversationViewModel destination, ResolutionContext context)
        {
            //  destination.Participants = mapper.Map<IEnumerable<ConversationMemberViewModel>>(source.Participants);
        }
    }
}
