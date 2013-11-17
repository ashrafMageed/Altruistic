using System;
using Moq;

namespace Altruistic
{
    public interface ICreateMock
    {
        /// <summary>
        /// Creates a mocked object from the supplied type
        /// </summary>
        /// <param name="type">The type of the object to mock</param>
        /// <returns>mocked object</returns>
        Mock CreateFromType(Type type);
    }
}
