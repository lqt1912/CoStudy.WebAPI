
using System;
using System.Globalization;

namespace CoStudy.API.Application.Utilities
{
    /// <summary>
    /// Class DateTimeUtils.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// <para>Truncates a DateTime to a specified resolution.</para>
        /// <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
        /// </summary>
        /// <param name="date">The DateTime object to truncate</param>
        /// <param name="resolution">e.g. to round to nearest second, TimeSpan.TicksPerSecond</param>
        /// <returns>Truncated DateTime</returns>
        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
        }

        /// <summary>
        /// Truncates the specified resolution.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="resolution">The resolution.</param>
        /// <returns></returns>
        public static DateTime Truncate(this DateTime? date, long resolution)
        {
            var dateVal = date != null && date.HasValue ? date.Value : DateTime.Now;
            return new DateTime(dateVal.Ticks - (dateVal.Ticks % resolution), dateVal.Kind);
        }

        /// <summary>
        /// Converts the date.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="dateFormat">The date format.</param>
        /// <returns></returns>
        public static DateTime ConvertDate(string dateTime, string dateFormat)
        {
            if (DateTime.TryParseExact(dateTime, dateFormat, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime date))
                return date;
            else return DateTime.Now;
        }

        public static bool IsEqualDayByDay(DateTime dateTime1, DateTime dateTime2)
        {
            if (dateTime1.Day != dateTime2.Day || dateTime1.Month != dateTime2.Month || dateTime1.Year != dateTime2.Year)
                return false;
            return true;
        }

    }
}
