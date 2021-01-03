using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class ClientGroup : Entity
    {
        public ClientGroup() : base()
        {
            UserIds = new List<string>();
        
        }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }


        [BsonElement("user_ids")]
        [JsonPropertyName("user_ids")]
        public List<string> UserIds { get; set; }
    }
}
