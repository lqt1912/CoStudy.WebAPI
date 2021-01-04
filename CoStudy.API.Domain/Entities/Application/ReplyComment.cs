using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class ReplyComment : Entity
    {
        public ReplyComment() : base()
        {

        }

        [BsonElement("parent_id")]
        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [BsonElement("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }


        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [BsonElement("upvote_count")]
        [JsonPropertyName("upvote_count")]
        public int UpvoteCount { get; set; }

        [BsonElement("downvote_count")]
        [JsonPropertyName("downvote_count")]
        public int DownvoteCount { get; set; }
        [JsonPropertyName("is_vote_by_current")]
        [BsonElement("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        [JsonPropertyName("is_downvote_by_current")]
        [BsonElement("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }
    }
}
