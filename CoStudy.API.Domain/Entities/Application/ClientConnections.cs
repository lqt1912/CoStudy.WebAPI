using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class ClientConnections : Entity
    {
        public ClientConnections() : base()
        {
            ClientConnection = new List<string>();
        }

        [BsonElement("client_connections")]
        [JsonPropertyName("client_connections")]
        public List<string> ClientConnection { get; set; }
    }
}
