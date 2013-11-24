using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Altruistic.Tests
{
    public class ObjectBuilderTests
    {
        public class CreateNewMethodTests
        {
            public void WhenCreatingNewObject_ShouldReturnObjectWithAllPropertiesInitialised()
            {
                var sutCreator = new SUTCreator();
                var sut = sutCreator.Create<ObjectBuilder>();

                var result = sut.CreateNew<AutoMockerTests.TestComplexType>();

                result.Should().NotBeNull();
                result.Test.Should().NotBe(0);

            }
        }

        public class CreateNewWithConstructorMethodTests
        {
            public void WhenCreatingNewObjectThroughConstructor_ShouldSuccessfullyCreateObject()
            {
                var sutCreator = new SUTCreator();
                var sut = sutCreator.Create<ObjectBuilder>();

                var result = sut.CreateNewWithConstructor(() => new AutoMockerTests.SUTWithPrimitiveParameters(12));

                result.Should().NotBeNull();
            }

            public void WhenCreatingNewObjectThroughConstructor_ShouldInitialiseAllObjectProperties()
            {
                var type = typeof(AutoMockerTests.SUTWithPrimitiveParameters);

                var call = Expression.Call(GetType(), "CreateLambda", new[] {type}); //Expression.Lambda<Func<object>>(ctor, null);

                var result = Expression.Lambda(call).Compile().DynamicInvoke();
                result.Should().NotBeNull();
                
            }

            public static TObject CreateLambda<TObject>()
            {
                var type = typeof(TObject);
                var constructor = type.GetConstructors().First();
                var parameters = constructor.GetParameters();

                var arguments = parameters.Select(p => CreateDefaultExpressionConstant(p.ParameterType));
                var ctor = Expression.New(constructor, arguments);
                var sutCreator = new SUTCreator();
                var sut = sutCreator.Create<ObjectBuilder>();
                var expression = Expression.Lambda<Func<TObject>>(ctor, null);
                return sut.CreateNewWithConstructor(expression);

            }

            public static Expression CreateDefaultExpressionConstant(Type type)
            {
                if (type.IsValueType)
                    return Expression.Constant(Activator.CreateInstance(type));

                return Expression.Constant(null, type);
            }
        }
    }
}
