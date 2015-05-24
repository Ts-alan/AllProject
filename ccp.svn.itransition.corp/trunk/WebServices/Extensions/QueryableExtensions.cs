using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using CCP.WebApi.Helpers;
using CCP.WebApi.Models;

namespace CCP.WebApi.Extensions
{
    public static class QueryableExtensions
    {
        public static DataSourceResult GetDataSource<T>(this IQueryable<T> source, DataSourceRequest request = null)
        {
            var count = 0;
            if (source != null && source.Any() && request != null)
            {
                source = source.Filtering(request.FilterItems);
                source = source.Sorting(request.SortItems);
                count = source.Count();
                source = source.Paging(request.PageNumber, request.PageSize, count);
            }

            return CreateDataSourceResult(source, count);
        }


        private static IQueryable<T> Paging<T>(this IQueryable<T> source, int pageNumber, int pageSize, int count)
        {
            var position = (pageNumber - 1) * pageSize;
            if ((position + pageSize) > count)
            {
                pageSize = count - position;
            }

            if (pageSize > 0)
            {
                source = (position < 0 || pageSize < 0) ? source : source.Skip(position).Take(pageSize);
            }

            return source;
        }


        private static IQueryable<T> Filtering<T>(this IQueryable<T> source, IList<FilterItem> filterItems = null)
        {
            if (filterItems != null && filterItems.Any())
            {
                var type = typeof(T);
                var groupedFilterItems = filterItems.GroupBy(f => f.FilterOperation);
                ParameterExpression p = Expression.Parameter(type, "p");
                Expression expression = null;
                foreach (var groupedFilterItem in groupedFilterItems)
                {
                    Expression leftExpression = null;
                    foreach (var filterItem in groupedFilterItem)
                    {
                        if (!string.IsNullOrEmpty(filterItem.FilterValue))
                        {
                            leftExpression = GetExpressionByFilterItem(leftExpression, p, filterItem);
                        }
                    }
                    if (leftExpression != null)
                    {
                        expression = expression == null ? leftExpression : Expression.AndAlso(expression, leftExpression);
                    }
                }
                if (expression != null)
                {
                    var keySelector = Expression.Lambda(expression, new[] { p });
                    source = source.Provider.CreateQuery<T>(Expression.Call(
                        typeof(Queryable),
                        "Where",
                        new[] { type },
                        new[] { source.Expression, keySelector }));
                }
            }

            return source;
        }

        private static Expression GetExpressionByFilterItem(Expression leftExpression, Expression parameterExpression, FilterItem filterItem)
        {
            var rightExpression = GetPropertyExpression(filterItem.FilterFieldNames.First(), parameterExpression);
            Func<Expression, Expression, BinaryExpression> operation = null;
            switch (filterItem.FilterOperation)
            {
                case "equal":
                    rightExpression = GetEqualExpression(rightExpression, filterItem);
                    operation = Expression.And; 
                    break;

                case "contain":
                    rightExpression = GetContainExpression(parameterExpression, filterItem);
                    operation = Expression.Or;
                    break;

                case "greaterThan":
                    rightExpression = GetComparisonOperationExpression(rightExpression, filterItem, Expression.GreaterThan);
                    operation = Expression.And;
                    break;

                case "lessThan":
                    rightExpression = GetComparisonOperationExpression(rightExpression, filterItem, Expression.LessThan);
                    operation = Expression.And;
                    break;
            }
            leftExpression = MergeExpressions(leftExpression, rightExpression, operation);

            return leftExpression;
        }

        private static Expression GetComparisonOperationExpression(Expression expression, FilterItem filterItem, Func<Expression, Expression, BinaryExpression> comparisonOperation)
        {
            if (expression.Type == typeof(DateTime?))
            {
                Expression hasValueExpression = Expression.PropertyOrField(expression, "HasValue");
                expression = Expression.Convert(expression, typeof (DateTime));
                expression = comparisonOperation(expression,  Expression.Constant(DateTime.Parse(filterItem.FilterValue, CultureInfo.GetCultureInfo("en"))) );
                expression = Expression.And(hasValueExpression, expression);
            }
            return expression;
        }

