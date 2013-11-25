using System;
using System.Reflection;

namespace Altruistic
{
    public abstract class Wrapper
    {
        public abstract bool MethodHasSetup(Type type, MethodInfo method);
    }
}