// --------------------------------------------------------------------------
// <copyright file="IRepository{T}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Jenerik repository tanimlari icin kullanilan interface
    /// </summary>
    public interface IRepository<T> : IRepository
        where T : class, IEntity, new() {
        // properties

        IRelationalDataContext DataContext { get; }

        // methods
        T GetSingle(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false, CancellationToken token = default(CancellationToken));

        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false);

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null, bool dontTrack = false, CancellationToken token = default(CancellationToken));

        void Add(T entity);

        Task AddAsync(T entity, CancellationToken token = default(CancellationToken));

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken));

        void Update(T entity);

        Task UpdateAsync(T entity, CancellationToken token = default(CancellationToken));

        void UpdateRange(IEnumerable<T> entities);

        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken));

        void UpdateAll(T entity);

        Task UpdateAllAsync(T entity, CancellationToken token = default(CancellationToken));

        void UpdateAllRange(IEnumerable<T> entities);

        Task UpdateAllRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken));

        void Delete(T entity);

        Task DeleteAsync(T entity, CancellationToken token = default(CancellationToken));

        void DeleteRange(IEnumerable<T> entities);

        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken token = default(CancellationToken));

        IRepository<T> WithDataContext(IRelationalDataContext dataContext);
    }
}
