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
            ConnectionGroupIds = new List<string>();
        }

        [BsonElement("connection_group_ids")]
        [JsonPropertyName("connection_group_ids")]
        public List<string> ConnectionGroupIds { get; set; }
    }
}
