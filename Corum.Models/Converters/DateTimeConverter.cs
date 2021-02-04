using System;

namespace Barnivann.Models
{
    public static class DateTimeConvertClass
    {
        //"yyyy-MM-ddTHH:mm:sszzzzz";
        public static DateTime getDateTime(string momentjsUTC)
        {
            int year = Convert.ToInt32(momentjsUTC.Substring(0, 4));
            int month = Convert.ToInt32(momentjsUTC.Substring(5, 2));
            int day = Convert.ToInt32(momentjsUTC.Substring(8, 2));
            //int hour = Convert.ToInt32(momentjsUTC.Substring(11, 2));
            //int minute = Convert.ToInt32(momentjsUTC.Substring(14, 2));
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static TimeSpan getTime(string momentjsUTC)
        {            
            int hour = Convert.ToInt32(momentjsUTC.Substring(11, 2));
            int minute = Convert.ToInt32(momentjsUTC.Substring(14, 2));
            return new TimeSpan(hour, minute, 0);
        }

        public static DateTime getDate(string momentjsUTC)
        {
            int year = Convert.ToInt32(momentjsUTC.Substring(0, 4));
            int month = Convert.ToInt32(momentjsUTC.Substring(5, 2));
            int day = Convert.ToInt32(momentjsUTC.Substring(8, 2));

            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static string GetString(DateTime Value)
        {
            string year = Value.Year.ToString();

            string month = Value.Month.ToString();
            if (month.Length == 1) { month = "0" + month; }

            string day = Value.Day.ToString();
            if (day.Length == 1) { day = "0" + day; }

            string hour = Value.Hour.ToString();
            if (hour.Length == 1) { hour = "0" + hour; }

            string minute = Value.Minute.ToString();
            if (minute.Length == 1) { minute = "0" + minute; }

            return string.Concat(year,"-",month,"-",day,"T",hour,":",minute,":00");
        }
    }
}
