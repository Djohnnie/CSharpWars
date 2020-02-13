using System;
using System.Linq.Expressions;
using Moq;

namespace CSharpWars.Tests.Framework.Mocks
{
    public static class Any
    {
        public static Expression<Func<TModel, bool>> Predicate<TModel>()
        {
            return It.IsAny<Expression<Func<TModel, bool>>>();
        }

        public static Expression<Func<TModel, TProperty>> Include<TModel, TProperty>()
        {
            return It.IsAny<Expression<Func<TModel, TProperty>>>();
        }
    }
}