// --------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="-">
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Unit of work implementasyonlarinin kullanacagi interface
    /// </summary>
    public class UnitOfWork : IUnitOfWork {
        // fields

        private readonly IDataManager dataManager;
        private ScopeType scopeType;
        private IDictionary<Type, IDataContext> dataContexts;
        private bool isSaveChangesCalled;
        private volatile bool isDisposed;

        // constructors

        public UnitOfWork(IDataManager dataManager, ScopeType scopeType = ScopeType.Default) {
            this.dataManager = dataManager;
            this.scopeType = scopeType;
            this.dataContexts = new Dictionary<Type, IDataContext>();
            this.isSaveChangesCalled = false;
            this.isDisposed = false;

            this.dataManager.PushUnitOfWork(this);
        }

        ~UnitOfWork() {
            this.Dispose(false);
        }

        // properties

        public ScopeType ScopeType {
            get => this.scopeType;
            set => this.scopeType = value;
        }

        // methods

        public IDataContext GetDataContext(IDataContext dataContext) {
            var dbContextType = dataContext.ContextObject.GetType();

            if (!this.dataContexts.ContainsKey(dbContextType)) {
                var newContext = dataContext.NewContext();

                this.dataContexts.Add(dbContextType, newContext);
            }

            return this.dataContexts[dbContextType];
        }

        public void SaveChanges() {
            this.CheckDisposed();
            this.CheckSaveChangesCalledPreviously();

            var transactions = new List<IDataContextTransaction>();

            if (this.scopeType == ScopeType.Transactional) {
                foreach (var dataContext in this.dataContexts.Values) {
                    var transaction = dataContext.BeginTransaction();

                    transactions.Add(transaction);
                }
            }

            foreach (var dbContext in this.dataContexts.Values) {
                dbContext.SaveChanges();
            }

            foreach (var transaction in transactions) {
                transaction.Commit();
            }

            // FIXME dispose transactions
            // FIXME try-catch rollback

            this.dataContexts.Clear();
            this.isSaveChangesCalled = true;
        }

        public async Task SaveChangesAsync(CancellationToken token = default(CancellationToken)) {
            this.CheckDisposed();
            this.CheckSaveChangesCalledPreviously();

            var transactions = new List<IDataContextTransaction>();

            if (this.scopeType == ScopeType.Transactional) {
                foreach (var dataContext in this.dataContexts.Values) {
                    var transaction = await dataContext.BeginTransactionAsync(token).ConfigureAwait(false);

                    transactions.Add(transaction);
                }
            }

            foreach (var dbContext in this.dataContexts.Values) {
                await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            }

            foreach (var transaction in transactions) {
                transaction.Commit();
            }

            // FIXME dispose transactions
            // FIXME try-catch rollback

            this.dataContexts.Clear();
            this.isSaveChangesCalled = true;
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void CheckDisposed() {
            if (this.isDisposed) {
                throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
            }
        }

        protected void CheckSaveChangesCalledPreviously() {
            if (this.isSaveChangesCalled) {
                throw new ObjectDisposedException("The UnitOfWork is already called save changes method and cannot be used anymore.");
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (this.isDisposed) {
                return;
            }

            this.isDisposed = true;

            if (disposing) {
                this.dataManager.PopUnitOfWork(this);

                this.dataContexts = null;
            }
        }
    }
}
