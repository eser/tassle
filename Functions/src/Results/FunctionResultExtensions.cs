// --------------------------------------------------------------------------
// <copyright file="FunctionResultExtensions.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

namespace Tassle.Functions {
    /// <summary>
    /// Request class.
    /// </summary>
    public static class FunctionResultExtensions {
        public static Response Ok(this IFunction function) {
            return new Response() { Result = FunctionResultType.Success };
        }

        public static Response Ok<T>(this IFunction function, T value) {
            return new Response<T>() { Result = FunctionResultType.Success, Value = value };
        }

        public static Response Reject(this IFunction function) {
            return new Response() { Result = FunctionResultType.Rejected };
        }

        public static Response Reject<T>(this IFunction function, T value) {
            return new Response<T>() { Result = FunctionResultType.Rejected, Value = value };
        }

        public static Response Fail(this IFunction function) {
            return new Response() { Result = FunctionResultType.Failed };
        }

        public static Response Fail<T>(this IFunction function, T value) {
            return new Response<T>() { Result = FunctionResultType.Failed, Value = value };
        }
    }
}
