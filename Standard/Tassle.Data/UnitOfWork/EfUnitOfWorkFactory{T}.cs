// --------------------------------------------------------------------------
// <copyright file="EfUnitOfWorkFactory{T}.cs" company="-">
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

namespace Tassle.Data.UnitOfWork {
    /// <summary>
    /// Entity framework uzerinde kullanilabilir unit of work sinif factorysi
    /// </summary>
    /// <typeparam name="T">DbContext</typeparam>
    public class EfUnitOfWorkFactory<T> : IUnitOfWorkFactory
        where T : DbContext {
        // fields

        private readonly IServiceProvider _serviceProvider;

        // constructors

        public EfUnitOfWorkFactory(IServiceProvider serviceProvider) {
            this._serviceProvider = serviceProvider;
        }

        // methods

        public IUnitOfWork Create(ScopeType scopeType = ScopeType.Default) {
            var dbContext = this._serviceProvider.GetService(typeof(T)) as T;

            return this.Create(dbContext, scopeType);
        }

        public async Task<IUnitOfWork> CreateAsync(ScopeType scopeType = ScopeType.Default) {
            var dbContext = this._serviceProvider.GetService(typeof(T)) as T;

            return await this.CreateAsync(dbContext, scopeType);
        }

        public IUnitOfWork Create(T dbContext, ScopeType scopeType = ScopeType.Default) {
            var unitOfWork = new EfUnitOfWork<T>(this._serviceProvider, dbContext);

            if (scopeType == ScopeType.Transactional) {
                unitOfWork.BeginTransaction();
            }

            return unitOfWork;
        }

        public async Task<IUnitOfWork> CreateAsync(T dbContext, ScopeType scopeType = ScopeType.Default, CancellationToken cancellationToken = default(CancellationToken)) {
            var unitOfWork = new EfUnitOfWork<T>(this._serviceProvider, dbContext);

            if (scopeType == ScopeType.Transactional) {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
            }

            return unitOfWork;
        }
    }
}
