using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static bool IsEmpty<T>(this T[] source)
    {
        return source == null || !source.Any();
    }

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return source == null || !source.Any();
    }

    public static bool HasElements<T>(this T[] source)
    {
        return !IsEmpty(source);
    }

    public static bool HasElements<T>(this IEnumerable<T> source)
    {
        return !IsEmpty(source);
    }
}
