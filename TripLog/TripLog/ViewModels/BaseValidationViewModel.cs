using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TripLog.ViewModels
{
    public class BaseValidationViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Privates Feld _errors
        readonly IDictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        #endregion

        #region Konstruktor
        public BaseValidationViewModel() { }
        #endregion

        #region Ereigniss ErrorsChanged
        /// <summary>
        /// Deklarieren von Public Event <see cref="ErrorsChanged"/>
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion

        #region Eigenschaft HasErrors
        /// <summary>
        /// Überprüfen ob <see cref="_errors"/> Errors enthält
        /// </summary>
        public bool HasErrors => _errors?.Any(x => x.Value?.Any() == true) == true;
        #endregion

        #region Methode GetErrors
        /// <summary>
        /// Hier werden die Errors gesammelt
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return _errors.SelectMany(x => x.Value);
            }

            if (_errors.ContainsKey(propertyName) && _errors[propertyName].Any())
            {
                return _errors[propertyName];
            }

            return new List<string>();
        }
        #endregion

        #region
        /// <summary>
        /// Hier wird ein Error validiert, bzw. angelegt oder gelöscht falls nicht mehr vorhanden.
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="error"></param>
        /// <param name="propertyName"></param>
        protected void Validate(Func<bool> rule, string error, [CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }

            if (rule() == false)
            {
                _errors.Add(propertyName, new List<string> { error });
            }

            OnPropertyChanged(nameof(HasErrors));

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
    }
}
