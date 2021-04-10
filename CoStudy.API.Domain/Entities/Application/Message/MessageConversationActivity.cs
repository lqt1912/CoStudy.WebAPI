using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class MesageConversationActivity
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.Application.MessageBase" />
    public class MessageConversationActivity : MessageBase
    {
        /// <summary>
        /// Gets or sets the activity detail.
        /// </summary>
        /// <value>
        /// The activity detail.
        /// </value>
        [BsonElement("activity_detail")]
        public IEnumerable<string> ActivityDetail { get; set; }
    }
}
