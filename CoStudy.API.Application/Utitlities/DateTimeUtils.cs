
using System;
using System.Globalization;

namespace CoStudy.API.Application.Utilities
{
    public static class DateTimeUtils
    {
        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
        }

        public static DateTime Truncate(this DateTime? date, long resolution)
        {
            var dateVal = date != null && date.HasValue ? date.Value : DateTime.Now;
            return new DateTime(dateVal.Ticks - (dateVal.Ticks % resolution), dateVal.Kind);
        }

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
