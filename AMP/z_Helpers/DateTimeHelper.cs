

using System;

namespace AMP.Helpers
{
    public class DateTimeHelper
    {
        public static string GetDurationString(int totalDays, string periodString = "")
        {
            int remainingDays = totalDays;
            if (totalDays >= 365)
            {
                var value = GetYears(totalDays, out remainingDays);
                periodString += value + " Year(s) ";
                if (remainingDays > 0)
                    periodString = GetDurationString(remainingDays, periodString);
                else
                    return periodString;
            }
            else if (totalDays >= 30)
            {
                var value = GetMonths(totalDays, out remainingDays);
                periodString += value + " Month(s) ";
                if (remainingDays > 0)
                    periodString = GetDurationString(remainingDays, periodString);
                else
                    return periodString;
            }
            else if (totalDays >= 7)
            {
                var value = GetWeeks(totalDays, out remainingDays);
                periodString += value + " Week(s) ";
                if (remainingDays > 0)
                    periodString = GetDurationString(remainingDays, periodString);
                else
                    return periodString;
            }
            else
            {
                periodString += totalDays + " Day(s) ";
                return periodString;
            }
            return periodString;

        }

        public static int GetYears(int totalDays, out int remainingDays)
        {
            var periodValue = totalDays / 365;
            if (periodValue > 0)
            {
                remainingDays = totalDays - (periodValue * 365);
                remainingDays = remainingDays < 0 ? 0 : remainingDays;
                return periodValue;
            }
            else
            {
                remainingDays = totalDays;
                periodValue = 0;
                return periodValue;
            }
        }

        public static int GetMonths(int totalDays, out int remainingDays)
        {
            var periodValue = totalDays / 30;
            if (periodValue > 0)
            {
                remainingDays = totalDays - (periodValue * 30);
                remainingDays = remainingDays < 0 ? 0 : remainingDays;
                return periodValue;
            }
            else
            {
                remainingDays = totalDays;
                periodValue = 0;
                return periodValue;
            }
        }

        public static int GetWeeks(int totalDays, out int remainingDays)
        {
            var periodValue = totalDays / 7;
            if (periodValue > 0)
            {
                remainingDays = totalDays - (periodValue * 7);
                remainingDays = remainingDays < 0 ? 0 : remainingDays;
                return periodValue;
            }
            else
            {
                remainingDays = totalDays;
                periodValue = 0;
                return periodValue;
            }
        }


        public static string ToDateString(DateTime? date)
        {
            string str = "";
            if (date.HasValue)
                str = date.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return str;
        }

        public static DateTime? ToDateObject(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime date;
                if (DateTime.TryParseExact(str, new[] { "dd MMM yyyy", "dd MMMM yyyy", "dd/MM/yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static object ToDateDbObject(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime date;
                if (DateTime.TryParseExact(str, new[] { "dd MMM yyyy", "dd MMMM yyyy", "dd/MM/yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            else
            {
                return DBNull.Value;
            }
        }
    }
}