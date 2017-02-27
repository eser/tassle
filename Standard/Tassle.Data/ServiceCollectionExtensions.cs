// --------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="-">
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

using Tassle.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tassle.Data {
    /// <summary>
    /// Dependency injection containera extension olarak eklenecek metodlarin yer aldigi extension sinifi
    /// </summary>
    public static class ServiceCollectionExtensions {
        public static void AddUnitOfWorkForEntityFramework<T>(this IServiceCollection services)
            where T : DbContext {
            services.AddSingleton<IUnitOfWorkFactory, EfUnitOfWorkFactory<T>>();
        }
    }
}
