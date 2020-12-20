using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddFieldRequest
    {

       

        [JsonPropertyName("field_image_hash")]
        public string ImageHash { get; set; }

        [JsonPropertyName("field_value")]
        public string UserField { get; set; }
    }
}
