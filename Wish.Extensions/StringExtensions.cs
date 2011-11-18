using System;
using System.Text;

namespace Wish.Extensions
{
    public static class StringExtensions
    {
        public static string Surround(this string source, string surround)
        {
            var sb = new StringBuilder();
            sb.Append(surround);
            sb.Append(source);
            sb.Append(surround);
            return sb.ToString();
        }

        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return source.IndexOf(toCheck, comparison) >= 0;
        }
    }
}
