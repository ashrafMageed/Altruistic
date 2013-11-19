using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
