using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class AddPostRequest
    {

              [JsonPropertyName("title")]
        public string Title { get; set; }

              [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

              [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        [JsonPropertyName("fields")]
        public IEnumerable<ObjectLevel> Fields { get; set; }

    }
}
