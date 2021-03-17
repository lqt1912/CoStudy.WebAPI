using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class ConversationMember.
    /// </summary>
    public class ConversationMember
    {
        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        [BsonElement("member_id")]
        [JsonPropertyName("member_id")]
        public string MemberId { get; set; }


        [BsonElement("nickname")]
        [JsonPropertyName("nickname")]
        public string  Nickname { get; set; }

        /// <summary>
        /// Gets or sets the date join.
        /// </summary>
        /// <value>
        /// The date join.
        /// </value>
        [BsonElement("date_join")]
        [JsonPropertyName("date_join")]
        public DateTime? DateJoin { get; set; }



        /// <summary>
        /// Gets or sets the join by.
        /// </summary>
        /// <value>
        /// The join by.
        /// </value>
        [BsonElement("join_by")]
        [JsonPropertyName("join_by")]
        public string JoinBy { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        [BsonElement("role")]
        [JsonPropertyName("role")]
        public ConversationRole Role { get; set; } = ConversationRole.Admin;

    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConversationRole
    {
        /// <summary>
        /// The admin
        /// </summary>
        Admin,
        /// <summary>
        /// The member
        /// </summary>
        Member
    }

}
