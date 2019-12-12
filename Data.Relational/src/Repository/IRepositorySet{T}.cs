// --------------------------------------------------------------------------
// <copyright file="IRepositorySet{T}.cs" company="-">
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
using System.Linq;
using System.Linq.Expressions;

namespace Tassle.Data {
    /// <summary>
    /// Jenerik repository tanimlari icin kullanilan interface
    /// </summary>
    public interface IRepositorySet<T>
        where T : class, IEntity {
        IRepositorySet<T, IEnumerable<TProperty>> Include<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> navigationProperty);

        IRepositorySet<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationProperty);

        IQueryable<T> AsQueryable();
    }
}
