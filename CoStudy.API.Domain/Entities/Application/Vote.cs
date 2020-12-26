using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class UpVote:Entity
    {
        [BsonElement("object_vote_id")]
        [JsonPropertyName("object_vote_id")]
        public string ObjectVoteId { get; set; }

        [BsonElement("upvote_by")]
        [JsonPropertyName("upvote_by")]
        public string UpVoteBy { get; set; }
    }
    
    public class DownVote : Entity
    {
        [BsonElement("object_vote_id")]
        [JsonPropertyName("object_vote_id")]
        public string ObjectVoteId { get; set; }

        [BsonElement("downvote_by")]
        [JsonPropertyName("downvote_by")]
        public string DownVoteBy { get; set; }
    }
}
