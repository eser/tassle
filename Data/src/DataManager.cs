// --------------------------------------------------------------------------
// <copyright file="DataManager.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
