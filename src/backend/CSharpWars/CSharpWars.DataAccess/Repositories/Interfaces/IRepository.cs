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
        Task<TModel> Single(Expression<Func<TModel, Boolean>> predicate);

        Task<TModel> Create(TModel toCreate);

        Task Update(TModel toUpdate);

        Task Update(IList<TModel> toUpdate);
    }

    public interface IRepository<TModel1, TModel2> : IRepository<TModel1>
        where TModel1 : class, IHasId
        where TModel2 : class, IHasId, new()
    {

    }
}