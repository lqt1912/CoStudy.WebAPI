using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class User. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class User : Entity
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [BsonElement("first_name")]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [BsonElement("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [BsonElement("date_of_birth")]
        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [BsonElement("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [BsonElement("phone_number")]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [BsonElement("address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }


        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>
        /// The avatar.
        /// </value>
        [BsonElement("avatar")]
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }

        /// <summary>
        /// Gets or sets the avatar hash.
        /// </summary>
        /// <value>
        /// The avatar hash.
        /// </value>
        [BsonElement("avatar_hash")]
        [JsonPropertyName("avatar_hash")]
        public string AvatarHash { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }




        
        /// <summary>
        /// Thông tin thêm
        /// </summary>
        /// <value>
        /// The additional infos.
        /// </value>
        [BsonElement("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<IDictionary<string, string>> AdditionalInfos { get; set; }


        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [BsonElement("fields")]
        [JsonPropertyName("fields")]
        public List<string> Fields { get; set; }



        /// <summary>
        /// Gets or sets the post upvote.
        /// </summary>
        /// <value>
        /// The post upvote.
        /// </value>
        [BsonElement("post_upvote")]
        [JsonPropertyName("post_upvote")]
        public List<string> PostUpvote { get; set; }

        /// <summary>
        /// Gets or sets the post downvote.
        /// </summary>
        /// <value>
        /// The post downvote.
        /// </value>
        [BsonElement("post_downvote")]
        [JsonPropertyName("post_downvote")]
        public List<string> PostDownvote { get; set; }

        /// <summary>
        /// Gets or sets the post saved.
        /// </summary>
        /// <value>
        /// The post saved.
        /// </value>
        [BsonElement("post_saved")]
        [JsonPropertyName("post_saved")]
        public List<string> PostSaved { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() : base()
        {
            Avatar = new Image();

            AdditionalInfos = new List<IDictionary<string, string>>();
            PostDownvote = new List<string>();
            PostUpvote = new List<string>();
            PostSaved = new List<string>();
            Fields = new List<string>();
        }
    }
}
