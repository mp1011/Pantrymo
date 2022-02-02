using System.Globalization;

namespace Pantrymo.Domain.Extensions
{
    public static class DateExtensions
    {
        public static string ToUrlDateString(this DateTime date) => date.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture);

        public static TimeSpan TimeSince(this DateTime date) => DateTime.Now - date;
    }
}
