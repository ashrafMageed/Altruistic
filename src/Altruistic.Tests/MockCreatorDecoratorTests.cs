using Moq;

namespace Altruistic.Tests
{
    public class MockCreatorDecoratorTests
    {
        public void CreateMock_WhenTypeIsNotInCache_ShouldGetMockFromDecoratedObject()
        {
            // Arrange
            var sutCreator = new SUTCreator();
            var mockCreatorDecorator = sutCreator.Create<MockCreatorDecorator>();

            // Act
            mockCreatorDecorator.Get<TestClass>();

            // Assert
            sutCreator.GetMock<ICreateMock>().Verify(x => x.Get<TestClass>(), Times.Once());
        }

        public void CreateMock_WhenTypeIsInCache_ShouldReturnTypeFromCache()
        {
            // Arrange
            var sutCreator = new SUTCreator();
            var mockCreatorDecorator = sutCreator.Create<MockCreatorDecorator>();

            // Act
            mockCreatorDecorator.Get<TestClass>();
            mockCreatorDecorator.Get<TestClass>();

            // Assert
            sutCreator.GetMock<ICreateMock>().Verify(x => x.Get<TestClass>(), Times.Once());
        }

        public class TestClass {}
    }
}
