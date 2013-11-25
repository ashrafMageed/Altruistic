using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FizzWare.NBuilder;

namespace Altruistic
{
    public class ObjectBuilder : IObjectBuilderAdapter
    {

        public TObject CreateNew<TObject>()
        {
            var parameterlessConstructor = typeof(TObject).GetConstructor(Type.EmptyTypes);
            if (parameterlessConstructor != null)
                return Builder<TObject>.CreateNew().Build();

            return CreateObjectWithNoDefaultConstructor<TObject>();
        }

        public TObject CreateNewWithConstructor<TObject>(Expression<Func<TObject>> constructor)
        {
            return Builder<TObject>.CreateNew().WithConstructor(constructor).Build();
        }

        private TObject CreateObjectWithNoDefaultConstructor<TObject>()
        {
            // TODO: use MaxBy from MoreLinq as this is currently very ineffecient
            var parameterizedConstructor = typeof(TObject).GetConstructors().OrderByDescending(x => x.GetParameters().Count()).First();
            var arguments = GetDefaultArguments(parameterizedConstructor);
            var ctor = Expression.New(parameterizedConstructor, arguments);
            var expression = Expression.Lambda<Func<TObject>>(ctor, null);

            return CreateNewWithConstructor(expression);
        }

        private IEnumerable<Expression> GetDefaultArguments(ConstructorInfo parameterizedConstructor)
        {
            var parameters = parameterizedConstructor.GetParameters().ToList();
            return parameters.Select(p => CreateDefaultExpressionConstant(p.ParameterType));
        }

        private Expression CreateDefaultExpressionConstant(Type type)
        {
            return type.IsValueType ? Expression.Constant(Activator.CreateInstance(type)) : Expression.Constant(null, type);
        }
    }
}