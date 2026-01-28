namespace School.Core.Helpers
{
    public static class SignInHelper
    {
        public static bool IsEmail(string identifier)
        {
            return System.Net.Mail.MailAddress.TryCreate(identifier, out _);
        }
    }
}
