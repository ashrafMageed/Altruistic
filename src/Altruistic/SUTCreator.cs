﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Altruistic
{
    public class SUTCreator
    {
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
            var constructorParameters = parameters.Select(parameter => GetDefault(parameter.ParameterType, CreateMock));

            return (T)Activator.CreateInstance(typeof(T), constructorParameters.ToArray());
        }

        private object CreateMock(Type type)
        {
            var genericConstructor = typeof(Mock<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes);
            return ((Mock)genericConstructor.Invoke(new object[0])).Object;
        }

        public static object GetDefault(Type type, Func<Type, object> getReferenceTypeDefault)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : getReferenceTypeDefault(type);
        }
    }
}
