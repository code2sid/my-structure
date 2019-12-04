using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Client.Common.Utility
{
    public static class ValidationExtensions
    {
        public static void ValidateUniqueItemsCollection<TKey, TItem>(this ICollection<TItem> src, Func<TItem, TKey> keyExtractor, string errorMsgPrefix, ICollection<ValidationResult> aggregator, IEqualityComparer<TKey> comparer = null) where TKey : IEquatable<TKey>
        {
            object obj1 = (object)src ?? (object)new TItem[0];
            TKey[] array = ((IEnumerable<TItem>)obj1).Select<TItem, TKey>(keyExtractor).FindDuplicates<TKey>(comparer).ToArray<TKey>();
            if (((IEnumerable<TKey>)array).Any<TKey>())
                aggregator.Add(new ValidationResult(string.Format("{0}: {1}", (object)errorMsgPrefix, (object)string.Join(", ", ((IEnumerable<TKey>)array).Select<TKey, string>((Func<TKey, string>)(c => string.Format("[{0}]", (object)c)))))));
            foreach (TItem obj2 in (IEnumerable<TItem>)obj1)
                Validator.TryValidateObject((object)obj2, new ValidationContext((object)obj2), aggregator, true);
        }

        public static TObject CheckNotNull<TObject>(this TObject target, string paramName) where TObject : class
        {
            if ((object)target == null)
                throw new ArgumentNullException(paramName);
            return target;
        }

        public static string CheckNotEmpty(this string input, string paramName)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Param [{0}] cannot be null or empty".FormatWith((object)paramName));
            return input;
        }
    }
}
