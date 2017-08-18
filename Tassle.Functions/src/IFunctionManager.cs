// --------------------------------------------------------------------------
// <copyright file="IFunctionManager.cs" company="-">
// Copyright (c) 2008-2017 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Tassle.Functions {
    /// <summary>
    /// IFunctionManager interface.
    /// </summary>
    public interface IFunctionManager {
        Task Run(Func<Request, Response> target);
    }
}
