using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using TaskTracker.Shared.Models;
using static TaskTracker.Shared.Configurations.AppBusinessCategories;

namespace TaskTracker.Shared.Extensions
{
    public static class UtilitiesExtension
    {
        public static string GetYesOrNoValue(this bool boolValue) => boolValue ? "Yes" : "No";
        public static string GetYesOrNoValue(this bool? boolValue) => boolValue == true ? "Yes" : "No";
        public static string GetActiveOrInactiveValue(this bool boolValue) => boolValue ? "Active" : "Inactive";

        public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;
        public static bool IsEmpty(this Guid? guid) => guid == Guid.Empty || guid == null;

        public static string GetDateStamp(this DateTime date) => date.ToString("yyyyMMdd");
        public static string GetLocalDate(this DateTime date) => date.ToString("dd/MM/yyyy");
        public static string GetTimeStamp(this DateTime time) => time.ToString("HHmmss");
        public static string GetDateTimeStamp(this DateTime dateTime) => dateTime.ToString("yyyyMMddHHmmss");
        public static string GetDateStamp(this DateTimeOffset date) => date.ToString("yyyyMMdd");
        public static string GetLocalDate(this DateTimeOffset date) => date.ToString("dd/MM/yyyy");
        public static string GetTimeStamp(this DateTimeOffset time) => time.ToString("HHmmss");
        public static string GetDateTimeStamp(this DateTimeOffset dateTime) => dateTime.ToString("yyyyMMddHHmmss");

        public static string GetLocalDate(this DateTime? date)
        {
            if (date is null) return string.Empty;
            return date.Value.ToString("dd/MM/yyyy");
        }

        public static void FixNullableDateTime(this DateTime? date)
        {

            date = date.HasValue ? date.Value.AddHours(6) : date;
        }
        public static void FixDateTime(this DateTime date)
        {

            date = date.AddHours(6);
        }

        public static string ToAmPmString(this TimeSpan time)
        {
            return DateTime.Today.Add(time).ToString("hh:mm tt");
        }
        public static List<SelectModel> GetBusinessCategorySelectList(Type businessCategoryType)
        {
            List<SelectModel> businessCategoryList = new List<SelectModel>();

            foreach (FieldInfo field in businessCategoryType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                int value = (int)field.GetValue(null);
                string name = field.Name;

                SelectModel item = new SelectModel
                {
                    Id = value,
                    Name = GetDisplayName(field) ?? Regex.Replace(name, @"(\p{Lu})", " $1").TrimStart()
                };

                businessCategoryList.Add(item);
            }
            return businessCategoryList;
        }

        public static double GetYearsDifference(DateTime startDate, DateTime endDate)
        {
            // Calculate the difference in days between the two dates
            TimeSpan timeSpan = endDate - startDate;

            // Calculate the difference in years including fraction
            double years = timeSpan.TotalDays / 365.25;

            return years;
        }

        public static List<SelectModel> GetCustomCategorySelectList(Type customCategoryType)
        {
            List<SelectModel> customCategoryList = new List<SelectModel>();

            foreach (FieldInfo field in customCategoryType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                Guid value = (Guid)field.GetValue(null);

                SelectModel item = new SelectModel
                {
                    Id = value,
                    Name = field.Name
                };

                customCategoryList.Add(item);
            }
            return customCategoryList;
        }

        //public static string GetAttendanceParticularName(Guid attendanceMapTypeId)
        //{
        //    if (attendanceMapTypeId == AttendanceStatusCategoriesMapType.OnTime
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.OnTime
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.LateIn)
        //    {
        //        return AttendancePerticularName.OnDuty;
        //    }

        //    if (attendanceMapTypeId == AttendanceStatusCategoriesMapType.Absent)
        //    {
        //        return AttendancePerticularName.Absent;
        //    }

        //    if (attendanceMapTypeId == AttendanceStatusCategoriesMapType.AnnualLeave
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.SickLeave
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.CasualLeave
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.MaternityLeave
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.LeaveWithoutPay
        //        || attendanceMapTypeId == AttendanceStatusCategoriesMapType.Compensatory)
        //    {
        //        return AttendancePerticularName.OnLeave;
        //    }

        //    if (attendanceMapTypeId == AttendanceStatusCategoriesMapType.EarlyLeave)
        //    {
        //        return AttendancePerticularName.EarlyExit;
        //    }

        //    if (attendanceMapTypeId == AttendanceStatusCategoriesMapType.Weekend)
        //    {
        //        return AttendancePerticularName.Weekend;
        //    }

        //    if (attendanceMapTypeId == AttendanceStatusCategoriesMapType.Holiday)
        //    {
        //        return AttendancePerticularName.Holiday;
        //    }

        //    return "";
        //}


