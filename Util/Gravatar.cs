namespace Util
{
    public static class Gravatar
    {
        public static string GetHash(string email) => email.Trim().ToLower().CalculateMD5();

        public static string GetUrl(string email, string defaultValue = null)
        {
            string url = $"https://www.gravatar.com/avatar/{GetHash(email)}";
            if (defaultValue != null)
            {
                url += $"?d={defaultValue}";
            }
            return url;
        }
    }
}