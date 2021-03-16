using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    /// <summary>
    /// Class Get Post By User Request
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequ                     [kkkest" />
    public class GetPostByUserRequest :BaseGetAllRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user ientifer 
        /// </value>
        [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string  UserId { get; set; }
    }
}
