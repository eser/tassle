// --------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Unit of work implementasyonlarinin kullanacagi interface
    /// </summary>
    public interface IUnitOfWork : IDisposable {
        // properties

        ScopeType ScopeType { get; set; }

        // methods

        IDataContext GetDataContext(IDataContext dataContext);

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken token = default(CancellationToken));
    }
}
