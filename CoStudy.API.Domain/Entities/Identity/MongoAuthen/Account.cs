using CoStudy.API.Domain.Entities.BaseEntity;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Identity.MongoAuthen
{
    /// <summary>
    /// Class Account
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Account : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        public Account() : base()
        {
            RefreshTokens = new List<RefreshToken>();
        }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [accept terms].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [accept terms]; otherwise, <c>false</c>.
        /// </value>
        public bool AcceptTerms { get; set; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Role Role { get; set; }
        /// <summary>
        /// Gets or sets the verification token.
        /// </summary>
        /// <value>
        /// The verification token.
        /// </value>
        public string VerificationToken { get; set; }
        /// <summary>
        /// Gets or sets the verified.
        /// </summary>
        /// <value>
        /// The verified.
        /// </value>
        public DateTime? Verified { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is verified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is verified; otherwise, <c>false</c>.
        /// </value>
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        /// <summary>
        /// Gets or sets the reset token.
        /// </summary>
        /// <value>
        /// The reset token.
        /// </value>
        public string ResetToken { get; set; }
        /// <summary>
        /// Gets or sets the reset token expires.
        /// </summary>
        /// <value>
        /// The reset token expires.
        /// </value>
        public DateTime? ResetTokenExpires { get; set; }
        /// <summary>
        /// Gets or sets the password reset.
        /// </summary>
        /// <value>
        /// The password reset.
        /// </value>
        public DateTime? PasswordReset { get; set; }
        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }
        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        /// <value>
        /// The updated.
        /// </value>
        public DateTime? Updated { get; set; }
        /// <summary>
        /// Gets or sets the refresh tokens.
        /// </summary>
        /// <value>
        /// The refresh tokens.
        /// </value>
        public List<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Ownses the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
