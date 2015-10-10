using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static bool IsEmpty<T>(this List<T> source)
    {
        return source == null || !source.Any();
    }

    public static bool IsEmpty<T>(this T[] source)
    {
        return source == null || !source.Any();
    }

    public static bool IsEmpty<T>(this ICollection<T> source)
    {
        return source == null || !source.Any();
    }

    public static bool HasElements<T>(this List<T> source)
    {
        return !IsEmpty<T>(source);
    }

    public static bool HasElements<T>(this T[] source)
    {
        return !IsEmpty<T>(source);
    }

    public static bool HasElements<T>(this ICollection<T> source)
    {
        return !IsEmpty<T>(source);
    }
}
