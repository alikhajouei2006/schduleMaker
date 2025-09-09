using System.Globalization;

namespace Utils
{
    class Helper
    {
        public static TimeSpan ParseTime(string text)
        {
            string time = text.Length == 4 ? "0" + text : text;
            return TimeSpan.ParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture);
        }
    }
}