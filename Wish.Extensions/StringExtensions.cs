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
    }
}
