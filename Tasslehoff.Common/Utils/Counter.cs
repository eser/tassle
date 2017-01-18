// --------------------------------------------------------------------------
// <copyright file="Counter.cs" company="-">
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

namespace Tasslehoff.Common.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Counter class.
    /// </summary>
    public class Counter
    {
        // fields

        /// <summary>
        /// The counter stack
        /// </summary>
        private IDictionary<int, KeyValuePair<string, DateTimeOffset>> counterStack;

        /// <summary>
        /// The numerator
        /// </summary>
        private Numerator numerator;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class.
        /// </summary>
        public Counter()
        {
            this.counterStack = new Dictionary<int, KeyValuePair<string, DateTimeOffset>>();
            this.numerator = new Numerator();
        }

        // events

        /// <summary>
        /// Occurs when [on popped].
        /// </summary>
        public event EventHandler<CounterPoppedEventArgs> OnPopped;

        // methods

        /// <summary>
        /// Pushes this instance.
        /// </summary>
        /// <returns>
        /// The number
        /// </returns>
        public int Push()
        {
            return this.Push(null, DateTimeOffset.UtcNow);
        }

        /// <summary>
        /// Pushes the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>
        /// The number
        /// </returns>
        public int Push(string key)
        {
            return this.Push(key, DateTimeOffset.UtcNow);
        }

        /// <summary>
        /// Pushes the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="dateTime">The date time</param>
        /// <returns>
        /// The number
        /// </returns>
        public int Push(string key, DateTimeOffset dateTime)
        {
            int number = this.numerator.Get();
            this.counterStack.Add(number, new KeyValuePair<string, DateTimeOffset>(key, dateTime));

            return number;
        }

        /// <summary>
        /// Pops this instance.
        /// </summary>
        /// <param name="number">The number</param>
        /// <returns>
        /// The period.
        /// </returns>
        public TimeSpan Pop(int number)
        {
            KeyValuePair<string, DateTimeOffset> pop = this.counterStack[number];
            this.counterStack.Remove(number);

            TimeSpan period = DateTimeOffset.UtcNow.Subtract(pop.Value);

            if (this.OnPopped != null)
            {
                CounterPoppedEventArgs e = new CounterPoppedEventArgs(pop.Key, period);
                this.OnPopped(this, e);
            }

            return period;
        }
    }
}