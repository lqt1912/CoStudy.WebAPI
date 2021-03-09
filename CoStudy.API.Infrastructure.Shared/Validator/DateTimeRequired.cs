using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Validator
{
    /// <summary>
    /// DateTime validator attribute
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field)]
    public class DateTimeRequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRequiredAttribute"/> class.
        /// </summary>
        public DateTimeRequiredAttribute()
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
            var strDate = (string)value;
            DateTime dt = new DateTime();

            var convertable = DateTime.TryParseExact(strDate, "yyyy-MM-dd",null, DateTimeStyles.None, out dt);
            if (!convertable)
                throw new Exception(ErrorMessage);
            return true;
        }
    }
}
