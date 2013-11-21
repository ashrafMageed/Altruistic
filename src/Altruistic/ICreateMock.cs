using System;
using Moq;

namespace Altruistic
{
    public interface ICreateMock
    {

        MockingWrapper<T> Get<T>() where T : class;
    }
}
