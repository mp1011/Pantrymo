namespace Pantrymo.Application.Extensions
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
    }
}
