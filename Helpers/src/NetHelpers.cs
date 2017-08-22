// --------------------------------------------------------------------------
// <copyright file="NetHelpers.cs" company="-">
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
