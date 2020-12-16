using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse
{
    public class GetConversationByUserIdResponse
    {
        [JsonPropertyName("conversations")]
        public List<Tuple<Conversation,Message>> Conversations { get; set; }

    }
}
