// --------------------------------------------------------------------------
// <copyright file="RepositorySet{TEntity,TProperty}.cs" company="-">
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
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq.Expressions;

namespace Tassle.Data {
    /// <summary>
    /// Repository set sinifi
    /// </summary>
    public class RepositorySet<TEntity, TProperty> : RepositorySet<TEntity>, IRepositorySet<TEntity, TProperty>
        where TEntity : class, IEntity {
        // fields

        private readonly IIncludableQueryable<TEntity, TProperty> dbSet;

        // constructors

        public RepositorySet(IIncludableQueryable<TEntity, TProperty> dbSet)
            : base(dbSet) {
            this.dbSet = dbSet;
        }

        // properties

        internal IIncludableQueryable<TEntity, TProperty> DbSetReference {
            get => this.dbSet;
        }

        // methods

        public IRepositorySet<TEntity, TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, TNextProperty>> navigationPropertyPath) {
            var newDbSet = EntityFrameworkQueryableExtensions.ThenInclude(this.DbSetReference, navigationPropertyPath);

            return new RepositorySet<TEntity, TNextProperty>(newDbSet);
        }
    }
}
