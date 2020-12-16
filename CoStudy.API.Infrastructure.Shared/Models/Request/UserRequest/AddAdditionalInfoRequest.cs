using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddAdditionalInfoRequest
    {
        public AddAdditionalInfoRequest()
        {
            AdditionalInfos = new List<AdditionalInfo>();
        }

        [JsonPropertyName("additional_infos")]
        public List<AdditionalInfo> AdditionalInfos { get; set; }
    }
}
