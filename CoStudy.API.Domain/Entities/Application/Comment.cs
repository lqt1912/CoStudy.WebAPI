﻿using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Comment : Entity
    {
        public Comment() : base()
        {
            Replies = new List<ReplyComment>();
        }

        [BsonElement("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        [BsonElement("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }


        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [BsonElement("replies")]
        [JsonPropertyName("replies")]
        public List<ReplyComment> Replies { get; set; }

    }
}
