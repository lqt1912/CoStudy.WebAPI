using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Request
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