        private static Expression GetContainExpression( Expression parameterExpression, FilterItem filterItem)
        {
            Expression expression = null;
            if (filterItem.FilterFieldNames.Count == 1)
            {
                expression = GetPropertyExpression(filterItem.FilterFieldNames.First(), parameterExpression);
                if (expression.Type == typeof(DateTime?))
                {
                    Expression hasValueExpression = Expression.PropertyOrField(expression, "HasValue");
                    expression = Expression.Convert(expression, typeof(DateTime));
                    expression = DateExpressionBuilder.GetDateValueExpression(filterItem.FilterValue, expression);
                    expression = Expression.And(hasValueExpression, expression);
                }
                else
                {
                    if (expression.Type == typeof (int?))
                    {
                        Expression hasValueExpression = Expression.PropertyOrField(expression, "HasValue");
                        expression = Expression.Convert(expression, typeof (int));
                        expression = Expression.Call(expression, "ToString", null, null);
                        expression = Expression.Call(expression, "ToLower", null, null);
                        expression = Expression.Call(expression, "Contains", null,
                            Expression.Constant(filterItem.FilterValue.ToLower()));
                        expression = Expression.And(hasValueExpression, expression);
                    }
                    else
                    {
                        expression = Expression.Call(expression, "ToString", null, null);
                        expression = Expression.Call(expression, "ToLower", null, null);
                        expression = Expression.Call(expression, "Contains", null, Expression.Constant(filterItem.FilterValue.ToLower()));
                    }
                }
            }
            else
            {
                
                foreach (var fieldName in filterItem.FilterFieldNames)
                {
                    var tempExpression = GetPropertyExpression(fieldName, parameterExpression);
                    tempExpression = Expression.Call(tempExpression, "ToString", null, null);
                    tempExpression = Expression.Call(tempExpression, "ToLower", null, null);
                    if (expression == null)
                    {
                        expression = tempExpression;
                    }
                    else
                    {
                        expression =
                            Expression.Call(
                                typeof (String).GetMethod("Concat", new[] {typeof (String), typeof (String)}),
                                expression, tempExpression);
                    }
                }
                if (expression != null)
                {
                    var value = filterItem.FilterValue.Replace(" ", "").ToLower();
                    expression = Expression.Call(expression, "Contains", null, Expression.Constant(value)); 
                }
            }
            return expression;
        }

        private static Expression GetEqualExpression(Expression expression, FilterItem filterItem)
        {
            return Expression.Equal(expression, Expression.Constant(filterItem.FilterValue));
        }

        private static Expression MergeExpressions(Expression leftExpression, Expression rightExpression,Func<Expression,Expression, BinaryExpression> binaryOperation )
        {
            if (leftExpression == null || binaryOperation == null || rightExpression == null)
            {
                leftExpression = rightExpression;
            }
            else
            {
                leftExpression = binaryOperation(leftExpression, rightExpression);
            }
            return leftExpression;
        }

        private static Expression GetPropertyExpression(string propertyName, Expression expression)
        {
            var properties = propertyName.Split(new [] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            return properties.Aggregate(expression, Expression.PropertyOrField);
        }

        private static IQueryable<T> Sorting<T>(this IQueryable<T> source, IList<SortItem> sortItems = null)
        {
            if (sortItems != null && sortItems.Any())
            {
                int length = sortItems.Count;
                source = source.OrderBy(sortItems.First().FieldName, sortItems.First().Direction.Equals("asc") ? "OrderBy" : "OrderByDescending");
                for (int i = 1; i < length; i++)
                {
                    source = source.OrderBy(sortItems[i].FieldName, sortItems[i].Direction.Equals("asc") ? "ThenBy" : "ThenByDescending");
                }
            }

            return source;
        }


        private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyNameName, string direction)
        {
            var type = typeof(TSource);
            ParameterExpression p = Expression.Parameter(type, "p");
            Expression mbr = p;
            mbr = GetPropertyExpression(propertyNameName,mbr);
            var keySelector = Expression.Lambda(mbr, new[] { p });
            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(
                typeof(Queryable),
                direction,
                new[] { type, mbr.Type },
                new[] { source.Expression, keySelector }));
        }

        private static DataSourceResult CreateDataSourceResult<T>(IQueryable<T> source, int count = 0)
        {
            var model = new DataSourceResult
            {
                DataCount = count,
                Data = source != null ? source.ToList() : new List<T>()
            };

            return model;
        }
    }
}