// --------------------------------------------------------------------------
// <copyright file="ValidationPropertyRuleBuilderExtensions.cs" company="-">
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
using System.Text.RegularExpressions;

namespace Tassle.Validation {
    public static class ValidationPropertyRuleBuilderExtensions {
        // any
        public static IValidationPropertyRuleBuilder<T, TProperty> NotNull<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance) {
            return instance.Must(x => x != null, "not null");
        }

        public static IValidationPropertyRuleBuilder<T, TProperty> Null<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance) {
            return instance.Must(x => x == null, "null");
        }

        public static IValidationPropertyRuleBuilder<T, TProperty> NotEqual<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance, TProperty value) {
            return instance.Must(x => !x.Equals(value), $"not equal to {value}");
        }

        public static IValidationPropertyRuleBuilder<T, TProperty> Equal<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance, TProperty value) {
            return instance.Must(x => x.Equals(value), $"equal to {value}");
        }

        // string
        public static IValidationPropertyRuleBuilder<T, string> NotEmpty<T>(this IValidationPropertyRuleBuilder<T, string> instance) {
            return instance.Must(x => x.Length > 0, "not empty");
        }

        public static IValidationPropertyRuleBuilder<T, string> Empty<T>(this IValidationPropertyRuleBuilder<T, string> instance) {
            return instance.Must(x => x.Length == 0, "empty");
        }

        public static IValidationPropertyRuleBuilder<T, string> MinimumLength<T>(this IValidationPropertyRuleBuilder<T, string> instance, int minimumLength) {
            return instance.Must(x => x.Length >= minimumLength, $"longer than {minimumLength} characters");
        }

        public static IValidationPropertyRuleBuilder<T, string> MaximumLength<T>(this IValidationPropertyRuleBuilder<T, string> instance, int maximumLength) {
            return instance.Must(x => x.Length <= maximumLength, $"shorter than {maximumLength} characters");
        }

        public static IValidationPropertyRuleBuilder<T, string> Matches<T>(this IValidationPropertyRuleBuilder<T, string> instance, string pattern) {
            return instance.Must(x => Regex.Match(x, pattern).Success, $"matches {pattern} regex pattern");
        }

        // IComparable
        public static IValidationPropertyRuleBuilder<T, TProperty> LessThan<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance, TProperty value)
            where TProperty : IComparable {
            return instance.Must(x => x.CompareTo(value) == -1, $"less than {value}");
        }

        public static IValidationPropertyRuleBuilder<T, TProperty> LessThanOrEqual<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance, TProperty value)
            where TProperty : IComparable {
            return instance.Must(x => x.CompareTo(value) != 1, $"less than or equal {value}");
        }

        public static IValidationPropertyRuleBuilder<T, TProperty> GreaterThan<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance, TProperty value)
            where TProperty : IComparable {
            return instance.Must(x => x.CompareTo(value) == 1, $"greater than {value}");
        }

        public static IValidationPropertyRuleBuilder<T, TProperty> GreaterThanOrEqual<T, TProperty>(this IValidationPropertyRuleBuilder<T, TProperty> instance, TProperty value)
            where TProperty : IComparable {
            return instance.Must(x => x.CompareTo(value) != -1, $"greater than or equal {value}");
        }
    }
}
