// --------------------------------------------------------------------------
// <copyright file="Validator{T}.cs" company="-">
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
