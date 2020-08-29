using System;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// EnumerableExtensions static class, provides extensions methods for Enumerable and IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Iterates through the enumerable and performs the action on each item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Array.ForEach(enumerable.ToArray(), action);
        }
    }
}
