// --------------------------------------------------------------------------
// <copyright file="IEntityHasId{T}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

namespace Tassle.Data {
    /// <summary>
    /// Id property'si iceren entity tanimlarina ait interface
    /// </summary>
    /// <typeparam name="T">Id property'sinin tipi</typeparam>
    public interface IEntityHasId<T> : IEntity {
        T Id { get; set; }
    }
}
