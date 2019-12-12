// --------------------------------------------------------------------------
// <copyright file="Validator{T}.cs" company="-">
//   Copyright (c) 2008-2019 Eser Ozvataf (eser@ozvataf.com). All rights reserved.
//   Licensed under the Apache-2.0 license. See LICENSE file in the project root
//   for full license information.
//
//   Web: http://eser.ozvataf.com/ GitHub: http://github.com/eserozvataf
// </copyright>
// <author>Eser Ozvataf (eser@ozvataf.com)</author>
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tassle.Validation {
    public class Validator<T> : IValidator<T> {
        private List<ValidationRule<T>> rules;

        public Validator() {
            this.rules = new List<ValidationRule<T>>();
        }

        public ValidationRule<T> AddRule(ValidationRule<T> rule) {
            this.rules.Add(rule);

            return rule;
        }

        public IValidationPropertyRuleBuilder<T, TProperty> AddRuleFor<TProperty>(Expression<Func<T, TProperty>> navigation) {
            var rule = new ValidationRule<T>();

            this.AddRule(rule);

            var propertyRuleBuilder = new ValidationPropertyRuleBuilder<T, TProperty>(rule, navigation);

            return propertyRuleBuilder;
        }

        public IValidationResult Validate(T target) {
            var result = new ValidationResult() {
                IsValid = true,
                Errors = new List<IValidationError>(),
            };

            foreach (var rule in this.rules) {
                var ruleValidationResult = rule.Check(target);

                this.CombineValidationResults(result, ruleValidationResult);
            }

            return result;
        }

        private void CombineValidationResults(ValidationResult target, IValidationResult source) {
            if (!source.IsValid) {
                target.IsValid = false;
            }

            if (source.Errors != null) {
                foreach (var error in source.Errors) {
                    target.Errors.Add(error);
                }
            }
        }
    }
}
