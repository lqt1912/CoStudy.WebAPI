using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse
{
    /// <summary>
    /// Class Get Conversation By User Id Response
    /// </summary>
    public class GetConversationByUserIdResponse
    {
        public IEnumerable<ConversationData> Conversations { get; set; }

    }


    public class ConversationData
    {
        /// <summary>
        /// Gets or sets the conversation.
        /// </summary>
        /// <value>
        /// The conversation.
        /// </value>
        public ConversationViewModel Conversation { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
