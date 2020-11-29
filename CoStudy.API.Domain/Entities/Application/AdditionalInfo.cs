using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class AdditionalInfo { 


        [BsonElement("info_type")]
        public InfoType InfoType { get; set; }

        [BsonElement("info_value")]
        public string InfoValue { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}
