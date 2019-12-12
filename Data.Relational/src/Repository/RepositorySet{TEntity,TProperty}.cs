// --------------------------------------------------------------------------
// <copyright file="RepositorySet{TEntity,TProperty}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
