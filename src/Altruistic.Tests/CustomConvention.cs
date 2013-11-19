using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fixie;
using Fixie.Conventions;

namespace Altruistic.Tests
{
    public class CustomConvention : Convention
    {
        public CustomConvention()
        {
            Classes
                .NameEndsWith("Tests");

            Methods
                .Where(method => method.IsVoid());

            Parameters(FindInputs);
        }

        private static IEnumerable<object[]> FindInputs(MethodInfo method)
        {
            var parameters = method.GetParameters();

            if (parameters.Length == 1)
            {
                var testClass = method.ReflectedType;
                var parameterType = parameters.Single().ParameterType;

                var enumerableOfParameterType = typeof(IEnumerable<>).MakeGenericType(parameterType);

                var sources = testClass.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                       .Where(m => !m.GetParameters().Any())
                                       .Where(m => m.ReturnType == enumerableOfParameterType)
                                       .ToArray();

                foreach (var source in sources)
                    foreach (var input in (IEnumerable)source.Invoke(null, null))
                        yield return new[] { input };
            }
        }
    }
}