using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class SearchHistoryViewModel
    {
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string  OId { get; set; }

        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string  UserId { get; set; }

        [JsonProperty("history_type")]
        [JsonPropertyName("history_type")]
        public HistoryType HistoryType { get; set; }

        [JsonProperty("post_value")]
        [JsonPropertyName("post_value")]
        public FilterRequest PostValue { get; set; }

        [JsonProperty("user_value")]
        [JsonPropertyName("user_value")]
        public FilterUserRequest UserValue { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
