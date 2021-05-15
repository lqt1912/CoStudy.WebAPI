using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class UpdatePostRequest
    /// </summary>
    public class UpdatePostRequest
    {
        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// String content of posts
        /// </summary>
        /// <value>
        /// The string contents.
        /// </value>
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

        /// <summary>
        /// Media content of post
        /// </summary>
        /// <value>
        /// The media contents.
        /// </value>
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        [JsonPropertyName("fields")]
        public UpdatePostLevelRequest Fields { get; set; }

    }
}
