﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class AddPostRequest
    {

        [JsonPropertyName("title")]
        public string Title { get; set; }


    }
}
