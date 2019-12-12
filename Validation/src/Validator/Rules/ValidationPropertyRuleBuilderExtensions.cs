// --------------------------------------------------------------------------
// <copyright file="ValidationPropertyRuleBuilderExtensions.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

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
