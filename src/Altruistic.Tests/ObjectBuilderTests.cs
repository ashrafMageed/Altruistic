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

            public void WhenCreatingObjectWithNoDefaultContructor_ShouldCreateObjectSuccessfully()
            {
                var sutCreator = new SUTCreator();
                var sut = sutCreator.Create<ObjectBuilder>();

                var result = sut.CreateNew<AutoMockerTests.SUTWithPrimitiveParameters>();

                result.Should().NotBeNull();
            }
        }

        public class CreateNewWithConstructorMethodTests
        {
            public void WhenCreatingNewObjectThroughConstructor_ShouldSuccessfullyCreateObject()
            {
                var sutCreator = new SUTCreator();
                var sut = sutCreator.Create<ObjectBuilder>();

                var result = sut.CreateNewWithSpecificConstructor(() => new AutoMockerTests.SUTWithPrimitiveParameters(12));

                result.Should().NotBeNull();
            }
        }
    }
}
