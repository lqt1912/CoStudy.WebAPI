using CoStudy.API.Domain.Entities.Application;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class UpdateUserRequest
    {

              [JsonPropertyName("first_name")]
        public string FisrtName { get; set; }

              [JsonPropertyName("last_name")]
        public string LastName { get; set; }


              [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

              [JsonPropertyName("address")]
        public Address Address { get; set; }

              [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

    }
}
