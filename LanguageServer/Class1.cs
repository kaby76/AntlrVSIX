using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer
{
    public static class Class1
    {
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source) => source.ToHashSet(null);

        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Don't pre-allocate based on knowledge of size as potentially many elements will be dropped.
            return new HashSet<TSource>(source, comparer);
        }
    }
}