        public static T GetCastingValue<T>(object obj, Type type)
        {
            if (obj == null || type == null)
            {
                return default(T);
            }

            if (obj is T)
            {
                return (T)obj;
            }

            if (obj is string stringValue)
            {
                if (typeof(T) == typeof(DateTime))
                {
                    if (DateTime.TryParse(stringValue, out var dateTimeValue))
                    {
                        return (T)(object)dateTimeValue;
                    }
                }
                // Handle conversion for numeric types
                else if (typeof(T).IsNumericType())
                {
                    // You can assume all numeric types are handled the same way
                    return (T)Convert.ChangeType(stringValue, typeof(T));
                }
            }

            // You can add more specific type conversions as needed

            // If no specific conversion is found, return default value for type T
            return default(T);
        }
        public static string FormatDate(object datetime)
        {
            if (datetime is null)
            {
                return null;
            }
            if (datetime is DateTime)
            {
                var castingDate = (DateTime)datetime;
                return $"{castingDate.Day}{GetDaySuffix(castingDate.Day)} {castingDate.ToString("MMMM")}, {castingDate.Year}";
            }
            else if (datetime is DateTimeOffset)
            {
                var castingDate = (DateTimeOffset)datetime;
                return $"{castingDate.Day}{GetDaySuffix(castingDate.Day)} {castingDate.ToString("MMMM")}, {castingDate.Year}";
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<DateTimeOffset> GetTotalDaysFromRangeDate(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            List<DateTimeOffset> daysList = new List<DateTimeOffset>();

            daysList.Add(fromDate);

            TimeSpan duration = toDate - fromDate;

            for (int i = 1; i <= duration.Days; i++)
            {
                DateTimeOffset nextDay = fromDate.AddDays(i);
                daysList.Add(nextDay);
            }

            return daysList;
        }

        public static int GetWeekdayId(DateTimeOffset date)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return WeekDay.Sunday;
                case DayOfWeek.Monday:
                    return WeekDay.Monday;
                case DayOfWeek.Tuesday:
                    return WeekDay.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekDay.Wednessday;
                case DayOfWeek.Thursday:
                    return WeekDay.Thursday;
                case DayOfWeek.Friday:
                    return WeekDay.Friday;
                case DayOfWeek.Saturday:
                    return WeekDay.Saturday;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid day of week.");
            }
        }
        public static int GetWeekdayId(DateTime date)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return WeekDay.Sunday;
                case DayOfWeek.Monday:
                    return WeekDay.Monday;
                case DayOfWeek.Tuesday:
                    return WeekDay.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekDay.Wednessday;
                case DayOfWeek.Thursday:
                    return WeekDay.Thursday;
                case DayOfWeek.Friday:
                    return WeekDay.Friday;
                case DayOfWeek.Saturday:
                    return WeekDay.Saturday;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid day of week.");
            }
        }


        private static string GetDisplayName(FieldInfo field)
        {
            DisplayAttribute displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

            return displayAttribute?.Name;
        }
        private static bool IsNumericType(this Type type)
        {
            if (type == null)
                return false;

            return type == typeof(byte) ||
                   type == typeof(sbyte) ||
                   type == typeof(short) ||
                   type == typeof(ushort) ||
                   type == typeof(int) ||
                   type == typeof(uint) ||
                   type == typeof(long) ||
                   type == typeof(ulong) ||
                   type == typeof(float) ||
                   type == typeof(double) ||
                   type == typeof(decimal);
        }

        private static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        private static string[] units = {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
            "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };

        private static string[] tens = {
            "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };

        private static string ConvertToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                //return "minus " + ConvertToWords(Math.Abs(number));
                return " " + ConvertToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000) > 0)
            {
                words += ConvertToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertToWords(number / 100) + " hundred ";
                number %= 100;

                if (number > 0)
                    words += "and ";
            }

            if (number > 0)
            {
                if (number < 20)
                    words += units[number];
                else
                {
                    words += tens[number / 10];
                    if ((number % 10) > 0)
                        words += " " + units[number % 10];
                }
            }

            return words.Trim();
        }

        public static string FormatTakaAmount(decimal amount, string msg = null)
        {
            // Split the amount into whole number and decimal parts
            int wholePart = (int)Math.Floor(amount);
            int decimalPart = (int)Math.Round((amount - wholePart) * 100);

            string wholeWords = ConvertToWords(wholePart);

            // Format the result based on whether there are decimal values
            string result;
            if (decimalPart > 0)
            {
                string decimalWords = ConvertToWords(decimalPart);
                result = $"{(msg != null ? msg : "In word,")} taka {char.ToUpper(wholeWords[0]) + wholeWords.Substring(1)} and {decimalWords} paisa only.";
            }
            else
            {
                result = $"{(msg != null ? msg : "In word,")} taka {char.ToUpper(wholeWords[0]) + wholeWords.Substring(1)} only.";
            }

            return result;
        }

        public static (int Years, int Months, int Days) CalculateDateDuration(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be after start date");
            }

            int years = endDate.Year - startDate.Year;
            int months = endDate.Month - startDate.Month;
            int days = endDate.Day - startDate.Day;

            // Adjust for negative days
            if (days < 0)
            {
                // Get days in the previous month
                months--;
                days += DateTime.DaysInMonth(startDate.Year, startDate.Month);
            }

            // Adjust for negative months
            if (months < 0)
            {
                years--;
                months += 12;
            }

            return (years, months, days);
        }

        public static (int Years, int Months) CalculateCompleteMonthsOnly(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var (years, months, _) = CalculateDateDuration(startDate, endDate);
            return (years, months);
        }

        public static string FormatDateDuration(DateTimeOffset startDate, DateTimeOffset endDate, bool completeMonthsOnly = true)
        {
            if (completeMonthsOnly)
            {
                var (years, months) = CalculateCompleteMonthsOnly(startDate, endDate);
                int totalMonths = (years * 12) + months;
                return $"{totalMonths} {(totalMonths == 1 ? "month" : "months")}";
            }
            else
            {
                var (years, months, days) = CalculateDateDuration(startDate, endDate);
                int totalMonths = (years * 12) + months;

                //if (days > 0)
                //{
                //    return $"{totalMonths} {(totalMonths == 1 ? "month" : "months")}, {days} {(days == 1 ? "day" : "days")}";
                //}
                //else
                //{
                //    return $"{totalMonths} {(totalMonths == 1 ? "month" : "months")}";
                //}

                string result = "";
                if (years > 0)
                {
                    result += $"{years} {(years == 1 ? "year" : "years")} ";
                }

                if (months > 0 || years > 0)
                {
                    result += $"{months} {(months == 1 ? "month" : "months")} ";
                }

                result += $"{days} {(days == 1 ? "day" : "days")}";

                return result.Trim();
            }
        }

    }

}
