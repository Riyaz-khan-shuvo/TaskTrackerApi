using TaskTracker.Shared.Exceptions;

namespace TaskTracker.Shared.Extensions
{
    public static class GuardClause
    {
        public static void ThrowExceptionIfNull(object parameter, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter), message);
        }
        public static async Task<List<T>> ThrowIfNullOrEmpty<T>(this Task<List<T>> task, string message = null, string paramName = null)
        {
            var result = await task;
            if (result == null || result.Count == 0)
            {
                throw new ArgumentException(message ?? "The collection cannot be null or empty.", paramName ?? nameof(result));
            }

            return result;
        }
        public static void ThrowNotFoundExceptionIfNull(object parameter, string message)
        {
            if (parameter == null)
                throw new PreRequisiteException(message);
        }

        public static void ThrowValidationException(string message)
        {
            throw new PreRequisiteException(message);
        }

        public static void ThrowDateRangeException(DateTime fromDate, DateTime toDate)
        {
            if (fromDate.Date > toDate.Date)
            {
                ThrowValidationException("To Date can't be earlier than From Date");
            }
        }
        public static void ThrowDateRangeException(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            ThrowDateRangeException(fromDate.DateTime.Date, toDate.DateTime.Date);
        }
        public static void ThrowMaxCharacterException(string value, int maxCount, string name = "")
        {
            if (value != null && value.Length > maxCount)
            {
                ThrowValidationException($"{name} can't be exceeded {maxCount} characters.");

            }
        }
    }
}
