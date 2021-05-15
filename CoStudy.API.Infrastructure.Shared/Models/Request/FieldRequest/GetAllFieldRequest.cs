using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.FieldRequest
{
    /// <summary>
    /// Class GetAllFieldRequest
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class GetAllFieldRequest : BaseGetAllRequest
    {
        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        [JsonProperty("group_id")]
        [JsonPropertyName("group_id")]
        public string GroupId { get; set; }
    }
}
