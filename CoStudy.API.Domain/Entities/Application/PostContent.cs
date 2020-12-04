using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class PostContent:Entity
    {
        public PostContent():base()
        {

        }

        [BsonElement("content_type")]
        public  PostContentType ContentType { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

    }
}
