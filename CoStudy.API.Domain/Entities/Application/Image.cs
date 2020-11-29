using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Image:Entity
    {
        public Image():base()
        {

        }

        [BsonElement("discription")]
        public string Discription { get; set; }

        [BsonElement("image_url")]
        public string ImageUrl { get; set; }

        [BsonElement("base64_string")]
        public string Base64String { get; set; }

        [BsonElement("original_width")]
        public int OriginalWidth { get; set; }

        [BsonElement("original_height")]
        public int OriginalHeight { get; set; }


        [BsonElement("compress_ratio")]
        public double CompressRatio { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}
