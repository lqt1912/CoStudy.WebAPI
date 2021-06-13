using CoStudy.API.Domain.Entities.Application;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddUserRequest
    {
        [Required]
        [JsonPropertyName("first_name")]
        public string FisrtName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }


        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }


        [JsonPropertyName("accept_term")]
        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        //   [Required]
        public string Title { get; set; }

        public bool IsExternalRegister { get; set; } 
    }
}