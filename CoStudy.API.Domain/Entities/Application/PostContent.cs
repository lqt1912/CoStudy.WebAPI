using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class PostContent:Entity
    {
        public PostContent():base()
        {

        }

        [BsonElement("content_type")]
        [JsonPropertyName("content_type")]
        public  PostContentType ContentType { get; set; }

        [BsonElement("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

    }
}
