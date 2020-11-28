using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
