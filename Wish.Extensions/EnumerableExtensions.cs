using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wish.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        /// <summary>
        /// Split an IEnumerable at the desired index
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int index)
        {
            return source.Where((x,i) => i % index == 0).Select((x,i) => source.Skip(i * index).Take(index));
        }
    }
}