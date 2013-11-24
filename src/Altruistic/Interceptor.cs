using System;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;

namespace Altruistic
{
    public class Interceptor<T> : IInterceptor where T : Wrapper
    {
        private readonly T _mockingWrapper;
        private readonly ICreateTestObject _testObjectCreator;

        public Interceptor(T mockingWrapper, ICreateTestObject testObjectCreator)
        {
            _mockingWrapper = mockingWrapper;
            _testObjectCreator = testObjectCreator;
        }

        public void Intercept(IInvocation invocation)
        {
            if(invocation.TargetType != null)
                invocation.Proceed();

            if (invocation.ReturnValue != null || _mockingWrapper.MethodHasSetup(invocation.Method.DeclaringType, invocation.Method)) 
                return;

            var type = invocation.Method.ReturnType;
            var dummyReturn = InvokeParameterlessGenericMethod(Utility.GetMethod(_testObjectCreator.CreateDummy<object>), type);
            invocation.ReturnValue = dummyReturn;
        }

        private object InvokeParameterlessGenericMethod(MethodInfo method, Type genericMethodType)
        {
            if (method == null)
                throw new ArgumentNullException();

            var call = Expression.Call(Expression.Constant(_testObjectCreator), method.Name, new[] { genericMethodType });
            return Expression.Lambda(call).Compile().DynamicInvoke();
        }

    }
}
