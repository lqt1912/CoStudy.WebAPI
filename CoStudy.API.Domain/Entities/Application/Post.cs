using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Post : Entity
    {
        public Post() : base()
        {
            StringContents = new List<PostContent>();
            MediaContents = new List<Image>();
            Comments = new List<Comment>();
            Fields = new List<Field>();
            CommentCount = 0;
        }

        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("upvote")]
        [JsonPropertyName("upvote")]
        public int Upvote { get; set; }

        [BsonElement("downvote")]
        [JsonPropertyName("downvote")]
        public int Downvote { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// String content of posts
        /// </summary>
        [BsonElement("string_contents")]
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }


        /// <summary>
        /// Media content of post
        /// </summary>
        [BsonElement("image_contents")]
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        /// <summary>
        /// Bình luận
        /// </summary>
        /// 
        [BsonElement("comments")]
        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; }


        [BsonElement("comments_count")]
        [JsonPropertyName("comments_countd")]
        public int CommentCount { get; set; }
        
        /// <summary>
        /// Lĩnh vực của bài post
        /// </summary>
        /// 
        [BsonElement("fields")]
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }
    }
}
