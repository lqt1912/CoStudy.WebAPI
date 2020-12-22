using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class UpdatePostRequest
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// String content of posts
        /// </summary>
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

        /// <summary>
        /// Media content of post
        /// </summary>
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        /// <summary>
        /// Lĩnh vực của bài post
        /// </summary>
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }


    }
}
