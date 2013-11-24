using System;
using System.Linq;
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

            // TODO: use MaxBy from MoreLinq as this is currently very ineffecient
            var parameterizedConstructor = typeof(TObject).GetConstructors().OrderByDescending(x => x.GetParameters().Count()).First();
            var parameters = parameterizedConstructor.GetParameters().ToList();
            var arguments = parameters.Select(p => CreateDefaultExpressionConstant(p.ParameterType));
            var ctor = Expression.New(parameterizedConstructor, arguments);
            var expression = Expression.Lambda<Func<TObject>>(ctor, null);

            return CreateNewWithConstructor(expression);
        }

        private Expression CreateDefaultExpressionConstant(Type type)
        {
            if (type.IsValueType)
                return Expression.Constant(Activator.CreateInstance(type));

            return Expression.Constant(null, type);
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
