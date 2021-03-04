using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using TripLog.ViewModels;

namespace TripLog.Services
{
    public class XamarinFormsNavService : INavService
    {
        public INavigation XamarinFormsNav { get; set; }


        readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        public void RegisterViewMapping(Type viewModel, Type view)
        {
            _map.Add(viewModel, view);
        }

        public event PropertyChangedEventHandler CanGoBackChanged;

        #region CanGoBack
        /// <summary>
        /// Prüft ob der <see cref="XamarinFormsNav.NavigationStack"/> bereits existiert und die Anzahl größer 0 ist.
        /// </summary>
        public bool CanGoBack => XamarinFormsNav.NavigationStack != null && XamarinFormsNav.NavigationStack.Count > 0;
        #endregion

        #region GoBack
        /// <summary>
        /// nach erfolgreicher Prüfung von <see cref="XamarinFormsNav.NavigationStack"/> wird die oberste Schicht auf dem Stack beendet
        /// <see cref="OnCanGoBackChanged()"/> überprüft erneut den Stack nach dem die oberste Schicht beendet wurde ob ein weiteres zurück möglich ist.
        /// </summary>
        /// <returns></returns>
        public async Task GoBack()
        {
            if (CanGoBack)
            {
                await XamarinFormsNav.PopAsync(true);
                OnCanGoBackChanged();
            }
        }
        #endregion
    }
}
