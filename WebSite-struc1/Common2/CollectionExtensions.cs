using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common2
{
    public static class CollectionExtensions
    {
        public static bool? AnyTrue(this IEnumerable<bool> source)
        {
            var sourceList = source.QuickToList();
            return sourceList.Any() ? sourceList.Any(_ => _) : (bool?)null;
        }

        public static IEnumerable<bool> AnyMany<T>(this IEnumerable<T> source, params Func<T, bool>[] predicates)
        {
            return predicates.Select(source.Any);
        }

        public static bool ContainsAny(this string source, params string[] items)
        {
            return items.Any(source.Contains);
        }

        public static string RemoveAll(this string original, char[] charactersToRemove)
        {
            return charactersToRemove.Aggregate(original, (modified, c) => modified.Replace(c.ToString(), ""));
        }

        public static void AddRange<TItem>(this ICollection<TItem> target, IEnumerable<TItem> source)
        {
            var lst = target as List<TItem>;
            if (lst != null)
            {
                lst.AddRange(source);
            }
            else
            {
                foreach (var item in source)
                {
                    target.Add(item);
                }
            }
        }

        public static async Task<IList<TOut>> ToListAsync<T, TOut>(this IEnumerable<T> source, Func<T, Task<TOut>> mapper)
        {
            var list = new List<TOut>();
            foreach (var item in source)
            {
                list.Add(await mapper(item).Caf());
            }

            return list;
        }

        public static async Task<IReadOnlyList<T>> QuickToList<T>(this Task<IEnumerable<T>> source)
        {
            var result = await source.Caf();
            return result.QuickToList();
        }

        public static async Task<IEnumerable<TOut>> Select<T, TOut>(this Task<IEnumerable<T>> source, Func<T, TOut> selector)
        {
            var result = await source.Caf();
            return result.Select(selector);
        }

        public static async Task<IEnumerable<T>> Where<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
        {
            return (await source.Caf()).Where(predicate);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T newItem)
        {
            foreach (var item in source)
            {
                yield return item;
            }

            yield return newItem;
        }

        public static bool TryGetValue<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key, out IEnumerable<TValue> values)
        {
            if (lookup.Contains(key))
            {
                values = lookup[key];
                return true;
            }

            values = null;
            return false;
        }

        public static IEnumerable<TValue> GetValueOrDefault<TKey, TValue>(this ILookup<TKey, TValue> lookup, TKey key)
        {
            return lookup.TryGetValue(key, out var values) ? values : new List<TValue>();
        }

        public static TItem Find<TItem, TKey>(this IEnumerable<TItem> collection, Func<TItem, TKey> keyGetter, TKey id, string itemDescription = "item")
        {
            var item = collection.SingleOrDefault(c => EqualityComparer<TKey>.Default.Equals(keyGetter(c), id));
            if (item == null)
            {
                throw new Exception($"Failed to find {itemDescription} with key: {id}");
            }

            return item;
        }

        public static async Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TValue>> valueSelector)
        {
            var dict = source.ToDictionary(keySelector, valueSelector);
            await Task.WhenAll(dict.Select(kvp => (Task)kvp.Key).Concat(dict.Select(kvp => kvp.Value)));
            return dict.ToDictionary(kvp => kvp.Key.Result, kvp => kvp.Value.Result);
        }

        public static async Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TValue>> valueSelector)
        {
            var dict = source.ToDictionary(keySelector, valueSelector);
            await Task.WhenAll(dict.Select(kvp => kvp.Value));
            return dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Result);
        }

        public static async Task<IReadOnlyDictionary<TKey, TValue>> ToReadOnlyDictionaryAsync<TKey, TValue>(this Task<IDictionary<TKey, TValue>> dictionaryTask)
        {
            var dictionary = await dictionaryTask.Caf();
            return dictionary.ToReadOnlyDictionary();
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary is Dictionary<TKey, TValue> dict
                ? dict
                : dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static T MinBy<T, TMin>(this IEnumerable<T> source, Func<T, TMin> minSelector) where TMin : IComparable<TMin>
        {
            return source.SelectBy((item1, item2) => minSelector(item1).CompareTo(minSelector(item2)) < 0);
        }

        public static T MaxBy<T, TMax>(this IEnumerable<T> source, Func<T, TMax> maxSelector) where TMax : IComparable<TMax>
        {
            return source.SelectBy((item1, item2) => maxSelector(item1).CompareTo(maxSelector(item2)) > 0);
        }

        private static T SelectBy<T>(this IEnumerable<T> source, Func<T, T, bool> shouldSelect)
        {
            var sourceList = source.QuickToList();
            if (!sourceList.Any())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            var value = sourceList.First();
            foreach (var item in sourceList)
            {
                if (shouldSelect(item, value))
                {
                    value = item;
                }
            }

            return value;
        }

        public static bool In<T>(this T item, params T[] collection) => collection.Contains(item);

        public static IReadOnlyList<T> QuickToList<T>(this IEnumerable<T> source) => source as IReadOnlyList<T> ?? source.ToList();

        public static IEnumerable<TValue> Flatten<TKey, TValue>(this ILookup<TKey, TValue> lookup)
        {
            return lookup.SelectMany(grouping => grouping.Select(item => item));
        }

        //public static IEnumerable<(TKey Key, TValue Value)> FlattenWithKey<TKey, TValue>(this ILookup<TKey, TValue> lookup)
        //{
        //    return lookup.SelectMany(grouping => grouping.Select(item => (grouping.Key, item)));
        //}

        public static async Task<bool> AllAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            foreach (var item in source)
            {
                if (!await predicate(item).Caf())
                {
                    return false;
                }
            }

            return true;
        }

        public static async Task<bool> AnyAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            foreach (var item in source)
            {
                if (await predicate(item).Caf())
                {
                    return true;
                }
            }

            return false;
        }

        public static Task<IEnumerable<T>> ExecuteInBatches<TParam, T>(this IEnumerable<TParam> allParams, int batchSize, Func<IEnumerable<TParam>, Task<IEnumerable<T>>> getter)
        {
            return allParams
                .QuickToList()
                .BatchIterate(batchSize)
                .Select((batch, idx) => new { batch, idx })
                .ToDictionaryAsync(b => b.idx, b => getter(b.batch))
                .PipeAsync(dict => dict
                    .OrderBy(kvp => kvp.Key)
                    .SelectMany(kvp => kvp.Value));
        }

        public static IEnumerable<IEnumerable<T>> BatchIterate<T>(this IReadOnlyList<T> source, int batchSize)
        {
            for (var i = 0; i < source.Count; i += batchSize)
            {
                yield return source.SkipTake(i, batchSize);
            }
        }

        public static IEnumerable<T> SkipTake<T>(this IReadOnlyList<T> source, int skipNumber, int takeNumber)
        {
            var lastIndex = Math.Min(skipNumber + takeNumber, source.Count);
            for (var i = skipNumber; i < lastIndex; i++)
            {
                yield return source[i];
            }
        }

        public static T GetSingleOrAlternate<T>(this IEnumerable<T> source, T alternative)
        {
            var sourceList = source.QuickToList();
            return sourceList.Count == 1 ? sourceList.Single() : alternative;
        }

        public static TOut GetSingleOrAlternate<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> selector, TOut alternative)
        {
            return source.Select(selector).GetSingleOrAlternate(alternative);
        }

        /// <summary>
        /// Gets the first date that occurs after the given date. If there are none,
        /// it returns the last date.
        /// </summary>
        /// <param name="dates">The dates to query.</param>
        /// <param name="date">The date to be compared with.</param>
        /// <returns>The first date that occurs after the given date, if any, otherwise the last date.</returns>
        public static DateTime GetFirstDateAfterDateOrLast(this IEnumerable<DateTime> dates, DateTime date)
        {
            var sortedDates = dates
                .OrderBy(d => d)
                .QuickToList();
            var datesAfterToday = sortedDates
                .Where(d => d >= date)
                .QuickToList();
            return datesAfterToday.Any() ?
                datesAfterToday.First() :
                sortedDates.Last();
        }
    }
}
