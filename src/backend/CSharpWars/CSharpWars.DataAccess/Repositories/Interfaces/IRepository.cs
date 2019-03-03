using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TModel> where TModel : ModelBase
    {
        Task<IList<TModel>> GetAll();

        Task<IList<TModel>> Find(Expression<Func<TModel, Boolean>> predicate);

        Task<TModel> Create(TModel toCreate);

        Task Update(IList<TModel> toUpdate);
    }
}