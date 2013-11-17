using System;
using System.Collections.Generic;
using Moq;

namespace Altruistic
{
    public class MockCreatorDecorator : ICreateMock
    {
        private readonly ICreateMock _decoratedMockCreator;
        private readonly IDictionary<Type, Mock> _cachedMocks = new Dictionary<Type, Mock>(); 

        public MockCreatorDecorator(ICreateMock decoratedMockCreator)
        {
            _decoratedMockCreator = decoratedMockCreator;
        }

        public Mock CreateFromType(Type type)
        {
            if (_cachedMocks.ContainsKey(type))
                return _cachedMocks[type];

            var mock = _decoratedMockCreator.CreateFromType(type);
            _cachedMocks.Add(type, mock);

            return mock;
        }
    }
}
