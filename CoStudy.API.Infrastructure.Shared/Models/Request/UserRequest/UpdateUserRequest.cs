using CoStudy.API.Domain.Entities.Application;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class UpdateUserRequest. 
    /// </summary>
    public class UpdateUserRequest
    {

        /// <summary>
        /// Gets or sets the name of the fisrt.
        /// </summary>
        /// <value>
        /// The name of the fisrt.
        /// </value>
        [JsonPropertyName("first_name")]
        public string FisrtName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }


        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

    }
}
