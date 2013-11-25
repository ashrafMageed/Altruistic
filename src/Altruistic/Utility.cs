using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Altruistic
{
    internal static class Utility
    {
        internal static MethodInfo GetMethod(LambdaExpression expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException("expression must be a method call");
            }
            return methodCallExpression.Method;
        }

        internal static string GetMethodName(LambdaExpression expression)
        {
            return GetMethod(expression).Name;
        }

        internal static MethodInfo GetMethod(Func<object> expression)
        {
            return expression.Method;
        }

        internal static object InvokeParameterlessGenericMethod<TTarget>(TTarget target, MethodInfo method, Type genericMethodType) where TTarget : class 
        {
            if (method == null)
                throw new ArgumentNullException();

            var call = Expression.Call(Expression.Constant(target), method.Name, new[] { genericMethodType });
            return Expression.Lambda(call).Compile().DynamicInvoke();
        }

        internal static ConstructorInfo GetConstructorWithMostParameters(IEnumerable<ConstructorInfo> constructors)
        {
            // TODO: use MaxBy from MoreLinq as this is currently very ineffecient
            return constructors.OrderByDescending(x => x.GetParameters().Count()).First();
        }
    }
}
