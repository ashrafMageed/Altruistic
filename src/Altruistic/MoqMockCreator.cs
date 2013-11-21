using Moq;

namespace Altruistic
{
    // TODO: I don't need this class anymore
    public class MockCreator : ICreateMock
    {
        public MockingWrapper<T> Get<T>() where T : class
        {
            return new MockingWrapper<T>(new Mock<T>());
        }
    }
}
