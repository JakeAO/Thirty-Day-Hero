using System.Collections.Generic;

namespace Core.Etc
{
    public static class CollectionExtensions
    {
        public static Dictionary<T, U> ToMutable<T, U>(this IReadOnlyDictionary<T, U> val)
        {
            Dictionary<T, U> newDict = new Dictionary<T, U>(val.Count);
            foreach (var keyValuePair in val)
            {
                newDict[keyValuePair.Key] = keyValuePair.Value;
            }

            return newDict;
        }

        public static SortedDictionary<T, U> ToSorted<T, U>(this IReadOnlyDictionary<T, U> val)
        {
            SortedDictionary<T, U> newDict = new SortedDictionary<T, U>();
            foreach (var keyValuePair in val)
            {
                newDict[keyValuePair.Key] = keyValuePair.Value;
            }

            return newDict;
        }
    }
}