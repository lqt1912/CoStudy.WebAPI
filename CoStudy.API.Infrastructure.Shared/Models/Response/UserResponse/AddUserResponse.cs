using CoStudy.API.Domain.Entities.Application;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse
{
    public class AddUserResponse
    {
        [JsonPropertyName("id")]
        public ObjectId Id { get; set; }


        [JsonPropertyName("first_name")]
        public string FisrtName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }


        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }


        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }
}
