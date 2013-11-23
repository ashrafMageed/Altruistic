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
        private readonly IList<string> _setMethods = new List<string>();

        public MockingWrapper(Mock<T> proxiedInstance)
        {
            _proxiedInstance = proxiedInstance;
        }

        public void Verify(Expression<Action<T>> expression, Times times)
        {
            _proxiedInstance.Verify(expression, times);
        }

        public void Setup<TResult>(Expression<Func<T, TResult>> expression, TResult returnObject)
        {
            var method = Utility.GetMethod(expression);
            var uniqueKey = CreateUniqueKey(typeof(T), method);
            _setMethods.Add(uniqueKey);
            _proxiedInstance.Setup(expression).Returns(returnObject);
        }

        private T _object;
        public T Object
        {
            get
            {
                if (_object == null)
                {
                    _object =   typeof(T).IsInterface ?
                                (T)new ProxyGenerator().CreateInterfaceProxyWithTarget(typeof(T), _proxiedInstance.Object, new Interceptor<MockingWrapper<T>>(this)) :
                                _proxiedInstance.Object;
                }

                return _object;
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

        public override bool MethodHasSetup(Type type, MethodInfo method)
        {
            var key = CreateUniqueKey(type, method);
            return _setMethods.Contains(key);
        }
    }
}
