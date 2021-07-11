using CoStudy.API.Domain.Base;
using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class SearchHistory : Entity
    {
        [BsonElement("user_id")]
        public string  UserId { get; set; }

        [BsonElement("history_type")]
        public HistoryType HistoryType { get; set; }

        [BsonElement("post_value")]
        public object PostValue { get; set; }

        [BsonElement("user_value")]
        public object UserValue { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public enum HistoryType
    {
        Post,
        User
    }

   
}
