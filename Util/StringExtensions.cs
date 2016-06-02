using System;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string value)
            => source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;

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