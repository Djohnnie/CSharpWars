using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TModel>
        where TModel : class, IHasId
    {
        Task<IList<TModel>> GetAll();

        Task<IList<TModel>> Find(Expression<Func<TModel, Boolean>> predicate);

        Task<IList<TModel>> Find<TProperty>(Expression<Func<TModel, Boolean>> predicate, Expression<Func<TModel, TProperty>> include);

        Task<IList<TModel>> FindDescending<TKey>(Expression<Func<TModel, TKey>> keySelector, int count);

        Task<TModel> Single(Expression<Func<TModel, Boolean>> predicate);

        Task<TModel> Create(TModel toCreate);

        Task Create(IList<TModel> toCreate);

        Task Update(TModel toUpdate);

        Task Update(IList<TModel> toUpdate);

        Task Delete(IList<TModel> toDelete);
    }
}