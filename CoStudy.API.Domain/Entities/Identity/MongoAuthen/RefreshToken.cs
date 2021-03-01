using CoStudy.API.Domain.Entities.BaseEntity;
using System;

namespace CoStudy.API.Domain.Entities.Identity.MongoAuthen
{
    /// <summary>
    /// Class Refresh Token
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class RefreshToken : Entity
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Account Account { get; set; }
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }
        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>
        /// The expires.
        /// </value>
        public DateTime Expires { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired => DateTime.UtcNow >= Expires;
        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }
        /// <summary>
        /// Gets or sets the created by ip.
        /// </summary>
        /// <value>
        /// The created by ip.
        /// </value>
        public string CreatedByIp { get; set; }
        /// <summary>
        /// Gets or sets the revoked.
        /// </summary>
        /// <value>
        /// The revoked.
        /// </value>
        public DateTime? Revoked { get; set; }
        /// <summary>
        /// Gets or sets the revoked by ip.
        /// </summary>
        /// <value>
        /// The revoked by ip.
        /// </value>
        public string RevokedByIp { get; set; }
        /// <summary>
        /// Gets or sets the replaced by token.
        /// </summary>
        /// <value>
        /// The replaced by token.
        /// </value>
        public string ReplacedByToken { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive => Revoked == null && !IsExpired;

    }

    /// <summary>
    /// 
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// The admin
        /// </summary>
        Admin,
        /// <summary>
        /// The user
        /// </summary>
        User
    }

}
