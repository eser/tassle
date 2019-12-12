// --------------------------------------------------------------------------
// <copyright file="IRepositoryExtensions.cs" company="-">
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

namespace Tassle.Data {
    /// <summary>
    /// Jenerik repository tanimlari icin kullanilan interface
    /// </summary>
    public static class IRepositoryExtensions {
        public static IRepositorySet<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IRepositorySet<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class, IEntity {
            return source.ThenInclude(navigationPropertyPath);
        }

        public static IRepositorySet<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IRepositorySet<TEntity, ICollection<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class, IEntity {
            return source.ThenInclude(navigationPropertyPath);
        }
    }
}
