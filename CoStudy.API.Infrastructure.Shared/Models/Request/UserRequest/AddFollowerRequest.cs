using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddFollowerRequest
    {
        public AddFollowerRequest()
        {
            Followers = new List<string>();
        }



        [JsonPropertyName("followers")]
        public List<string> Followers { get; set; }
    }
}
