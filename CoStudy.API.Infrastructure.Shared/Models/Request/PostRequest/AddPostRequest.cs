using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Common.Constants;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddPostRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("stringContents")]
        public List<PostContent> StringContents { get; set; }

        [JsonPropertyName("imageContents")]
        public List<Image> MediaContents { get; set; }

        [JsonPropertyName("fields")]
        public IEnumerable<ObjectLevel> Fields { get; set; }

        [JsonPropertyName("postType")]
        public PostType PostType { get; set; }
    }
}
