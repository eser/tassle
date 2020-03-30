// --------------------------------------------------------------------------
// <copyright file="EventHelpers.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Reflection;

namespace Tassle {
    /// <summary>
    /// EventUtils class.
    /// </summary>
    public static class EventHelpers {
        // methods

        /// <summary>
        /// Events the once.
        /// </summary>
        /// <param name="eventObject">The event object</param>
        /// <param name="eventMember">The event member</param>
        /// <param name="handler">The handler</param>
        public static void EventOnce(object eventObject, string eventMember, EventHandler handler) {
            EventInfo eventInfo = eventObject.GetType().GetTypeInfo().GetEvent(eventMember);
            EventHandler tempEventHandler = null;

            tempEventHandler = (sender, e) => {
                handler.Invoke(null, new EventArgs());
                eventInfo.RemoveEventHandler(eventObject, tempEventHandler);
            };
            eventInfo.AddEventHandler(eventObject, tempEventHandler);
        }
    }
}
