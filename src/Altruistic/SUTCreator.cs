using System;
using System.Linq;

namespace Altruistic
{
    public class SUTCreator
    {
        private readonly ICreateMock _mockCreator;

        public SUTCreator() : this(new MockCreatorDecorator(new MockCreator())){}

        public SUTCreator(ICreateMock mockCreator)
        {
            if(mockCreator == null)
                throw new ArgumentNullException("injected ICreateMock is null");

            _mockCreator = mockCreator;
        }

        /// <summary>
        /// Create the System Under Test (SUT) and all it's constructor dependencies
        /// </summary>
        /// <typeparam name="T"> SUT to create</typeparam>
        /// <returns>SUT with all constructor dependencies created</returns>
        public T Create<T>() where T : class
        {
            var constructors = typeof(T).GetConstructors().ToList();
            var parameterlessConstructor = typeof(T).GetConstructor(Type.EmptyTypes);

            if (parameterlessConstructor != null && constructors.Count == 1)
                return (T)Activator.CreateInstance(typeof(T));
;
            var parameterizedConstructor = constructors.OrderByDescending(x => x.GetParameters().Count()).First();
            var parameters = parameterizedConstructor.GetParameters().ToList();
            var constructorParameters = parameters.Select(parameter => GetParameterValue(parameter.ParameterType));
            
            return (T)Activator.CreateInstance(typeof(T), constructorParameters.ToArray());
        }

        public object GetMockObject<T>() where T : class
        {
            return GetMock<T>().Object;
        }

        public MockingWrapper<T> GetMock<T>() where T : class
        {
            return _mockCreator.Get<T>();
        }

        private object GetParameterValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            // TODO: remove magic string
            var genericMethod = GetType().GetMethod("GetMockObject").MakeGenericMethod(type);
            return genericMethod.Invoke(this, null);
        }

    }
}
