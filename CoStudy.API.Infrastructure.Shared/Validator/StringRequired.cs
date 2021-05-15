using System;
using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Shared.Validator
{
    /// <summary>
    /// Required string attribute
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringRequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>
        /// The maximum length.
        /// </value>
        public int MaxLength { get; set; } = 255;
        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>
        /// The minimum length.
        /// </value>
        public int MinLength { get; set; } = 0;

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
        /// Initializes a new instance of the <see cref="StringRequiredAttribute"/> class.
        /// </summary>
        public StringRequiredAttribute()
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
            {
                throw new Exception(ErrorMessage);
            }

            if (value.ToString().Length == 0)
            {
                throw new Exception(ErrorMessage);
            }

            if (value.ToString().Length < MinLength)
            {
                throw new Exception(LowerLimitMessage);
            }

            if (value.ToString().Length > MaxLength)
            {
                throw new Exception(UpperLimitMessage);
            }

            return true;
        }
    }
}
