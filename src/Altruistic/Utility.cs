using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Altruistic
{
    public static class Utility
    {
        public static MethodInfo GetMethod(LambdaExpression expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException("expression must be a method call");
            }
            return methodCallExpression.Method;
        }

        public static string GetMethodName(LambdaExpression expression)
        {
            return GetMethod(expression).Name;
        }

        public static MethodInfo GetMethod(Func<object> expression)
        {
            return expression.Method;
        }
    }
}
