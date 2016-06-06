namespace Util
{
    public static class Gravatar
    {
        public static string GetHash(string email) => email.Trim().ToLower().CalculateMD5();

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