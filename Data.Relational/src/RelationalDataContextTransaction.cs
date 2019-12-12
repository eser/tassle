// --------------------------------------------------------------------------
// <copyright file="RelationalDataContextTransaction.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Tassle.Data {
    public class RelationalDataContextTransaction : IDataContextTransaction {
        // fields

        private readonly IDbContextTransaction transaction;
        private volatile bool isDisposed;

        // constructors

        public RelationalDataContextTransaction(IDbContextTransaction transaction) {
            this.transaction = transaction;
            this.isDisposed = false;
        }

        ~RelationalDataContextTransaction() {
            this.Dispose(false);
        }

        // properties

        public object TransactionObject {
            get => this.transaction;
        }

        // methods

        public void Commit() {
            this.CheckDisposed();

            this.transaction.Commit();
        }

        public void Rollback() {
            this.CheckDisposed();

            this.transaction.Rollback();
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void CheckDisposed() {
            if (this.isDisposed) {
                throw new ObjectDisposedException("The RelationalDataContextTransaction is already disposed and cannot be used anymore.");
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (this.isDisposed) {
                return;
            }

            this.isDisposed = true;

            if (disposing) {
                this.transaction.Dispose();
            }
        }
    }
}
