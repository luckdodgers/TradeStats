using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace TradeStats.ViewModel
{
    abstract class BaseViewModel : BindableBase, INotifyDataErrorInfo
    {
        public bool HasErrors => _errorsDict.Values.SelectMany(el => el).Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly Dictionary<string, List<ValidationRule>> _validationRulesDict = new();
        private readonly Dictionary<string, List<string>> _errorsDict = new();

        protected void AddValidators(string propertyName, params ValidationRule[] validationRules)
        {
            _validationRulesDict.Add(propertyName, validationRules.ToList());
            _errorsDict.Add(propertyName, new List<string>());
        }

        protected bool ValidateProperty<TValue>(TValue propertyValue, [CallerMemberName] string propertyName = null)
        {
            OnErrorsChanged(propertyName);

            _errorsDict[propertyName].Clear();
            var validators = _validationRulesDict[propertyName];

            validators.Select(validationRule => validationRule.Validate(propertyValue, CultureInfo.CurrentCulture))
                .Where(result => !result.IsValid)
                .ToList()
                .ForEach(invalidResult => AddErrorToProperty(propertyName, invalidResult.ErrorContent as string));

            return !PropertyHasErrors(propertyName);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return _errorsDict.SelectMany(entry => entry.Value);

            else if (_errorsDict.TryGetValue(propertyName, out List<string> errorList))
                return errorList;

            return new List<string>();
        }

        protected virtual void OnErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        protected bool PropertyHasErrors(string propertyName)
        {
            var hasValue = _errorsDict.TryGetValue(propertyName, out var errors);

            if (!hasValue || errors.Count == 0)
                return false;

            return true;
        }

        private void AddErrorToProperty(string propertyName, string error)
        {
            _errorsDict[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }
}
