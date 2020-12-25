using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Field : Entity
    {
        public Field() : base()
        {

        }

        [BsonElement("value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
