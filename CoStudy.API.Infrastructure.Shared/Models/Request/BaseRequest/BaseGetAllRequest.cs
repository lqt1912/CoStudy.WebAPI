using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest
{
    public class BaseGetAllRequest
    {
        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }
    }
}
