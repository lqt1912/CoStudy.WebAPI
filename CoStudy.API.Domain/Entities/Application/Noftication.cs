using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Noftication :Entity
    {
        public Noftication() : base()
        {

        }
        [BsonElement("content")]
        public MediaContent Content { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [BsonElement("status")]
        public ItemStatus Status { get; set; }
    }
}
