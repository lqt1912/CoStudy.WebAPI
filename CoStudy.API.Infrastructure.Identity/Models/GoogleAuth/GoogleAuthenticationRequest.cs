using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Identity.Models.GoogleAuth
{
    /// <summary>
    /// Class GoogleAuthenticationRequest
    /// </summary>
    public class GoogleAuthenticationRequest
    {
        /// <summary>
        /// Gets or sets the identifier token.
        /// </summary>
        /// <value>
        /// The identifier token.
        /// </value>
        [JsonProperty("idToken")]
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets the server authentication code.
        /// </summary>
        /// <value>
        /// The server authentication code.
        /// </value>
        [JsonProperty("serverAuthCode")]
        [JsonPropertyName("serverAuthCode")]
        public string ServerAuthCode { get; set; }


        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        [JsonProperty("scopes")]
        [JsonPropertyName("scopes")]
        public IEnumerable<string> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [JsonProperty("user")]
        [JsonPropertyName("user")]
        public GoogleUser User { get; set; }
    }

    /// <summary>
    /// Class GoogleUser
    /// </summary>
    public class GoogleUser
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        [JsonProperty("givenName")]
        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        /// <value>
        /// The name of the family.
        /// </value>
        [JsonProperty("familyName")]
        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the photo.
        /// </summary>
        /// <value>
        /// The photo.
        /// </value>
        [JsonProperty("photo")]
        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
