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

        public async Task<IList<TModel>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IList<TModel>> Find(Expression<Func<TModel, Boolean>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TModel> Create(TModel toCreate)
        {
            toCreate.Id = Guid.NewGuid();
            await _dbSet.AddAsync(toCreate);
            await _dbContext.SaveChangesAsync();
            return toCreate;
        }

        public async Task Update(IList<TModel> toUpdate)
        {
            foreach (var entity in toUpdate)
            {
                var trackedEntity = _dbContext.ChangeTracker.Entries<TModel>().SingleOrDefault(x => x.Entity.Id == entity.Id);
                if (trackedEntity != null)
                {
                    trackedEntity.State = EntityState.Detached;
                }
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.Entry(entity).Property(x => x.SysId).IsModified = false;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}