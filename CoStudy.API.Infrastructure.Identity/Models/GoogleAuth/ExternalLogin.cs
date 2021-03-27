using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Models.GoogleAuth
{
    /// <summary>
    /// Class ExternalLogin
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class ExternalLogin : Entity
    {
        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        /// <value>
        /// The login provider.
        /// </value>
        [BsonElement("login_provider")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        /// <value>
        /// The provider key.
        /// </value>
        [BsonElement("provider_key")]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the display name of the provider.
        /// </summary>
        /// <value>
        /// The display name of the provider.
        /// </value>
        [BsonElement("provider_display_name")]
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [BsonElement("user_id")]
        public string UserId { get; set; }
    }
}
