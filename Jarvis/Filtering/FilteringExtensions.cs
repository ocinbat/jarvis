using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jarvis.Filtering
{
    public static class FilteringExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, params QueryParameter[] queryParameters)
        {
            if (queryParameters.HasElements())
            {
                foreach (QueryParameter parameter in queryParameters)
                {
                    if (parameter.Value != null)
                    {
                        ParameterExpression param = Expression.Parameter(typeof(T), "p");
                        Expression<Func<T, bool>> exp = Expression.Lambda<Func<T, bool>>(
                            Expression.Equal(
                                Expression.Property(param, parameter.Name),
                                Expression.Constant(parameter.Value)
                            ),
                            param
                        );

                        source = source.Where(exp.Compile());
                    }
                }
            }

            return source;
        }
    }
}
