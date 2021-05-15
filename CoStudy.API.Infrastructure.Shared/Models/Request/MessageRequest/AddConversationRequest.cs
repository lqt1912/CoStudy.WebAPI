using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class AddConversatonRequest
    /// </summary>
    public class AddConversationRequest
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AddConversationRequest"/> class.
        /// </summary>
        public AddConversationRequest()
        {
            Participants = new List<ConversationMember>();
        }

        /// <summary>
        /// Gets or sets the participants.
        /// </summary>
        /// <value>
        /// The participants.
        /// </value>
        [JsonPropertyName("participants")]
        public List<ConversationMember> Participants { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
