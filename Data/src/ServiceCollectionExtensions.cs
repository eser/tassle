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

using Microsoft.Extensions.DependencyInjection;

namespace Tassle.Data {
    /// <summary>
    /// Dependency injection containera extension olarak eklenecek metodlarin yer aldigi extension sinifi
    /// </summary>
    public static class ServiceCollectionExtensions {
        public static void AddData(this IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton<IDataManager, DataManager>();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
