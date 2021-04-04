using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class AdditionalInformation
    /// </summary>
    public class AdditionalInfomation
    {

        /// <summary>
        /// Gets or sets the name of the information.
        /// </summary>
        /// <value>
        /// The name of the information.
        /// </value>
        [JsonProperty("information_name")]
        [JsonPropertyName("information_name")]
        public string InformationName { get; set; }


        /// <summary>
        /// Gets or sets the information value.
        /// </summary>
        /// <value>
        /// The information value.
        /// </value>
        [JsonProperty("information_value")]
        [JsonPropertyName("information_value")]
        public string InformationValue { get; set; }
    }
}
