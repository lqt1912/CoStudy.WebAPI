using System;
using System.Collections.Generic;

namespace CoStudy.API.Application.Utilities
{
    /// <summary>
    /// Class DateUtils.
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Converts to string with default.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToStringWithDefault(DateTime? date, string format = "")
        {
            var val = date.HasValue ? date.Value : DateTime.Now;
            return !string.IsNullOrEmpty(format) ? val.ToString(format) : val.ToString();
        }

        /// <summary>
        /// Eaches the calendar day.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
