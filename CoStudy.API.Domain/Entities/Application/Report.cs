using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Report
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Report : Entity
    {
        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [BsonElement("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [BsonElement("object_id")]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        [BsonElement("object_type")]
        public string  ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        [BsonElement("reason")]
        public List<string> Reason { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is approve.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is approve; otherwise, <c>false</c>.
        /// </value>
        [BsonElement("is_approved")]
        public bool IsApproved { get; set; } = false;

        /// <summary>
        /// Gets or sets the approved by.
        /// </summary>
        /// <value>
        /// The approved by.
        /// </value>
        [BsonElement("approved_by")]
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets the approve date.
        /// </summary>
        /// <value>
        /// The approve date.
        /// </value>
        [BsonElement("approve_date")]
        public DateTime? ApproveDate { get; set; }


        public Report() :base()
        {
            Reason = new List<string>();
        }

    }
}
