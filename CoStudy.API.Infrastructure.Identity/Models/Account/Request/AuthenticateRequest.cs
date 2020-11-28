using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Request
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
