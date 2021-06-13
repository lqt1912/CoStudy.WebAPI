using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CoStudy.API.Infrastructure.Shared.Validator
{


        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class ListRequiredAttribute : ValidationAttribute
    {
              public int MaxCount { get; set; }
              public int MinCount { get; set; }
              public string LowerLimitMessage { get; set; }
              public string UpperLimitMessage { get; set; }

           public ListRequiredAttribute()
        {

        }

                 public override bool IsValid(object value)
        {
            if (value == null)
            {
                throw new Exception(ErrorMessage);
            }

            IList list = (IList)value;

            if (list.Count == 0)
            {
                throw new Exception(ErrorMessage);
            }

            if (list.Count < MinCount)
            {
                throw new Exception(LowerLimitMessage);
            }

            if (list.Count > MaxCount)
            {
                throw new Exception(UpperLimitMessage);
            }

            return true;
        }
    }
}
