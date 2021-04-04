using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class NotificationDetail
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class NotificationDetail :Entity
    {
        /// <summary>
        /// Gets or sets the notification object identifier.
        /// </summary>
        /// <value>
        /// The notification object identifier.
        /// </value>
        [BsonElement("notification_object_id")]
        public string  NotificationObjectId { get; set; }

        /// <summary>
        /// Gets or sets the creator identifier.
        /// </summary>
        /// <value>
        /// The creator identifier.
        /// </value>
        [BsonElement("creator_id")]
        public string  CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the receiver identifier.
        /// </summary>
        /// <value>
        /// The receiver identifier.
        /// </value>
        [BsonElement("receiver_id")]
        public string  ReceiverId { get; set; }


        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the is read.
        /// </summary>
        /// <value>
        /// The is read.
        /// </value>
        [BsonElement("is_read")]
        public bool? IsRead { get; set; } = false;


        /// <summary>
        /// Gets or sets the is deleted.
        /// </summary>
        /// <value>
        /// The is deleted.
        /// </value>
        [BsonElement("is_deleted")]
        public bool? IsDeleted { get; set; } = false;
    }
}
