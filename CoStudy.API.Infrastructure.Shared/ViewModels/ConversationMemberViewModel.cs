using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ConversationMemberViewModel
    {

        [JsonProperty("member_id")]
        [JsonPropertyName("member_id")]
        public string MemberId { get; set; }

        [JsonProperty("member_name")]
        [JsonPropertyName("member_name")]
        public string MemberName { get; set; }

        [JsonPropertyName("nickname")]
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("member_avatar")]
        [JsonPropertyName("member_avatar")]
        public string MemberAvatar { get; set; }

        [JsonProperty("date_join")]
        [JsonPropertyName("date_join")]
        public DateTime? DateJoin { get; set; }

        [JsonProperty("join_by")]
        [JsonPropertyName("join_by")]
        public string JoinBy { get; set; }

        [JsonProperty("join_by_name")]
        [JsonPropertyName("join_by_name")]
        public string JoinByName { get; set; }

        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public ConversationRole Role { get; set; }

    }
}
