using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class Get Post By User Request
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class GetPostByUserRequest : BaseGetAllRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user ientifer 
        /// </value>
        [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
