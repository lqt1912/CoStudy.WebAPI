using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Models.GoogleAuth
{
    /// <summary>
    /// Class GoogleAuth
    /// </summary>
    public class GoogleAuth
    {
        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public string  Provider { get; set; }

        /// <summary>
        /// Gets or sets the identifier token.
        /// </summary>
        /// <value>
        /// The identifier token.
        /// </value>
        public string  IdToken { get; set; }
    }
}
