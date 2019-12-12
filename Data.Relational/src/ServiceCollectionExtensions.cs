// --------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
