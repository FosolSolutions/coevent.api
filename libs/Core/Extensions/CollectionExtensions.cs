using System.Collections.Generic;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// CollectionExtensions static class, provides extension methods for Collection and ICollection.
    /// </summary>
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection is IList<T>)
            {
                ((IList<T>)collection).AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }
    }
}
