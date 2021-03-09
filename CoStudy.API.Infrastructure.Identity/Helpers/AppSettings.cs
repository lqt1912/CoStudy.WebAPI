namespace CoStudy.API.Infrastructure.Identity.Helpers
{
    /// <summary>
    /// Class AppSettings
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public string Secret { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        /// <summary>
        /// Gets or sets the refresh token TTL.
        /// </summary>
        /// <value>
        /// The refresh token TTL.
        /// </value>
        public int RefreshTokenTTL { get; set; }

        /// <summary>
        /// Gets or sets the email from.
        /// </summary>
        /// <value>
        /// The email from.
        /// </value>
        public string EmailFrom { get; set; }
        /// <summary>
        /// Gets or sets the SMTP host.
        /// </summary>
        /// <value>
        /// The SMTP host.
        /// </value>
        public string SmtpHost { get; set; }
        /// <summary>
        /// Gets or sets the SMTP port.
        /// </summary>
        /// <value>
        /// The SMTP port.
        /// </value>
        public int SmtpPort { get; set; }
        /// <summary>
        /// Gets or sets the SMTP user.
        /// </summary>
        /// <value>
        /// The SMTP user.
        /// </value>
        public string SmtpUser { get; set; }
        /// <summary>
        /// Gets or sets the SMTP pass.
        /// </summary>
        /// <value>
        /// The SMTP pass.
        /// </value>
        public string SmtpPass { get; set; }
    }
}
