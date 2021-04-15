using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class UserProfileViewModel
    /// </summary>
    public class UserProfileViewModel
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [JsonPropertyName("user")]
        [JsonProperty("user")]
        public UserViewModel? User { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        [JsonProperty("account")]
        [JsonPropertyName("account")]
        public Account? Account { get; set; }

        /// <summary>
        /// Gets or sets the external login.
        /// </summary>
        /// <value>
        /// The external login.
        /// </value>
        [JsonProperty("external_login")]
        [JsonPropertyName("external_login")]
        public ExternalLogin?  ExternalLogin { get; set; }
    }
}
