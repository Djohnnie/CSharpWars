using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TModel> where TModel : ModelBase
    {
        Task<IEnumerable<TModel>> GetAll();

        Task<IEnumerable<TModel>> Find(Expression<Func<TModel, Boolean>> predicate);
    }
}