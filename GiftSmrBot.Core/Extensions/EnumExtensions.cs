using GiftSmrBot.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GiftSmrBot.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            DisplayAttribute displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static string DescribeEnum<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            StringBuilder description = new StringBuilder();
            foreach (T category in values)
            {
                var enumDescription = category.DescribeEnum();
                description.AppendLine($"{enumDescription.displayName} - {enumDescription.value}");
            }
            return description.ToString().Trim();
        }

        public static (string displayName, int value) DescribeEnum(this Enum enumValue)
        {
            DisplayAttribute displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
            int enumIntValue = ((IConvertible)enumValue).ToInt32(null);
            return (displayAttribute.Name, enumIntValue);
        }

        public static int GetLower(this PriceCategories priceCategory)
        {
            return priceCategory.GetAttribute<PriceCategoryValueAttribute>().Lower;
        }

        public static int GetUpper(this PriceCategories priceCategory)
        {
            return priceCategory.GetAttribute<PriceCategoryValueAttribute>().Upper;
        }
    }
}
