using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ametrin.Utils
{
    public static partial class StringExtensions
    {
        public static string ToNumberFriendly(this string input, NumberFormatInfo formatInfo = null)
        {
            return input.ToNumberFriendly((formatInfo ?? CultureInfo.CurrentCulture.NumberFormat).NumberDecimalSeparator);
        }

        public static string ToNumberFriendly(this string input, string decimalSeparator)
        {
            input = Regex.Replace(input, "[.,]", decimalSeparator);
            return Regex.Replace(input, "[^0-9,]", "");
        }

        public static string ToIntFriendly(this string input)
        {
            return Regex.Replace(input, "\\D", "");
        }

        public static bool StartsWith(this string str, ReadOnlySpan<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }

            return str.AsSpan(0, value.Length).SequenceEqual(value);
        }
    }
}