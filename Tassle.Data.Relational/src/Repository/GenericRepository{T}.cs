// --------------------------------------------------------------------------
// <copyright file="GenericRepository{T}.cs" company="-">
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
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Jenerik repository tanimlari icin kullanilan sinif
    /// </summary>
    public class GenericRepository<T> : IRepository<T>
        where T : class, IEntity, new() {
        // fields

        private readonly IDataManager dataManager;

        private readonly IRelationalDataContext dataContext;

        // constructors

        public GenericRepository(IDataManager dataManager, IRelationalDataContext dataContext) {
            this.dataManager = dataManager;
            this.dataContext = dataContext;
        }

        // properties

        public IRelationalDataContext DataContext {
            get {
                var activeDataContext = this.GetActiveDataContext();

                return activeDataContext.dataContext;
            }
        }

        // methods

        public virtual T GetSingle(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false) {
            var result = this.GetDbSetByQuery(func, dontTrack);

            return result.dbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false, CancellationToken token = default(CancellationToken)) {
            var result = this.GetDbSetByQuery(func, dontTrack);

            return await result.dbSet.FirstOrDefaultAsync(predicate, token).ConfigureAwait(false);
        }

        public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false) {
            var result = this.GetDbSetByQuery(func, dontTrack);

            if (predicate == null) {
                return result.dbSet.ToList();
            }

            return result.dbSet.Where(predicate).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false, CancellationToken token = default(CancellationToken)) {
            var result = this.GetDbSetByQuery(func, dontTrack);

            if (predicate == null) {
                return await result.dbSet.ToListAsync().ConfigureAwait(false);
            }

            return await result.dbSet.Where(predicate).ToListAsync(token).ConfigureAwait(false);
        }

        public virtual void Add(T entity) {
            var result = this.GetDbSet();

            result.dbSet.Add(entity);

            if (!result.isUow) {
                result.dataContext.SaveChanges();
            }
        }

        public virtual async Task AddAsync(T entity, CancellationToken token = default(CancellationToken)) {
            var result = this.GetDbSet();

            await result.dbSet.AddAsync(entity, token).ConfigureAwait(false);

            if (!result.isUow) {
                await result.dataContext.SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual void AddRange(IEnumerable<T> entities) {
            var result = this.GetDbSet();

            result.dbSet.AddRange(entities);

            if (!result.isUow) {
                result.dataContext.SaveChanges();
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken)) {
            var result = this.GetDbSet();

            await result.dbSet.AddRangeAsync(entities, token).ConfigureAwait(false);

            if (!result.isUow) {
                await result.dataContext.SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual void Update(T entity) {
            var result = this.GetDbSet();

            var state = result.dataContext.GetEntryState(entity);

            if (state != ContextEntityState.Added) {
                result.dbSet.Update(entity);
            }

            if (!result.isUow) {
                result.dataContext.SaveChanges();
            }
        }

        public virtual Task UpdateAsync(T entity, CancellationToken token = default(CancellationToken)) {
            return Task.Run(
                () => { this.Update(entity); },
                token);
        }

        public virtual void UpdateRange(IEnumerable<T> entities) {
            var result = this.GetDbSet();

            result.dbSet.UpdateRange(entities);

            if (!result.isUow) {
                result.dataContext.SaveChanges();
            }
        }

        public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken)) {
            return Task.Run(
                () => { this.UpdateRange(entities); },
                token);
        }

        public virtual void UpdateAll(T entity) {
            this.Update(entity);
        }

        public virtual Task UpdateAllAsync(T entity, CancellationToken token = default(CancellationToken)) {
            return Task.Run(
                () => { this.UpdateAll(entity); },
                token);
        }

        public virtual void UpdateAllRange(IEnumerable<T> entities) {
            this.UpdateRange(entities);
        }

        public virtual Task UpdateAllRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken)) {
            return Task.Run(
                () => { this.UpdateAllRange(entities); },
                token);
        }

        public virtual void Delete(T entity) {
            var result = this.GetDbSet();

            result.dbSet.Remove(entity);

            if (!result.isUow) {
                result.dataContext.SaveChanges();
            }
        }

        public virtual Task DeleteAsync(T entity, CancellationToken token = default(CancellationToken)) {
            return Task.Run(
                () => { this.Delete(entity); },
                token);
        }

        public virtual void DeleteRange(IEnumerable<T> entities) {
            var result = this.GetDbSet();

            result.dbSet.RemoveRange(entities);

            if (!result.isUow) {
                result.dataContext.SaveChanges();
            }
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken)) {
            return Task.Run(
                () => { this.DeleteRange(entities); },
                token);
        }

        public IRepository<T> WithDataContext(IRelationalDataContext dataContext) {
            return new GenericRepository<T>(this.dataManager, dataContext);
        }

        protected (IRelationalDataContext dataContext, bool isUow) GetActiveDataContext() {
            var uow = this.dataManager.PeekUnitOfWork();

            if (uow != null) {
                var currentDataContext = uow.GetDataContext(this.dataContext) as IRelationalDataContext;

                return (currentDataContext, true);
            }

            return (this.dataContext, false);
        }

        protected (IRelationalDataContext dataContext, bool isUow, DbContext dbContext) GetDbContext() {
            var result = this.GetActiveDataContext();

            var dbContext = result.dataContext.ContextObject as DbContext;

            return (result.dataContext, result.isUow, dbContext);
        }

        protected (IRelationalDataContext dataContext, bool isUow, DbSet<T> dbSet) GetDbSet() {
            var result = this.GetDbContext();

            var dbSet = result.dbContext.Set<T>();

            return (result.dataContext, result.isUow, dbSet);
        }

        protected (IRelationalDataContext dataContext, bool isUow, IQueryable<T> dbSet) GetDbSetByQuery(Func<IRepositorySet<T>, IRepositorySet<T>> func, bool dontTrack) {
            var result = this.GetDbSet();

            if (func == null) {
                return (result.dataContext, result.isUow, result.dbSet);
            }

            var repositorySet = new RepositorySet<T>(result.dbSet);
            var dbSet = func(repositorySet).AsQueryable();

            if (dontTrack) {
                dbSet = dbSet.AsNoTracking();
            }

            return (result.dataContext, result.isUow, dbSet);
        }
    }
}
