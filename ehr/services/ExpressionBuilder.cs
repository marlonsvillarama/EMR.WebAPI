using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace EMR.WebAPI.ehr.services
{
    public static class ExpressionBuilder
    {
        private static MethodInfo methodContains = typeof(string).GetMethod("Contains");
        private static MethodInfo methodStartsWith = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo methodEndsWith = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        public static Expression<Func<T, bool>> GetExpression<T>(IList<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            ParameterExpression px = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(px, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = GetExpression<T>(px, filters[0], filters[1]);
            }
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                    {
                        exp = GetExpression<T>(px, filters[0], filters[1]);
                    }
                    else
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(px, filters[0], filters[1]));
                    }

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(px, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, px);
        }


        private static Expression GetExpression<T>(ParameterExpression px, ExpressionFilter filter)
        {
            MemberExpression mx = Expression.Property(px, filter.PropertyName);
            ConstantExpression cx = Expression.Constant(filter.Value);

            switch(filter.Operation)
            {
                case Op.Equals:
                    return Expression.Equal(mx, cx);
                case Op.GreaterThan:
                    return Expression.GreaterThan(mx, cx);
                case Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(mx, cx);
                case Op.LessThan:
                    return Expression.LessThan(mx, cx);
                case Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(mx, cx);
                case Op.Contains:
                    return Expression.Call(mx, methodContains, cx);
                case Op.StartsWith:
                    return Expression.Call(mx, methodStartsWith, cx);
                case Op.EndsWidth:
                    return Expression.Call(mx, methodEndsWith, cx);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression px, ExpressionFilter filter1, ExpressionFilter filter2)
        {
            Expression bin1 = GetExpression<T>(px, filter1);
            Expression bin2 = GetExpression<T>(px, filter2);

            return Expression.AndAlso(bin1, bin2);
        }
    }

    public class ExpressionFilter
    {
        public string PropertyName { get; set; }
        public Op Operation { get; set; }
        public object Value { get; set; }
    }

    public enum Op
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWidth
    }
}

