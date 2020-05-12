using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain.Interfaces;

namespace WebStoreApp.Data.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal WebStoreAppContext _context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(WebStoreAppContext context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public async virtual Task<IEnumerable<TEntity>> All()
        {
            return await dbSet.ToListAsync();
        }

        public async virtual Task<TEntity> Insert(TEntity entity)
        {
            var _entity = dbSet.Add(entity).Entity;
            return await Task.FromResult(_entity);
        }

        public async virtual Task<TEntity> Update(TEntity entity)
        {
            TEntity _entity = dbSet.Update(entity).Entity;
            return await Task.FromResult(_entity);
        }

        public async virtual Task<TEntity> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async virtual Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //TODO
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async virtual Task Delete(object id)
        {
            TEntity entity = await dbSet.FindAsync(id);
            await Delete(entity);
        }

        public async virtual Task Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            await Task.FromResult(dbSet.Remove(entity));
            
        }
    }
}