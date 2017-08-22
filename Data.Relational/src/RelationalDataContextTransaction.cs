// --------------------------------------------------------------------------
// <copyright file="RelationalDataContextTransaction.cs" company="-">
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
