using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Moq;

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

        // TODO: should not expose Moq's Mock... this is a leaky abstraction and should be replaced with an adapter
        // if i use an adapter, then do i really need the proxy generator ??
        public Mock<T> Get<T>() where T : class
        {
            if (_cachedMocks.ContainsKey(typeof(T)))
                return (Mock<T>)_cachedMocks[typeof(T)];

            // TODO: don't really need this call
            var mock = _decoratedMockCreator.Get<T>();

            var proxiedMock = new ProxyGenerator().CreateClassProxy(typeof(Mock<T>), new Interceptor());
            _cachedMocks.Add(typeof(T), proxiedMock);

            return (Mock<T>)proxiedMock;
        }
    }
}
