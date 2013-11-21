using Castle.DynamicProxy;

namespace Altruistic
{
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var name = invocation.Method.Name;
            invocation.Proceed();
            var returnValue = invocation.ReturnValue;
        }
        
    }
}
