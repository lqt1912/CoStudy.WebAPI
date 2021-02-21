using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
                return true;

            string strVal = (string)value;

            int intVal;
            if (!int.TryParse(strVal, out intVal))
                throw new Exception("Không chuyển đổi được");
            else
                if (intVal < 0)
                throw new Exception("Vui lòng nhập số dương");

            return true;
        }
    }
}
