// --------------------------------------------------------------------------
// <copyright file="EfUnitOfWork{T}.cs" company="-">
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
using System.Threading;
using System.Threading.Tasks;

namespace Tassle.Data {
    /// <summary>
    /// Entity framework uzerinde kullanilabilir unit of work sinifi
    /// </summary>
    /// <typeparam name="T">DbContext</typeparam>
    public class EfUnitOfWork<T> : IUnitOfWork
        where T : DbContext {
        // fields

        private readonly IServiceProvider _serviceProvider;
        private T _dbContext;
        private IUnitOfWorkTransaction _transaction;
        private bool _isSaveChangesCalled;
        private bool _isDisposed;

        // constructors

        internal EfUnitOfWork(IServiceProvider serviceProvider, T dbContext) {
            this._serviceProvider = serviceProvider;
            this._dbContext = dbContext;
            this._isSaveChangesCalled = false;
            this._isDisposed = false;
        }

        ~EfUnitOfWork() {
            this.Dispose(false);
        }

        // properties

        public T DbContext {
            get => this._dbContext;
        }

        public IUnitOfWorkTransaction Transaction {
            get => this._transaction;
        }

        // methods

        internal void BeginTransaction() {
            this.CheckDisposed();

            var transaction = this._dbContext.Database.BeginTransaction();

            this._transaction = new EfUnitOfWorkTransaction(transaction);
        }

        internal async Task BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            this.CheckDisposed();

            var transaction = await this._dbContext.Database.BeginTransactionAsync(cancellationToken);

            this._transaction = new EfUnitOfWorkTransaction(transaction);
        }

        public int SaveChanges(bool? acceptAllChangesOnSuccess = null) {
            this.CheckDisposed();
            this.CheckSaveChangesCalledPreviously();

            var result = acceptAllChangesOnSuccess.HasValue ?
                this._dbContext.SaveChanges(acceptAllChangesOnSuccess.Value) :
                this._dbContext.SaveChanges();

            this._isSaveChangesCalled = true;

            return result;
        }

        public async Task<int> SaveChangesAsync(bool? acceptAllChangesOnSuccess = null, CancellationToken cancellationToken = default(CancellationToken)) {
            this.CheckDisposed();
            this.CheckSaveChangesCalledPreviously();

            var result = acceptAllChangesOnSuccess.HasValue ?
                await this._dbContext.SaveChangesAsync(acceptAllChangesOnSuccess.Value, cancellationToken) :
                await this._dbContext.SaveChangesAsync(cancellationToken);

            if (this._transaction != null) {
                this._transaction.Commit();
            }

            this._isSaveChangesCalled = true;

            return result;
        }

        public IRepository<TEntity> GetGenericRepository<TEntity>()
            where TEntity : class, IEntity, new() {
            return new EfRepository<TEntity, T>(this._dbContext);
        }

        public TRepository GetRepository<TRepository>()
            where TRepository : class {
            return this._serviceProvider.GetService(typeof(TRepository)) as TRepository;
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void CheckDisposed() {
            if (this._isDisposed) {
                throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
            }
        }

        protected void CheckSaveChangesCalledPreviously() {
            if (this._isSaveChangesCalled) {
                throw new ObjectDisposedException("The UnitOfWork is already called save changes method and cannot be used anymore.");
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!this._isDisposed) {
                if (disposing) {
                    if (this._transaction != null) {
                        if (!this._isSaveChangesCalled) {
                            this._transaction.Rollback();
                        }

                        this._transaction.Dispose();
                        this._transaction = null;
                    }

                    if (this._dbContext != null) {
                        // FIXME: could be a shared DbContext
                        //this._dbContext.Dispose();
                        this._dbContext = null;
                    }
                }
            }

            this._isDisposed = true;
        }
    }
}
