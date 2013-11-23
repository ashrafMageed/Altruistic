using System;
using System.Linq.Expressions;
using System.Reflection;
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
                var dummyReturn = InvokeParameterlessGenericMethod(Utility.GetMethod(CreateDummy<object>), type);
                invocation.ReturnValue = dummyReturn;
            }
        }

        public static TObject CreateDummy<TObject>()
        {
            // does not handle types with no default constructors
            // need to extend NBuilder
            var parameterlessConstructor = typeof(TObject).GetConstructor(Type.EmptyTypes);
            if (parameterlessConstructor != null)
                return Builder<TObject>.CreateNew().Build();

            // create constructor expression 
            //return Builder<TObject>.CreateNew().WithConstructor(constructor expression).Build();
            return default(TObject);
        }

        private object InvokeParameterlessGenericMethod(MethodInfo method, Type genericMethodType)
        {
            var call = Expression.Call(method.DeclaringType, method.Name, new[] { genericMethodType });
            return Expression.Lambda(call).Compile().DynamicInvoke();
        }

    }
}
