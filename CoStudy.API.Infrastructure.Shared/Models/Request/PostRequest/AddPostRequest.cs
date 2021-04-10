using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    /// <summary>
    /// Class AddPostRequest.
    /// </summary>
    public class AddPostRequest
    {

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the string contents.
        /// </summary>
        /// <value>
        /// The string contents.
        /// </value>
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

        /// <summary>
        /// Gets or sets the media contents.
        /// </summary>
        /// <value>
        /// The media contents.
        /// </value>
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        [JsonPropertyName("fields")]
        public IEnumerable<ObjectLevel> Fields { get; set; }

    }
}
