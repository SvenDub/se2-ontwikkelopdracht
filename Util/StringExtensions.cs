using System;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    /// <summary>
    ///     Extensions class that contains various string manipulation methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Checks if a substring occurs in the string, ignoring case.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The substring to search.</param>
        /// <returns>Whether the substring occurs in the string.</returns>
        public static bool ContainsIgnoreCase(this string source, string value)
            => source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;

        /// <summary>
        ///     Calculate the MD5 hash for the string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>The lower case MD5 hash.</returns>
        public static string CalculateMD5(this string source)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(source);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}