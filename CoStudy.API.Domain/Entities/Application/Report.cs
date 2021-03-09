using CoStudy.API.Domain.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Report
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Report:Entity
    {
        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        public string  AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public List<string> Reason { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approve.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is approve; otherwise, <c>false</c>.
        /// </value>
        public bool IsApprove { get; set; }
        /// <summary>
        /// Gets or sets the approved by.
        /// </summary>
        /// <value>
        /// The approved by.
        /// </value>
        public string ApprovedBy { get; set; }

    }
}
