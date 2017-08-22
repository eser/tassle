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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Tassle.Data {
    /// <summary>
    /// Dependency injection containera extension olarak eklenecek metodlarin yer aldigi extension sinifi
    /// </summary>
    public static class ServiceCollectionExtensions {
        public static void AddRelationalData(this IServiceCollection serviceCollection) {
            serviceCollection.AddData();

            serviceCollection.TryAdd(ServiceDescriptor.Transient(typeof(IRepository<>), typeof(GenericRepository<>)));
        }

        public static void AddRelationalData<TDbContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext {
            serviceCollection.AddRelationalData();

            serviceCollection.AddScoped<IDataContext, RelationalDataContext<TDbContext>>();
            serviceCollection.AddScoped<IRelationalDataContext, RelationalDataContext<TDbContext>>();

            serviceCollection.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);
        }
    }
}
