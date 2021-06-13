using System;
using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Shared.Validator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class IntegerRequiredAttribute : ValidationAttribute
    {
        public IntegerRequiredAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var strVal = (string)value;

            if (!int.TryParse(strVal, out var intVal))
            {
                throw new Exception("Không chuyển đổi được");
            }

            if (intVal < 0)
            {
                throw new Exception("Vui lòng nhập số dương");
            }

            return true;
        }
    }
}
