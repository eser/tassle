// --------------------------------------------------------------------------
// <copyright file="IRelationalDataContext.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tassle.Data {
    public interface IRelationalDataContext : IDataContext {
        ContextEntityState GetEntryState<TEntity>(TEntity entity)
            where TEntity : class;

        void SetEntryState<TEntity>(TEntity entity, ContextEntityState state)
            where TEntity : class;
    }
}
