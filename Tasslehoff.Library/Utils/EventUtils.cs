// -----------------------------------------------------------------------
// <copyright file="EventUtils.cs" company="-">
// Copyright (c) 2013 larukedi (eser@sent.com). All rights reserved.
// </copyright>
// <author>larukedi (http://github.com/larukedi/)</author>
// -----------------------------------------------------------------------

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

namespace Tasslehoff.Library.Utils
{
    using System;
    using System.Reflection;

    /// <summary>
    /// EventUtils class.
    /// </summary>
    public static class EventUtils
    {
        // methods

        /// <summary>
        /// Events the once.
        /// </summary>
        /// <param name="eventObject">The event object</param>
        /// <param name="eventMember">The event member</param>
        /// <param name="handler">The handler</param>
        public static void EventOnce(object eventObject, string eventMember, EventHandler handler)
        {
            EventInfo eventInfo = eventObject.GetType().GetEvent(eventMember);
            EventHandler tempEventHandler = null;

            tempEventHandler = (sender, e) =>
            {
                handler.Invoke(null, new EventArgs());
                eventInfo.RemoveEventHandler(eventObject, tempEventHandler);
            };
            eventInfo.AddEventHandler(eventObject, tempEventHandler);
        }
    }
}
