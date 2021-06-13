using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.FieldRequest
{
        public class GetAllFieldRequest : BaseGetAllRequest
    {
              [JsonProperty("group_id")]
        [JsonPropertyName("group_id")]
        public string GroupId { get; set; }
    }
}
