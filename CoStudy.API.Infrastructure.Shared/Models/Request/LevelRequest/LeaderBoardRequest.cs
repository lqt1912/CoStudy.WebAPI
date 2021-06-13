using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest
{
    public class LeaderBoardRequest : BaseGetAllRequest
    {
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public string FieldId { get; set; }
    }
}
