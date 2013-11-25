using System;
using System.Linq.Expressions;

namespace Altruistic
{
    public interface IObjectBuilderAdapter
    {
        TObject CreateNew<TObject>();
        TObject CreateNewWithConstructor<TObject>(Expression<Func<TObject>> constructor);
    }
}