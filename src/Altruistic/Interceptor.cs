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
            var dummyReturn = Utility.InvokeParameterlessGenericMethod(_testObjectCreator, Utility.GetMethod(_testObjectCreator.CreateDummy<object>), type);
            invocation.ReturnValue = dummyReturn;
        }

    }
}
