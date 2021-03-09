using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// CLass Logging. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Logging : Entity
    {
        /// <summary>
        /// Gets or sets the request method.
        /// </summary>
        /// <value>
        /// The request method.
        /// </value>
        public string RequestMethod { get; set; }
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }
        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        /// <value>
        /// The request path.
        /// </value>
        public string RequestPath { get; set; }
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the time elapsed.
        /// </summary>
        /// <value>
        /// The time elapsed.
        /// </value>
        public double TimeElapsed { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime? CreatedDate { get; set; }
    }
}
