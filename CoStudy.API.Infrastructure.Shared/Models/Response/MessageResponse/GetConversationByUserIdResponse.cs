using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse
{
       public class GetConversationByUserIdResponse
    {
        public IEnumerable<ConversationData> Conversations { get; set; }

    }


    public class ConversationData
    {
              public ConversationViewModel Conversation { get; set; }

              public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
