using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Conversation:Entity
    {
        public Conversation():base()
        {
            Messages = new List<Message>();
        }

        [BsonElement("host_id")]
        public string HostId { get; set; }

        [BsonElement("guest_id")]
        public string GuestId { get; set; }

        [BsonElement("messages")]
        public  List<Message> Messages { get; set; }

        [BsonElement("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}
