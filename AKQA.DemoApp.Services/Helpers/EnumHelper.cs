using System;
using System.ComponentModel;

namespace AKQA.DemoApp.Services.Helpers
{
    /// <summary>
    /// Enum helper methods
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Get description from the enum value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Eum value</param>
        /// <returns></returns>
        public static string GetDescription<T>(this T value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }                
        }

        /// <summary>
        /// Parse text value to enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">text value</param>
        /// <returns></returns>
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
