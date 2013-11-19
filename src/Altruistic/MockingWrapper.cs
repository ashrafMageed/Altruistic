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
    public class MockingWrapper
    {
        public object Object { get; private set; }

        public MockingWrapper(Mock mock)
        {
            Object = mock.Object;
        }
    }

    public class MockingWrapper<T> : MockingWrapper where T : class
    {
        private readonly Mock<T> _proxiedInstance;
        private IList<string> _setMethods = new List<string>();

        public MockingWrapper(Mock<T> proxiedInstance) : base(new ProxyGenerator().CreateClassProxy<Mock<T>>(new Interceptor()))
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
            var uniqueKey = CreateUniqueKey(typeof (T), method);
            _setMethods.Add(uniqueKey);
            _proxiedInstance.Setup(expression);
        }

        public Mock<T> MockedInstance 
        {
            get { return _proxiedInstance; }
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

    }
}
