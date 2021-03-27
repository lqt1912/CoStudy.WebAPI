using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Request
{
    /// <summary>
    /// Class AuthenticateRequest
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        public string Password { get; set; }
    }
}
