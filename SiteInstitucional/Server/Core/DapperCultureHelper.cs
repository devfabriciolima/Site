using System.Globalization;

namespace SiteInstitucional.Server.Core
{
    public static class DapperCultureHelper
    {
        private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("pt-BR");

        public static string ToLocalizedProcedure(this string name) => Equals(CultureInfo.CurrentUICulture, DefaultCulture)
            ? name
            : $"{name}_{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}";
    }
}
