using System;
using System.Globalization;

namespace Moduler.Helpers
{
    public static class DateTimeHelpers
    {
        public static string ToMyString(this DateTime date)
        {
            return date.ToString("o", CultureInfo.InvariantCulture);
        }
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return date.AddDays(1 - date.Day);
        }
        public static int GetQuarter(this DateTime date)
        {
            return (date.Month + 2) / 3;
        }

        public static DateTime GetQuarterStartDate(this DateTime date)
        {
            return new DateTime(date.Year, ((date.GetQuarter() - 1) * 3 + 1), 1);
        }

        public static DateTime GetDateTimeParseExact(this string dateValue)
        {
            return DateTime.ParseExact(dateValue, DateParseOptions(), CultureInfo.InvariantCulture);
        }
        public static string GetDateTimeThenToString(this string dateValue)
        {
            return GetDateTimeParseExact(dateValue).ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("tr-TR"));
        }

        public static string[] DateParseOptions()
        {
            //return new string[] { "dd MMMM yyyy", "M/d/yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM-dd-yyyy" };
            return new string[] { "dd MMMM yyyy", "d.MM.yyyy", "dd.MM.yyyy", "M/d/yyyy", "MM/dd/yyyy", "MM-dd-yyyy" };
        }

        public static string GetNumericThenToString(this string dateValue)
        {
            var num = int.Parse(dateValue);
            return num.ToString("n0", CultureInfo.CreateSpecificCulture("tr-TR"));
        }
    }
}
