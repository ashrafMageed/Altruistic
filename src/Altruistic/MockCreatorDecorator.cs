using System;
using System.Collections.Generic;

namespace Altruistic
{
    // don't need this class anymore.... this functionality can go into the mocking wrapper/adapter
    public class MockCreatorDecorator : ICreateMock
    {
        private readonly ICreateMock _decoratedMockCreator;
        private readonly IDictionary<Type, object> _cachedMocks = new Dictionary<Type, object>(); 

        public MockCreatorDecorator(ICreateMock decoratedMockCreator)
        {
            _decoratedMockCreator = decoratedMockCreator;
        }

        // TODO: should not expose Moq's Mock... this is a leaky abstraction and should be replaced with an adapter
        // if i use an adapter, then do i really need the proxy generator ??
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
