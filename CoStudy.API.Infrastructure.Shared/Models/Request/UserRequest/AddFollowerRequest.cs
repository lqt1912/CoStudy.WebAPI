﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddFollowerRequest
    {
        public AddFollowerRequest()
        {
            Followers = new List<string>();
        }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("followers")]
        public List<string> Followers { get; set; }
    }
}
