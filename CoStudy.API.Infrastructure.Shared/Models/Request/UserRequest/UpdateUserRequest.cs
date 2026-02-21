using CoStudy.API.Domain.Entities.Application;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class UpdateUserRequest
    {
        [JsonPropertyName("firstName")]
        public string FisrtName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("phone")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }
}
