using System;
using System.Collections.Generic;

namespace CoStudy.API.Application.Utilities
{
    public static class DateUtils
    {
        public static string ConvertToStringWithDefault(DateTime? date, string format = "")
        {
            var val = date.HasValue ? date.Value : DateTime.Now;
            return !string.IsNullOrEmpty(format) ? val.ToString(format) : val.ToString();
        }

        public static IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
