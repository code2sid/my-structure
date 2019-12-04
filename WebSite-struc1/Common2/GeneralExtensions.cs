using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Common2
{
    public static class GeneralExtensions
    {
        /// <summary>
        /// Converts the given string to a boolean. Returns null if value is not recognized.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>A boolean representation of the given string, or null if it is unrecognized.</returns>
        public static bool? ToBooleanOrNull(this string value)
        {
            if (value == null)
            {
                return null;
            }

            if (bool.TryParse(value, out bool result))
            {
                return result;
            }

            switch (value.ToLower())
            {
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts the given nullable boolean to "Yes", "No" or "Unknown".
        /// </summary>
        /// <param name="value">The nullable boolean to be converted.</param>
        /// <returns>"Unknown" if the value is null, "Yes" if the value is true, "No" otherwise.</returns>
        public static string ToYesNoUnknown(this bool? value)
        {
            return !value.HasValue ? "Unknown" : (value.Value ? "Yes" : "No");
        }

        /// <summary>
        /// Converts a nullable boolean to a boolean.
        /// </summary>
        /// <param name="value">The nullable boolean to be converted.</param>
        /// <returns>True if the nullable boolean is true, false otherwise.</returns>
        public static bool IsTrue(this bool? value)
        {
            return value.HasValue && value.Value;
        }

        public static ConfiguredTaskAwaitable Caf(this Task task)
        {
            return task.ConfigureAwait(false);
        }

        public static ConfiguredTaskAwaitable<T> Caf<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }

        public static string SafeToString(this DateTime? dateTime, string format, string defaultValue = "")
        {
            return dateTime?.ToString(format) ?? defaultValue;
        }

        public static void Pipe<TIn>(this TIn source, Action<TIn> action) => action(source);

        public static TOut Pipe<TIn, TOut>(this TIn source, Func<TIn, TOut> func) => func(source);
        public static async Task PipeAsync<TIn>(this Task<TIn> source, Action<TIn> action) => action(await source);

        public static async Task<TOut> PipeAsync<TIn, TOut>(this Task<TIn> source, Func<TIn, TOut> func) => func(await source);
    }
}
