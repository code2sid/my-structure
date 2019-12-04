using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common2
{
    public static class Utilities
    {
        public static IEnumerable<T> GetEnumValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();

        public static T ParseEnum<T>(string enumValue, bool ignoreCase = false) => (T)Enum.Parse(typeof(T), enumValue, ignoreCase);
    }
}
