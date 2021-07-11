using System;
using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Shared.Validator
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringRequiredAttribute : ValidationAttribute
    {
        public int MaxLength { get; set; } = 255;
        public int MinLength { get; set; } = 0;

        public string LowerLimitMessage { get; set; }
        public string UpperLimitMessage { get; set; }

        public StringRequiredAttribute()
        {

        }

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
