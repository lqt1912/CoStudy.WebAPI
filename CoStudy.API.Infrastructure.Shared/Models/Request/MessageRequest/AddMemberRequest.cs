using CoStudy.API.Infrastructure.Shared.Validator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    /// <summary>
    /// Class AddMemberRequest.
    /// </summary>
    public class AddMemberRequest
    {
        /// <summary>
        /// Gets or sets the conversation identifier.
        /// </summary>
        /// <value>
        /// The conversation identifier.
        /// </value>
        [StringRequired]
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the user ids.
        /// </summary>
        /// <value>
        /// The user ids.
        /// </value>
        [ListRequired]
        public IEnumerable<string> UserIds { get; set; }
    }
}
