﻿using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class follow. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Follow : Entity
    {
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [BsonElement("full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>
        /// The avatar.
        /// </value>
        [BsonElement("avatar")]
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// Gets or sets the follow date.
        /// </summary>
        /// <value>
        /// The follow date.
        /// </value>
        [BsonElement("follow_date")]
        [JsonPropertyName("follow_date")]
        public DateTime? FollowDate { get; set; }


        /// <summary>
        /// Gets or sets from identifier.
        /// </summary>
        /// <value>
        /// From identifier.
        /// </value>
        [BsonElement("from_id")]
        [JsonPropertyName("from_id")]
        public string FromId { get; set; }

        /// <summary>
        /// Converts to id.
        /// </summary>
        /// <value>
        /// To identifier.
        /// </value>
        [BsonElement("to_id")]
        public string ToId { get; set; }
    }
}
