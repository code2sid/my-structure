using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common.Utility
{
    public static class StringExtensions
    {
        public static object ConvertTo(this string src, Type valueType, CultureInfo culture)
        {
            try
            {
                src.CheckNotNull<string>(nameof(src));
                if (valueType == typeof(Uri))
                    return (object)new Uri(src);
                if (valueType == typeof(TimeSpan))
                    return (object)TimeSpan.Parse(src, (IFormatProvider)culture);
                return Convert.ChangeType((object)src, valueType, (IFormatProvider)culture);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Cannot convert [{0}] to type [{1}]", (object)src, (object)valueType), ex);
            }
        }

        public static object ConvertTo(this string src, Type valueType)
        {
            return src.ConvertTo(valueType, CultureInfo.InvariantCulture);
        }

        public static TValue ConvertTo<TValue>(this string src, CultureInfo culture)
        {
            return (TValue)src.ConvertTo(typeof(TValue), culture);
        }

        public static TValue ConvertTo<TValue>(this string src)
        {
            return src.ConvertTo<TValue>(CultureInfo.InvariantCulture);
        }

        public static string FormatWith(this string template, params object[] args)
        {
            return string.Format((IFormatProvider)CultureInfo.InvariantCulture, template, args);
        }
    }
}
