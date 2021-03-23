﻿using CoStudy.API.Infrastructure.Shared.Validator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest
{
    /// <summary>
    /// Class UserAddFieldRequest
    /// </summary>
    public class UserAddFieldRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [JsonPropertyName("user_id")]
        [StringRequired]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        [ListRequired]
        [JsonPropertyName("field_id")]
        public IEnumerable<string> FieldId { get; set; }
    }
}
