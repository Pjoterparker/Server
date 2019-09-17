using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PjoterParker.Core.Extensions
{
    public static class StringExtensions
    {
        public static T To<T>(this string source)
        {
            return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T? ToEnum<T>(this string source) where T : struct
        {
            if (Enum.TryParse(source, out T value))
            {
                return value;
            }

            return null;
        }

        public static T? ToN<T>(this string source) where T : struct
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return (T)Convert.ChangeType(source, typeof(T), CultureInfo.InvariantCulture);
            }

            return null;
        }
    }
}
