using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.Model;

namespace CSharpWars.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TModel> where TModel : ModelBase
    {
        Task<IEnumerable<TModel>> GetAll();
    }
}