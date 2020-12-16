using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class GetPostsByUserIdResponse
    {
        public GetPostsByUserIdResponse()
        {
            Posts = new List<Post>();
        }

        [JsonPropertyName("posts")]
        public List<Post> Posts { get; set; }
    }
}
