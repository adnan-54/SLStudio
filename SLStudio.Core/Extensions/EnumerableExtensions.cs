using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLStudio.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, IEnumerable<TSource>> flatChildren)
        {
            var flattenedList = source.Where(predicate);

            foreach (TSource element in source)
                flattenedList = flattenedList.Concat(flatChildren(element).Flatten(predicate, flatChildren));

            return flattenedList;
        }
    }
}