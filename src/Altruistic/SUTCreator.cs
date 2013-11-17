using System;
using System.Linq;
using Moq;

namespace Altruistic
{
    public class SUTCreator
    {
        private readonly ICreateMock _mockCreator;

        public SUTCreator() : this(new MockCreatorDecorator(new MoqMockCreator())){}

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
            var constructorParameters = parameters.Select(parameter => GetDefault(parameter.ParameterType, _mockCreator.CreateFromType));

            return (T)Activator.CreateInstance(typeof(T), constructorParameters.ToArray());
        }

        public Mock<T> GetMock<T>() where T : class
        {
            return (Mock<T>)_mockCreator.CreateFromType(typeof (T));
        }

        private static object GetDefault(Type type, Func<Type, Mock> getReferenceTypeDefault)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : getReferenceTypeDefault(type).Object;
        }
    }
}
