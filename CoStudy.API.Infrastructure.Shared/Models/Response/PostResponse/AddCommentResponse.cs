using CoStudy.API.Domain.Entities.Application;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class AddCommentResponse
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

       [JsonPropertyName("comment")]
        public Comment Comment { get; set; }

    }
}
