using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    /// Extensions for List
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Get list sub content by indexes
        /// </summary>
        /// <param name="source">source list</param>
        /// <param name="indexes">indexes to take</param>
        /// <typeparam name="TSource">type of list content</typeparam>
        /// <returns>sub list</returns>
        public static IEnumerable<TSource> ByIndexes<TSource>(this IList<TSource> source, params int[] indexes)
        {        
            if (indexes == null || indexes.Length == 0)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var i in indexes)
                {
                    if (i >= 0 && i < source.Count)
                        yield return source[i];
                }
            }
        }
    }
}