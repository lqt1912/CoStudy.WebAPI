using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class document
    /// </summary>
    public class Document :Entity
    {
        /// <summary>
        /// Gets or sets the name of the local.
        /// </summary>
        /// <value>
        /// The name of the local.
        /// </value>
        [BsonElement("local_name")]
        public string LocalName { get; set; }


        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>
        /// The name of the server.
        /// </value>
        [BsonElement("server_name")]
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the upload by.
        /// </summary>
        /// <value>
        /// The upload by.
        /// </value>
        [BsonElement("upload_by")]
        public string UploadBy { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [BsonElement("object_id")]
        public string  ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        [BsonElement("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [BsonElement("length")]
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        [BsonElement("update_date")]
        public DateTime UpdateDate { get; set; }

    }
}
