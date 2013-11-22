using System;
using System.Linq.Expressions;
using Castle.DynamicProxy;
using FizzWare.NBuilder;

namespace Altruistic
{
    public class Interceptor<T> : IInterceptor where T : Wrapper
    {
        private readonly T _mockingWrapper; 

        public Interceptor(T mockingWrapper)
        {
            _mockingWrapper = mockingWrapper;
        }

        public void Intercept(IInvocation invocation)
        {
            if(invocation.TargetType != null)
                invocation.Proceed();

            if (invocation.ReturnValue == null && !_mockingWrapper.MethodHasSetup(invocation.Method.DeclaringType, invocation.Method))
            {
                var type = invocation.Method.ReturnType;
                var dummyReturn = InvokeParameterlessGenericMethod("CreateDummy", type);
                invocation.ReturnValue = dummyReturn;
            }
            // is method set
        }

        public static TObject CreateDummy<TObject>()
        {
            // does not handle types with no default constructors
            // need to extend NBuilder
            return Builder<TObject>.CreateNew().Build();
        }

        private object InvokeParameterlessGenericMethod(Type declaringType, string methodName, Type genericMethodType)
        {
            var call = Expression.Call(GetType(), methodName, new[] {genericMethodType});
            return Expression.Lambda(call).Compile().DynamicInvoke();
        }

    }
}
