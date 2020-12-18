using CoStudy.API.Domain.Entities.BaseEntity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class PostContent : Entity
    {
        public PostContent() : base()
        {

        }

        [BsonElement("content_type")]
        [JsonPropertyName("content_type")]
        [FromForm(Name = "content_type")]
        public PostContentType ContentType { get; set; }

        [BsonElement("content")]
        [JsonPropertyName("content")]
        [FromForm(Name = "content")]
        public string Content { get; set; }

    }
}
