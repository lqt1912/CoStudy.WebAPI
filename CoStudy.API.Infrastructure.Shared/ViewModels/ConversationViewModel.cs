using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
       public class ConversationViewModel
    {
              [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

              [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

              [JsonProperty("participants")]
        [JsonPropertyName("participants")]
        public IEnumerable<ConversationMemberViewModel> Participants { get; set; }


              [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

              [JsonPropertyName("client_group_id")]
        [JsonProperty("client_group_id")]
        public string ClientGroupId { get; set; }

              [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

              [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}
