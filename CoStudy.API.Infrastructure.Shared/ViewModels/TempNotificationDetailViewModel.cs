using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class TempNotificationDetailViewModel
    {
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonProperty("creator_name")]
        [JsonPropertyName("creator_name")]
        public string CreatorName { get; set; }


        [JsonProperty("creator_id")]
        [JsonPropertyName("creator_id")]
        public string CreatorId { get; set; }

        [JsonProperty("receiver_id")]
        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }

        [JsonProperty("is_read")]
        [JsonPropertyName("is_read")]
        public bool? IsRead { get; set; } = false;
    }
}
