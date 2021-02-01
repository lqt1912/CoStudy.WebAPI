using CoStudy.API.Domain.Entities.Application;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse
{
    public class AddNofticationResponse
    {
        public string Id { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        public string Content { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [JsonPropertyName("is_read")]
        public bool? IsRead { get; set; }
    }
}
