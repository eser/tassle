// --------------------------------------------------------------------------
// <copyright file="RelationalDataContext.cs" company="-">
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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    public class RelationalDataContext<T> : IRelationalDataContext
        where T : DbContext {
        // fields

        private readonly IServiceProvider serviceProvider;
        private readonly T dbContext;

        // constructors

        public RelationalDataContext(IServiceProvider serviceProvider, T dbContext) {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
        }

        // properties

        public object ContextObject {
            get => this.dbContext;
        }

        // methods

        public IDataContext NewContext() {
            var newContext = this.serviceProvider.GetService<T>();
            return new RelationalDataContext<T>(this.serviceProvider, newContext);
        }

        public void EnsureCreated() {
            this.dbContext.Database.EnsureCreated();
        }

        public async Task EnsureCreatedAsync(CancellationToken token = default(CancellationToken)) {
            await this.dbContext.Database.EnsureCreatedAsync(token).ConfigureAwait(false);
        }

        public void EnsureDeleted() {
            this.dbContext.Database.EnsureDeleted();
        }

        public async Task EnsureDeletedAsync(CancellationToken token = default(CancellationToken)) {
            await this.dbContext.Database.EnsureDeletedAsync(token).ConfigureAwait(false);
        }

        public IDataContextTransaction BeginTransaction() {
            var transaction = this.dbContext.Database.BeginTransaction();

            return new RelationalDataContextTransaction(transaction);
        }

        public async Task<IDataContextTransaction> BeginTransactionAsync(CancellationToken token = default(CancellationToken)) {
            var transaction = await this.dbContext.Database.BeginTransactionAsync(token).ConfigureAwait(false);

            return new RelationalDataContextTransaction(transaction);
        }

        public void UseTransaction(IDataContextTransaction transaction) {
            var dbContextTransaction = transaction.TransactionObject as IDbContextTransaction;
            var dbTransaction = dbContextTransaction.GetDbTransaction();

            this.dbContext.Database.UseTransaction(dbTransaction);
        }

        public ContextEntityState GetEntryState<TEntity>(TEntity entity)
            where TEntity : class {
            var entityState = this.dbContext.Entry(entity).State;

            if (entityState == EntityState.Detached) {
                return ContextEntityState.Detached;
            }

            if (entityState == EntityState.Unchanged) {
                return ContextEntityState.Unchanged;
            }

            if (entityState == EntityState.Deleted) {
                return ContextEntityState.Deleted;
            }

            if (entityState == EntityState.Modified) {
                return ContextEntityState.Modified;
            }

            if (entityState == EntityState.Added) {
                return ContextEntityState.Added;
            }

            return ContextEntityState.Unknown;
        }

        public void SetEntryState<TEntity>(TEntity entity, ContextEntityState state)
            where TEntity : class {
            var entry = this.dbContext.Entry(entity);

            if (state == ContextEntityState.Detached) {
                entry.State = EntityState.Detached;
            }

            if (state == ContextEntityState.Unchanged) {
                entry.State = EntityState.Unchanged;
            }

            if (state == ContextEntityState.Deleted) {
                entry.State = EntityState.Deleted;
            }

            if (state == ContextEntityState.Modified) {
                entry.State = EntityState.Modified;
            }

            if (state == ContextEntityState.Added) {
                entry.State = EntityState.Added;
            }
        }

        public void SaveChanges() {
            if (!this.dbContext.ChangeTracker.HasChanges()) {
                return;
            }

            this.dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken token = default(CancellationToken)) {
            if (!this.dbContext.ChangeTracker.HasChanges()) {
                return;
            }

            await this.dbContext.SaveChangesAsync(token).ConfigureAwait(false);
        }
    }
}
