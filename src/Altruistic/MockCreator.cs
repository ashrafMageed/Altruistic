using Moq;

namespace Altruistic
{
    public class MockCreator : ICreateMock
    {
        public MockingWrapper<T> Get<T>() where T : class
        {
            return new MockingWrapper<T>(new Mock<T>());
        }
    }
}
