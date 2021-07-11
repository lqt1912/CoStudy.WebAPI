using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class ViolenceWord : Entity
    {
        [BsonElement("value")]
        public string Value { get; set; }

        [BsonElement("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}
