// --------------------------------------------------------------------------
// <copyright file="IRepository{T}.cs" company="-">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Jenerik repository tanimlari icin kullanilan interface
    /// </summary>
    /// <typeparam name="T">Repositorynin icerdigi veri sinif tipi</typeparam>
    public interface IRepository<T>
        where T : class, IEntity, new() {
        T Get(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null);

        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null);

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IRepositorySet<T>, IRepositorySet<T>> func = null);

        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        Task UpdateAsync(T entity);

        void UpdateRange(IEnumerable<T> entities);

        Task UpdateRangeAsync(IEnumerable<T> entities);

        void Delete(T entity);

        Task DeleteAsync(T entity);

        void DeleteRange(IEnumerable<T> entities);

        Task DeleteRangeAsync(IEnumerable<T> entities);

        // IQueryable<T> Query(string query, params object[] parameters);

        // Task<IQueryable<T>> QueryAsync(string query, params object[] parameters);
    }
}
