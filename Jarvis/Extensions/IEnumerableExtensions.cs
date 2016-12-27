using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Jarvis.Filtering;

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

    /// <summary>
    /// Taken from https://github.com/elastic/elasticsearch-net-example/blob/master/src/NuSearch.Domain/Extensions/PartitionExtension.cs
    /// </summary>
    /// <typeparam name="T">Generic IEnumerable type</typeparam>
    /// <param name="source">source</param>
    /// <param name="size">each partition size</param>
    /// <returns>a list of partitions</returns>
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source", "source cannot be null.");
        }

        T[] array = null;
        int count = 0;

        foreach (T item in source)
        {
            if (array == null)
            {
                array = new T[size];
            }

            array[count] = item;

            count++;

            if (count == size)
            {
                yield return new ReadOnlyCollection<T>(array);
                array = null;
                count = 0;
            }
        }

        if (array != null)
        {
            Array.Resize(ref array, count);
            yield return new ReadOnlyCollection<T>(array);
        }
    }

    public static IEnumerable<T> Select<T>(this IEnumerable<T> source, string fields)
    {
        // Check source.
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        // Check if we have any parameters.
        if (String.IsNullOrEmpty(fields))
        {
            return source;
        }

        // Create input parameter "o".
        ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "o");

        // Create new statement "new Data()".
        NewExpression newExpression = Expression.New(typeof(T));

        // Get a list of assignable members.
        IEnumerable<string> assignableMembers = fields
            .Split(',')
            .Where(o => typeof(T).GetProperty(o, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
            .Select(o => o.Trim());

        // Check if we have any assignable member.
        if (assignableMembers.IsEmpty())
        {
            throw new ArgumentException($"None of the given names ({fields}) is assignable to a property on type: {typeof(T).FullName}", nameof(fields));
        }

        // Create initializers.
        IEnumerable<MemberAssignment> memberAssignments = assignableMembers
            .Select(o =>
            {
                // Property "Field1".
                PropertyInfo propertyInfo = typeof(T).GetProperty(o, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // Original value "o.Field1".
                MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);

                // Set value "Field1 = o.Field1".
                return Expression.Bind(propertyInfo, memberExpression);
            });

        // Initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }".
        MemberInitExpression memberInitExpression = Expression.MemberInit(newExpression, memberAssignments);

        // Expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }".
        Expression<Func<T, T>> lambda = Expression.Lambda<Func<T, T>>(memberInitExpression, parameterExpression);

        // Compile to Func<Data, Data>.
        return source.Select(lambda.Compile());
    }
}
