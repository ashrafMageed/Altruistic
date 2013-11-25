using System;
using System.Collections.Generic;

namespace Altruistic
{
    public class MockCreatorDecorator : ICreateMock
    {
        private readonly ICreateMock _decoratedMockCreator;
        private readonly IDictionary<Type, object> _cachedMocks = new Dictionary<Type, object>(); 

        public MockCreatorDecorator(ICreateMock decoratedMockCreator)
        {
            _decoratedMockCreator = decoratedMockCreator;
        }

        public MockingWrapper<T> Get<T>() where T : class
        {
            if (_cachedMocks.ContainsKey(typeof(T)))
                return (MockingWrapper<T>)_cachedMocks[typeof(T)];

            var mock = _decoratedMockCreator.Get<T>();

            _cachedMocks.Add(typeof(T), mock);

            return mock;
        }
    }
}
