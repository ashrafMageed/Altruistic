using FluentAssertions;
using Moq;

namespace Altruistic.Tests
{
    public class TestObjectCreatorTests
    {
        public class CreateDummyMethodTests
        {
            public void WhenObjectHasParameterlessConstructor_ShouldCreateDummyObjectWithInitialisedFields()
            {
                // Arrange
                var sutCreator = new SUTCreator();
                var objectCreator = sutCreator.Create<TestObjectCreator>();

                // Act
                var dummyTestObject = objectCreator.CreateDummy<AutoMockerTests.TestComplexType>();

                // Assert
                dummyTestObject.Should().NotBeNull();
                dummyTestObject.Test.Should().NotBe(0);
            }
        }
    }
}
