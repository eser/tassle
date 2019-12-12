// --------------------------------------------------------------------------
// <copyright file="IDataContext.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    public interface IDataContext {
        // properties

        object ContextObject { get; }

        // methods

        IDataContext NewContext();

        void EnsureCreated();

        Task EnsureCreatedAsync(CancellationToken token = default(CancellationToken));

        void EnsureDeleted();

        Task EnsureDeletedAsync(CancellationToken token = default(CancellationToken));

        IDataContextTransaction BeginTransaction();

        Task<IDataContextTransaction> BeginTransactionAsync(CancellationToken token = default(CancellationToken));

        void UseTransaction(IDataContextTransaction transaction);

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken token = default(CancellationToken));
    }
}
