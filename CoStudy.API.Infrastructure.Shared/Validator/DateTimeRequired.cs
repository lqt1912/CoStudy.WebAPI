using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Validator
{
    [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field)]
    public class DateTimeRequiredAttribute : ValidationAttribute
    {
        public DateTimeRequiredAttribute()
        {

        }

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
