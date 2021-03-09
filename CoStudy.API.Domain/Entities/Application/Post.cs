using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// CLass Post. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Post : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class.
        /// </summary>
        public Post() : base()
        {
            StringContents = new List<PostContent>();
            MediaContents = new List<Image>();
            Comments = new List<Comment>();
            Fields = new List<Field>();
            CommentCount = 0;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        [BsonElement("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the author avatar.
        /// </summary>
        /// <value>
        /// The author avatar.
        /// </value>
        [BsonElement("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }


        /// <summary>
        /// Gets or sets the upvote.
        /// </summary>
        /// <value>
        /// The upvote.
        /// </value>
        [BsonElement("upvote")]
        [JsonPropertyName("upvote")]
        public int Upvote { get; set; }

        /// <summary>
        /// Gets or sets the downvote.
        /// </summary>
        /// <value>
        /// The downvote.
        /// </value>
        [BsonElement("downvote")]
        [JsonPropertyName("downvote")]
        public int Downvote { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// String content of posts
        /// </summary>
        /// <value>
        /// The string contents.
        /// </value>
        [BsonElement("string_contents")]
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }


        /// <summary>
        /// Media content of post
        /// </summary>
        /// <value>
        /// The media contents.
        /// </value>
        [BsonElement("image_contents")]
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        /// <summary>
        /// Bình luận
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        [BsonElement("comments")]
        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; }


        /// <summary>
        /// Gets or sets the comment count.
        /// </summary>
        /// <value>
        /// The comment count.
        /// </value>
        [BsonElement("comments_count")]
        [JsonPropertyName("comments_countd")]
        public int CommentCount { get; set; }

        /// <summary>
        /// Lĩnh vực của bài post
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [BsonElement("fields")]
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }
    }
}
