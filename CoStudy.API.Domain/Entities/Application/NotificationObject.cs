using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class NotificationObject
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class NotificationObject : Entity
    {
        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [BsonElement("object_id")]
        public string ObjectId { get; set; }
        

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [BsonElement("owner_id")]
        public string  OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the type of the notification.
        /// </summary>
        /// <value>
        /// The type of the notification.
        /// </value>
        [BsonElement("notification_type")]
        public string NotificationType { get; set; }

    }
}
