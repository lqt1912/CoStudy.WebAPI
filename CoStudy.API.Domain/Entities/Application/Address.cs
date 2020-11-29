using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Address
    {

        [BsonElement("district")]
        public string District { get; set; }
        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("detail")]
        public string Detail { get; set; }

        [BsonElement("longtitude")]
        public string Longtitude { get; set; }

        [BsonElement("latitude")]
        public string Latitude { get; set; }

    }
}
