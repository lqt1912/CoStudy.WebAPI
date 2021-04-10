using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Validator
{


    /// <summary>
    /// Class ListRequiredAttribute
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class ListRequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the maximum count.
        /// </summary>
        /// <value>
        /// The maximum count.
        /// </value>
        public int MaxCount { get; set; }
        /// <summary>
        /// Gets or sets the minimum count.
        /// </summary>
        /// <value>
        /// The minimum count.
        /// </value>
        public int MinCount { get; set; }
        /// <summary>
        /// Gets or sets the lower limit message.
        /// </summary>
        /// <value>
        /// The lower limit message.
        /// </value>
        public string LowerLimitMessage { get; set; }
        /// <summary>
        /// Gets or sets the upper limit message.
        /// </summary>
        /// <value>
        /// The upper limit message.
        /// </value>
        public string UpperLimitMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListRequiredAttribute"/> class.
        /// </summary>
        public ListRequiredAttribute()
        {

        }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        ///   <see langword="true" /> if the specified value is valid; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public override bool IsValid(object value)
        {
            if (value == null)
                throw new Exception(ErrorMessage);

            IList list = (IList)value;

            if (list.Count == 0)
                throw new Exception(ErrorMessage);

            if (list.Count < MinCount)
                throw new Exception(LowerLimitMessage);

            if (list.Count > MaxCount)
                throw new Exception(UpperLimitMessage);

            return true;
        }
    }
}
