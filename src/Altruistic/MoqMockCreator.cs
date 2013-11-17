using System;
using Moq;

namespace Altruistic
{
    public class MoqMockCreator : ICreateMock
    {
        public Mock CreateFromType(Type type)
        {
            var genericConstructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes);
            return (Mock)genericConstructor.Invoke(new object[0]);
        }
    }
}
