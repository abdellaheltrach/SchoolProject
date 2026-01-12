using System.Globalization;

namespace School.Core.Helpers
{
    public static class LocalizationHelper
    {
        public static string? GetLocalizedName(string? nameAr, string? nameEn)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            return culture.TwoLetterISOLanguageName.ToLower().Equals("ar")
                ? nameAr ?? nameEn
                : nameEn ?? nameAr;
        }
    }
}
