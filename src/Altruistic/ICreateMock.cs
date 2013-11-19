using System;
using Moq;

namespace Altruistic
{
    public interface ICreateMock
    {

        Mock<T> Get<T>() where T : class;
    }
}
