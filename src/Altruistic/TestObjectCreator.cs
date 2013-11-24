﻿using System;
using System.Linq.Expressions;
using FizzWare.NBuilder;

namespace Altruistic
{
    public interface IObjectBuilderAdapter
    {
        TObject CreateNew<TObject>();
        TObject CreateNewWithConstructor<TObject>(Expression<Func<TObject>> constructor);
    }

    public interface ICreateTestObject
    {
        TObject CreateDummy<TObject>();
    }

    public class ObjectBuilder : IObjectBuilderAdapter
    {

        public TObject CreateNew<TObject>()
        {
            var parameterlessConstructor = typeof(TObject).GetConstructor(Type.EmptyTypes);
            if (parameterlessConstructor != null)
                return Builder<TObject>.CreateNew().Build();

//            var parameterizedConstructor = typeof(TObject).GetConstructors().OrderByDescending(x => x.GetParameters().Count()).First();
//            var parameters = parameterizedConstructor.GetParameters().ToList();

            //var constructorParameters = parameters.Select(parameter => CreateParameter(parameter.ParameterType));
            // create constructor expression 
            //return Builder<TObject>.CreateNew().WithConstructor(() => new TObject()).Build();

            //return CreateNew(() => new TObject());
            return default(TObject);
        }

        public TObject CreateNewWithConstructor<TObject>(Expression<Func<TObject>> constructor)
        {
            return Builder<TObject>.CreateNew().WithConstructor(constructor).Build();
        }
    }

    public class TestObjectCreator : ICreateTestObject
    {
        private readonly IObjectBuilderAdapter _objectBuilder;

        public TestObjectCreator(IObjectBuilderAdapter objectBuilder)
        {
            _objectBuilder = objectBuilder;
        }

        public TObject CreateDummy<TObject>()
        {
            return _objectBuilder.CreateNew<TObject>();
        }
    }
}
