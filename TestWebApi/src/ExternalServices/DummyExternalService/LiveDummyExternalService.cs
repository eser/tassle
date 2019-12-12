// --------------------------------------------------------------------------
// <copyright file="LiveDummyExternalService.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

namespace Tassle.TestWebApi {
    public class LiveDummyExternalService : IDummyExternalService {
        public decimal Calculate() {
            return 5m + 3.1m;
        }
    }
}
