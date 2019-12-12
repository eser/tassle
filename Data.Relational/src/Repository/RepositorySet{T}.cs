// --------------------------------------------------------------------------
// <copyright file="RepositorySet{T}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tassle.Data {
    /// <summary>
    /// Repository set sinifi
    /// </summary>
    public class RepositorySet<T> : IRepositorySet<T>
        where T : class, IEntity {
        // fields

        private readonly IQueryable<T> dbSet;

        // constructors

        public RepositorySet(IQueryable<T> dbSet) {
            this.dbSet = dbSet;
        }

        // properties

        internal IQueryable<T> DbSet {
            get => this.dbSet;
        }

        // methods

        public IRepositorySet<T, IEnumerable<TProperty>> Include<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty) {
            var newDbSet = this.dbSet.Include(navigationProperty);

            return new RepositorySet<T, IEnumerable<TProperty>>(newDbSet);
        }

        public IRepositorySet<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationProperty) {
            var newDbSet = this.dbSet.Include(navigationProperty);

            return new RepositorySet<T, TProperty>(newDbSet);
        }

        public IQueryable<T> AsQueryable() {
            return this.dbSet;
        }
    }
}
