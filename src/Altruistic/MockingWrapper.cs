using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using Moq;

namespace Altruistic
{
    public abstract class Wrapper
    {
        public abstract bool MethodHasSetup(Type type, MethodInfo method);
    }

    public class MockingWrapper<T> : Wrapper where T : class
    {
        private readonly Mock<T> _proxiedInstance;
        private IList<string> _setMethods = new List<string>();

        public MockingWrapper(Mock<T> proxiedInstance)
        {
            _proxiedInstance = proxiedInstance;
        }

        public void Verify(Expression<Action<T>> expression, Times times)
        {
            _proxiedInstance.Verify(expression, times);
        }

        public void Setup<TResult>(Expression<Func<T, TResult>> expression)
        {
            var method = GetMethod(expression);
            var uniqueKey = CreateUniqueKey(typeof(T), method);
            _setMethods.Add(uniqueKey);
            _proxiedInstance.Setup(expression);
        }

        public T Object
        {
            get
            {
                return typeof(T).IsInterface ? (T)new ProxyGenerator().CreateInterfaceProxyWithoutTarget(typeof(T), new Altruistic.Interceptor<MockingWrapper<T>>(this)) : _proxiedInstance.Object;
            }
        }

        internal string CreateUniqueKey(Type type, MethodInfo method)
        {
            return typeof(T).Name + method.Name + GetParametersTypesString(method.GetParameters());
        }

        private string GetParametersTypesString(IEnumerable<ParameterInfo> parameterInfo)
        {
            var stringBuilder = new StringBuilder();
            parameterInfo.ToList().ForEach(parameter => stringBuilder.Append(parameter.ParameterType.Name));
            return stringBuilder.ToString();
        }



        internal MethodInfo GetMethod<TTarget, TResult>(Expression<Func<TTarget, TResult>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException("expression must be a method call");
            }
            return methodCallExpression.Method;
        }


        public override bool MethodHasSetup(Type type, MethodInfo method)
        {
            var key = CreateUniqueKey(type, method);
            return _setMethods.Contains(key);
        }
    }
}
