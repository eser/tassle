// --------------------------------------------------------------------------
// <copyright file="NetHelpers.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;

namespace Tassle.Helpers {
    /// <summary>
    /// NetUtils class.
    /// </summary>
    public static class NetHelpers {
        // methods

        /// <summary>
        /// Converts to IP range.
        /// </summary>
        /// <param name="ipAddress">The IP address</param>
        /// <returns>The IP range</returns>
        public static string ConvertToIPRange(string ipAddress) {
            string[] ipArray = ipAddress.Split('.');
            double ipRange = 0;

            for (var i = 0; i < 4; i++) {
                var numPosition = int.Parse(ipArray[3 - i].ToString());

                if (i == 4) {
                    ipRange += numPosition;
                }
                else {
                    ipRange += (numPosition % 256) * Math.Pow(256, i);
                }
            }

            return ipRange.ToString();
        }
    }
}
