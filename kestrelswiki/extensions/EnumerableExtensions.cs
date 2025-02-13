using System.Collections.Generic;

namespace kestrelswiki.extensions;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T t in enumerable) action(t);
    }
}