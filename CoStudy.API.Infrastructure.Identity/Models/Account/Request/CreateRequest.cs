using CoStudy.API.Domain.Entities.Identity;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Request
{
    public class CreateRequest
    {
       

        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
