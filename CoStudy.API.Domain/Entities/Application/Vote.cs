using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Upvote. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class UpVote : Entity
    {
        /// <summary>
        /// Gets or sets the object vote identifier.
        /// </summary>
        /// <value>
        /// The object vote identifier.
        /// </value>
        [BsonElement("object_vote_id")]
        [JsonPropertyName("object_vote_id")]
        public string ObjectVoteId { get; set; }

        /// <summary>
        /// Gets or sets up vote by.
        /// </summary>
        /// <value>
        /// Up vote by.
        /// </value>
        [BsonElement("upvote_by")]
        [JsonPropertyName("upvote_by")]
        public string UpVoteBy { get; set; }
    }

    /// <summary>
    /// Class Downvote. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class DownVote : Entity
    {
        /// <summary>
        /// Gets or sets the object vote identifier.
        /// </summary>
        /// <value>
        /// The object vote identifier.
        /// </value>
        [BsonElement("object_vote_id")]
        [JsonPropertyName("object_vote_id")]
        public string ObjectVoteId { get; set; }

        /// <summary>
        /// Gets or sets down vote by.
        /// </summary>
        /// <value>
        /// Down vote by.
        /// </value>
        [BsonElement("downvote_by")]
        [JsonPropertyName("downvote_by")]
        public string DownVoteBy { get; set; }
    }
}
