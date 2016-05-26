using System;

namespace Util
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string value)
            => source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;
    }
}