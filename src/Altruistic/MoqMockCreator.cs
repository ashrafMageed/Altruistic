using Moq;

namespace Altruistic
{
    // TODO: I don't need this class anymore
    public class MoqMockCreator : ICreateMock
    {
        public Mock<T> Get<T>() where T : class
        {
            return new Mock<T>();
        }
    }
}
