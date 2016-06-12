namespace Util
{
    /// <summary>
    ///     Utility class for working with Gravatar links. See also the Gravtar <a href="https://en.gravatar.com/site/implement/hash/">documentation.</a>
    /// </summary>
    public static class Gravatar
    {
        /// <summary>
        ///     Generate the hash for an email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The hash for the email.</returns>
        public static string GetHash(string email) => email.Trim().ToLower().CalculateMD5();

        /// <summary>
        ///     Generate the full url to an avatar.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="defaultValue">The default value parameter to append to the url.</param>
        /// <returns>The full url for the email.</returns>
        public static string GetUrl(string email, string defaultValue = null)
        {
            string url = $"https://www.gravatar.com/avatar/{GetHash(email)}";
            if (!string.IsNullOrEmpty(defaultValue))
            {
                url += $"?d={defaultValue}";
            }
            return url;
        }
    }
}