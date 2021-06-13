using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class ModifiedCommentStatusRequest
    {
        [JsonProperty("comment_id")]
        [JsonPropertyName("comment_id")]
        public  string  CommentId { get; set; }
        
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }
    }
}
