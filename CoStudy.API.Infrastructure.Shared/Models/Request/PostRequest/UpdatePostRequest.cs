using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class UpdatePostRequest
    {
              [JsonPropertyName("post_id")]
        public string PostId { get; set; }

              [JsonPropertyName("title")]
        public string Title { get; set; }

              [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

              [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

              [JsonPropertyName("fields")]
        public UpdatePostLevelRequest Fields { get; set; }

    }
}
