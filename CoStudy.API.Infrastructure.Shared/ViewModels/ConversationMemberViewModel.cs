using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ConversationMemberViewModel
    {

        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

        [JsonProperty("member_name")]
        [JsonPropertyName("member_name")]
        public string MemberName { get; set; }

        [JsonProperty("member_avatar")]
        [JsonPropertyName("member_avatar")]
        public string  MemberAvatar { get; set; }

        [JsonProperty("date_join")]
        [JsonPropertyName("date_join")]
        public DateTime? DateJoin { get; set; }

        [JsonProperty("join_by")]
        [JsonPropertyName("join_by")]
        public string  JoinBy { get; set; }

        [JsonProperty("is_admin")]
        [JsonPropertyName("is_admin")]
        public bool? IsAdmin { get; set; }

    }
}
