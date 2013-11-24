using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
