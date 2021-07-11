using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Common.Constants;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Post : Entity
    {
        public Post() : base()
        {
            StringContents = new List<PostContent>();
            MediaContents = new List<Image>();
        }

        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [BsonElement("string_contents")]
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

        [BsonElement("image_contents")]
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("post_type")]
        [JsonPropertyName("post_type")]
        public PostType PostType { get; set; }
    }
}
