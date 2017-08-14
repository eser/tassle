// --------------------------------------------------------------------------
// <copyright file="EfRepository{TEntity,TDbContext}.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Entity framework uzerinde calisan jenerik repository sinifi
    /// </summary>
    /// <typeparam name="TEntity">Repository'nin icerdigi veriye ait sinif tipi</typeparam>
    /// <typeparam name="TDbContext">Veri tipini iceren DbContext</typeparam>
    public class EfRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TDbContext : DbContext {
        // fields

        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        // constructors

        public EfRepository(TDbContext dbContext) {
            this._dbContext = dbContext;
            this._dbSet = this._dbContext.Set<TEntity>();
        }

        // methods

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate = null, Func<IRepositorySet<TEntity>, IRepositorySet<TEntity>> func = null) {
            var dbSet = this.GetDbSet(func);

            return dbSet.SingleOrDefault(predicate);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IRepositorySet<TEntity>, IRepositorySet<TEntity>> func = null) {
            var dbSet = this.GetDbSet(func);

            return await dbSet.SingleOrDefaultAsync(predicate);
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null, Func<IRepositorySet<TEntity>, IRepositorySet<TEntity>> func = null) {
            var dbSet = this.GetDbSet(func);

            if (predicate == null) {
                return dbSet.ToList();
            }

            return dbSet.Where(predicate).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IRepositorySet<TEntity>, IRepositorySet<TEntity>> func = null) {
            var dbSet = this.GetDbSet(func);

            if (predicate == null) {
                return await dbSet.ToListAsync();
            }

            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual void Add(TEntity entity) {
            // var addedEntity = this._dbContext.Entry(entity);
            // addedEntity.State = EntityState.Added;
            this._dbContext.Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity) {
            // var addedEntity = this._dbContext.Entry(entity);
            // addedEntity.State = EntityState.Added;
            await this._dbContext.AddAsync(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities) {
            // foreach (var entity in entities)
            // {
            //     var addedEntity = this._dbContext.Entry(entity);
            //     addedEntity.State = EntityState.Added;
            // }
            this._dbContext.AddRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) {
            // foreach (var entity in entities)
            // {
            //     var addedEntity = this._dbContext.Entry(entity);
            //     addedEntity.State = EntityState.Added;
            // }
            await this._dbContext.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity) {
            this._dbSet.Attach(entity);

            var entityToUpdate = this._dbContext.Entry(entity);
            entityToUpdate.State = EntityState.Modified;
        }

        public virtual async Task UpdateAsync(TEntity entity) {
            this._dbSet.Attach(entity);

            var entityToUpdate = this._dbContext.Entry(entity);
            entityToUpdate.State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities) {
            this._dbSet.AttachRange(entities);

            foreach (var entity in entities) {
                var entityToUpdate = this._dbContext.Entry(entity);
                entityToUpdate.State = EntityState.Modified;
            }
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities) {
            this._dbSet.AttachRange(entities);

            foreach (var entity in entities) {
                var entityToUpdate = this._dbContext.Entry(entity);
                entityToUpdate.State = EntityState.Modified;
            }
        }

        public virtual void Delete(TEntity entity) {
            var entityToDelete = this._dbContext.Entry(entity);

            if (entityToDelete.State == EntityState.Detached) {
                this._dbSet.Attach(entity);
            }

            // entityToDelete.State = EntityState.Deleted;
            this._dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity) {
            var entityToDelete = this._dbContext.Entry(entity);

            if (entityToDelete.State == EntityState.Detached) {
                this._dbSet.Attach(entity);
            }

            // entityToDelete.State = EntityState.Deleted;
            this._dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities) {
            foreach (var entity in entities) {
                var entityToDelete = this._dbContext.Entry(entity);

                if (entityToDelete.State == EntityState.Detached) {
                    this._dbSet.Attach(entity);
                }

                // entityToDelete.State = EntityState.Deleted;
            }

            this._dbSet.RemoveRange(entities);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities) {
            foreach (var entity in entities) {
                var entityToDelete = this._dbContext.Entry(entity);

                if (entityToDelete.State == EntityState.Detached) {
                    this._dbSet.Attach(entity);
                }

                // entityToDelete.State = EntityState.Deleted;
            }

            this._dbSet.RemoveRange(entities);
        }

        //public virtual IQueryable<TEntity> Query(string query, params object[] parameters)
        //{
        //    return this._dbSet.SqlQuery(query, parameters).AsQueryable();
        //}

        //public virtual Task<IQueryable<TEntity>> QueryAsync(string query, params object[] parameters)
        //{
        //    return (await this._dbSet.SqlQueryAsync(query, parameters)).AsQueryable();
        //}

        protected IQueryable<TEntity> GetDbSet(Func<IRepositorySet<TEntity>, IRepositorySet<TEntity>> func = null) {
            if (func != null) {
                var dbSet = this._dbContext.Set<TEntity>();
                var repositorySet = new EfRepositorySet<TEntity>(dbSet);

                return func(repositorySet).AsQueryable();
            }

            return this._dbSet;
        }
    }
}
