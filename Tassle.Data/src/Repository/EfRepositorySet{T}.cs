// --------------------------------------------------------------------------
// <copyright file="EfRepositorySet{T}.cs" company="-">
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

namespace Tassle.Data {
    public class EfRepositorySet<T> : IRepositorySet<T>
        where T : class, IEntity {
        // fields

        private IQueryable<T> dbSet;

        // constructors

        public EfRepositorySet(IQueryable<T> dbSet) {
            this.dbSet = dbSet;
        }

        // properties

        internal IQueryable<T> DbSet {
            get => this.dbSet;
        }

        // methods

        public IRepositorySet<T, IEnumerable<TProperty>> Include<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty) {
            var newDbSet = this.dbSet.Include(navigationProperty);

            return new EfRepositorySet<T, IEnumerable<TProperty>>(newDbSet);
        }

        public IRepositorySet<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationProperty) {
            var newDbSet = this.dbSet.Include(navigationProperty);

            return new EfRepositorySet<T, TProperty>(newDbSet);
        }

        public IQueryable<T> AsQueryable() {
            return this.dbSet;
        }
    }
}
