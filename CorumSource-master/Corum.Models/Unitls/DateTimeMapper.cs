using System;
using System.Dynamic;


namespace Corum.Common
{    
    public static class DateTimeConvertClass
    {
        //"yyyy-MM-ddTHH:mm:sszzzzz";
        public static DateTime getDateTime(string momentjsUTC)
        {
            int year = Convert.ToInt32(momentjsUTC.Substring(0, 4));
            int month = Convert.ToInt32(momentjsUTC.Substring(5, 2));
            int day = Convert.ToInt32(momentjsUTC.Substring(8, 2));
            
            return new DateTime(year, month, day, 0, 0, 0);
        }

        //"dd-MM-yyyyTHH:mm:sszzzzz";
        public static DateTime getDateTime2(string momentjsUTC)
        {
            int day = Convert.ToInt32(momentjsUTC.Substring(0, 2));
            int month = Convert.ToInt32(momentjsUTC.Substring(3, 2));
            int year = Convert.ToInt32(momentjsUTC.Substring(6, 4));
                        
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static int getHours(string momentjsUTC)
        {
            return Convert.ToInt32(momentjsUTC.Substring(11, 2));
        }

        public static int getMinutes(string momentjsUTC)
        {
            return Convert.ToInt32(momentjsUTC.Substring(14, 2));
        }

        public static DateTime getDate(string momentjsUTC)
        {
            int year = Convert.ToInt32(momentjsUTC.Substring(0, 4));
            int month = Convert.ToInt32(momentjsUTC.Substring(5, 2));
            int day = Convert.ToInt32(momentjsUTC.Substring(8, 2));

            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static string getString(DateTime Value)
        {
            string year = Value.Year.ToString();
            string month = Value.Month.ToString();
            if (month.Length == 1) { month = "0" + month; }
            string day = Value.Day.ToString();
            if (day.Length == 1) { day = "0" + day; }
            string hours = Value.Hour.ToString();
            if (hours.Length == 1) { hours = "0" + hours; }
            string minutes = Value.Minute.ToString();
            if (minutes.Length == 1) { minutes = "0" + minutes; }
            return string.Concat(year,"-",month,"-",day,"T",hours,":",minutes,":00");
        }

        public static string getTimeFormat(int Value)
        {
            TimeSpan t = TimeSpan.FromMilliseconds((double)Value);
            return string.Format("{0:D2}:{1:D2}",
                                   (int)t.TotalHours,
                                    t.Minutes);
        }

        public static string getHoursFormat(int Value)
        {
            TimeSpan result = TimeSpan.FromHours(Value);
            int TotalDays = result.Days;
            int TotalHours = result.Hours;
            return string.Format("{0:D2}:{1:D2}",
                                   TotalDays,
                                   TotalHours);            

        }

        public static int convertHoursToInt(string modelTime)
        {
            string[] DaysHours = modelTime.Split(':');
            var DaysStr = DaysHours[0];
            var HoursStr = "0";
            if (DaysHours.Length > 1)
            {
                HoursStr = DaysHours[1];
            }
            var HoursInt = Convert.ToInt32(HoursStr);
            var DaysInt = Convert.ToInt32(DaysStr);
            decimal modelHoursInt = (HoursInt + DaysInt * 24);

            return (int)modelHoursInt;
        }

        public static int convertTimeToInt(string modelTime)
        {
            string[] HourMin = modelTime.Split(':');
            var HoursStr = HourMin[0];
            var MinutesStr = "0";
            if (HourMin.Length > 1)
            {
                MinutesStr = HourMin[1];
            }
            var MinutesInt = Convert.ToInt32(MinutesStr);
            var HoursInt = Convert.ToInt32(HoursStr);
            decimal modelTimeInt = (MinutesInt + HoursInt * 60) * 60000;

            return (int)modelTimeInt;
        }
    }
}
