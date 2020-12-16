using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddFieldRequest
    {

        [JsonPropertyName("field_image")]
        public IFormFile Image { get; set; }

        [JsonPropertyName("field_value")]
        public string UserField { get; set; }
    }
}
