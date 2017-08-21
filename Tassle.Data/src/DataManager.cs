// --------------------------------------------------------------------------
// <copyright file="DataManager.cs" company="-">
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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tassle.Data {
    public class DataManager : IDataManager {
        // fields

        private readonly IServiceProvider serviceProvider;
        private readonly IList<IUnitOfWork> unitOfWorks;

        // constructors

        public DataManager(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
            this.unitOfWorks = new List<IUnitOfWork>();
        }

        // methods

        public IUnitOfWork CreateUnitOfWork(ScopeType scopeType = ScopeType.Default) {
            // FIXME scope type?
            return this.serviceProvider.GetService<IUnitOfWork>();
        }

        public void PushUnitOfWork(IUnitOfWork unitOfWork) {
            this.unitOfWorks.Add(unitOfWork);
        }

        public void PopUnitOfWork(IUnitOfWork unitOfWork) {
            var last = this.PeekUnitOfWork();

            if (last != unitOfWork) {
                throw new InvalidOperationException("The referenced unit of work object is not the latest one in the stack.");
            }

            this.unitOfWorks.Remove(unitOfWork);
        }

        public IUnitOfWork PeekUnitOfWork() {
            return this.unitOfWorks.LastOrDefault();
        }

        public T GetRepository<T>()
            where T : class, IRepository {
            return this.serviceProvider.GetService<T>();
        }
    }
}
