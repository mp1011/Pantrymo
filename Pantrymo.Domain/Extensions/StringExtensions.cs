namespace Pantrymo.Domain.Extensions
{
    public static class StringExtensions
    {
        public static int TryParseInt(this string str)
        {
            int val;
            if (int.TryParse(str, out val))
                return val;
            else
                return 0;
        }

        public static string[] FromCSV(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new string[] { };
            else
                return str.Split(',');
        }
    }
}
