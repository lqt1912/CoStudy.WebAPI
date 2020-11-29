﻿using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddAdditionalInfoRequest
    {
        public AddAdditionalInfoRequest()
        {
            AdditionalInfos = new List<AdditionalInfo>();
        }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("additional_infos")]
        public List<AdditionalInfo> AdditionalInfos { get; set; }
    }
}
