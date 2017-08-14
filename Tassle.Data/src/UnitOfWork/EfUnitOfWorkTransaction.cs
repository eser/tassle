// --------------------------------------------------------------------------
// <copyright file="EfUnitOfWorkTransaction.cs" company="-">
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
    /// <summary>
    /// Entity framework uzerinde kullanilabilir unit of work siniflarinin olusturdugu
    /// transaction scopelara ait sinif tanimi
    /// </summary>
    public class EfUnitOfWorkTransaction : IUnitOfWorkTransaction {
        // fields

        private IDbContextTransaction _transaction;
        private bool _isDisposed;

        // constructors

        public EfUnitOfWorkTransaction(IDbContextTransaction transaction) {
            this._transaction = transaction;
            this._isDisposed = false;
        }

        ~EfUnitOfWorkTransaction() {
            this.Dispose(false);
        }

        // methods

        public void Commit() {
            this.CheckDisposed();

            this._transaction.Commit();
        }

        public void Rollback() {
            this.CheckDisposed();

            this._transaction.Rollback();
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void CheckDisposed() {
            if (this._isDisposed) {
                throw new ObjectDisposedException("The UnitOfWorkTransaction is already disposed and cannot be used anymore.");
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!this._isDisposed) {
                if (disposing) {
                    if (this._transaction != null) {
                        this._transaction.Dispose();
                        this._transaction = null;
                    }
                }
            }

            this._isDisposed = true;
        }
    }
}
