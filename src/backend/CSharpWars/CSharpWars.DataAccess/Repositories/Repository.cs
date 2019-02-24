using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.Model;
using Microsoft.EntityFrameworkCore;

namespace CSharpWars.DataAccess.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : ModelBase
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TModel> _dbSet;

        public Repository(DbContext dbContext, DbSet<TModel> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public async Task<IEnumerable<TModel>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> Find(Expression<Func<TModel, Boolean>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}