using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class ReplyComment:Entity
    {
        public ReplyComment():base()
        {

        }

        [BsonElement("parent_id")]
        public string ParentId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("status")]
        public ItemStatus Status { get; set; }


        [BsonElement("created_date")]
        public DateTime? CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
