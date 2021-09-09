using System;

namespace messanger.Client.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToAgoString(this DateTime dateTime)
        {
            var diff = DateTime.UtcNow - dateTime;

            var sign = string.Empty;
            if (diff.Minutes < 0)
            {
                diff = diff.Negate();
                sign = "-";
            }

            string rsStr;
            var years = diff.Days / 365;
            if ((rsStr = ConstructStr(years, "year")) is not null)
                return $"{sign}{rsStr}";

            if ((rsStr = ConstructStr(diff.Days, "day")) is not null)
                return $"{sign}{rsStr}";

            if ((rsStr = ConstructStr(diff.Hours, "hour")) is not null)
                return $"{sign}{rsStr}";

            if ((rsStr = ConstructStr(diff.Minutes, "min")) is not null)
                return $"{sign}{rsStr}";

            return $"{sign}1 min";
        }

        private static string ConstructStr(int value, string postfix)
        {
            if (value <= 0)
                return null;

            var parsedStr = $"{value} {postfix}";
            if (value > 1)
                parsedStr += "s";

            return parsedStr;
        }
    }
}