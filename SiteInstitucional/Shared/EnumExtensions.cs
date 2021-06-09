using System;
using System.ComponentModel;
using System.Linq;

namespace SiteInstitucional.Shared
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var enumField = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttr =
                enumField.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttr?.Description ?? enumValue.ToString();
        }
    }
}
