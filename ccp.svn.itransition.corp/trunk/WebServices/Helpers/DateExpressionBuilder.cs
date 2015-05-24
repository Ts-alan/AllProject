using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CCP.WebApi.Models;

namespace CCP.WebApi.Helpers
{
    public static class DateExpressionBuilder
    {
        private static Expression GetDatePartStringOperationExpression(Expression expression, string datePartName, string value, string stringOperationName)
        {
            expression = Expression.PropertyOrField(expression, datePartName);
            expression = Expression.Call(expression, "ToString", null, null);
            expression = Expression.Call(expression, stringOperationName, null, Expression.Constant(value));
            return expression;
        }

        public static Expression GetDateValueExpression(string value, Expression expression)
        {
            var dateParts = ConvertToDate(value);
            if (dateParts != null)
            {
                var temp = expression;
                if (dateParts.Length == 3)
                {
                    expression = Expression.And(Expression.Equal(Expression.PropertyOrField(expression, "Month"), Expression.Constant(int.Parse(dateParts[0]))),
                            Expression.Equal(Expression.PropertyOrField(expression, "Day"), Expression.Constant(int.Parse(dateParts[1]))));
                    expression =
                        Expression.And(expression, GetDatePartStringOperationExpression(temp, "Year", dateParts[2], "StartsWith"));
                }
                else
                {
                    if (dateParts.Length == 2)
                    {
                        Expression expressionForDays;
                        if (dateParts[1].Length == 2)
                        {
                            expressionForDays =
                                Expression.Equal(Expression.PropertyOrField(expression, "Day"),
                                    Expression.Constant(int.Parse(dateParts[1])));
                        }
                        else
                        {
                            expressionForDays = GetDatePartStringOperationExpression(temp, "Day", dateParts[1], "StartsWith");
                        }

                        expression = Expression.And(Expression.Equal(Expression.PropertyOrField(expression, "Month"), Expression.Constant(int.Parse(dateParts[0]))),
                            expressionForDays);
                        expression = Expression.Or(expression, Expression.And(Expression.Equal(Expression.PropertyOrField(temp, "Day"), Expression.Constant(int.Parse(dateParts[0]))),
                            GetDatePartStringOperationExpression(temp, "Year", dateParts[1], "StartsWith")));
                    }
                    else
                    {
                        expression = Expression.Or(GetDatePartStringOperationExpression(temp, "Day", dateParts[0], "Contains"), GetDatePartStringOperationExpression(temp, "Month", dateParts[0], "Contains"));
                        expression =
                            Expression.Or(expression, GetDatePartStringOperationExpression(temp, "Year", dateParts[0], "Contains"));
                    }
                }
                return expression;
            }

            return Expression.Equal(Expression.Call(expression, "ToString", null, null), Expression.Constant(""));
        }

        private static string[] ConvertToDate(string value)
        {
            var dateParts = value.Split(new[] { '/', ',', '.', '-', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (dateParts.Length <= 3 && dateParts.Length != 0)
            {
                foreach (var datePart in dateParts)
                {
                    int number;
                    if (!int.TryParse(datePart, out number))
                    {
                        return null;
                    }
                }
                return dateParts;
            }

            return null;
        }
    }
}