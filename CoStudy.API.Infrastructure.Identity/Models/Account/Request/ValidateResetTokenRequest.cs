using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Request
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
