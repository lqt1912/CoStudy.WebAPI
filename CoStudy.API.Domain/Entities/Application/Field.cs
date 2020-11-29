using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Field:Entity
    {
        public Field() : base()
        {

        }

        [BsonElement("value")]
        public string Value { get; set; }

        [BsonElement("thumbnail_image")]
        public Image ThumbnailImage { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }


    }
}
