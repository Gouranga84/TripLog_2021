using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using TripLog.ViewModels;
using TripLog.Services;

[assembly: Dependency(typeof(XamarinFormsNavService))]
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

        #region NavigateTo<TVM>
        /// <summary>
        /// Lädt mit diesem Task die MainPage, da als Grundlage das <see cref="BaseViewModel"/> ohne überladung dient
        /// </summary>
        /// <typeparam name="TVM"></typeparam>
        /// <returns></returns>
        public async Task NavigateTo<TVM>() where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is BaseViewModel)
            {
                ((BaseViewModel)XamarinFormsNav.NavigationStack.Last().BindingContext).Init();
            }
        }
        #endregion

        #region NavigateTo<TVM>
        /// <summary>
        /// Lädt an der Stelle "wahrscheinlich" das DetailViewModel da eine Parameterübergabe stattfindet, in dem Fall
        /// die Selection der MainPage
        /// </summary>
        /// <typeparam name="TVM"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task NavigateTo<TVM, TParameter>(TParameter parameter) where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is BaseViewModel<TParameter>)
            {
                ((BaseViewModel<TParameter>)XamarinFormsNav.NavigationStack.Last().BindingContext).Init(parameter);
            }
        }
        #endregion

        #region RemoveLastView
        /// <summary>
        /// Prüft ob auf dem Stack 2 ViewModels drauf sind, wenn ja wird die unterste gelöscht.
        /// </summary>
        public void RemoveLastView()
        {
            if (XamarinFormsNav.NavigationStack.Count() < 2)
            {
                return;
            }

            var lastView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 2];
            XamarinFormsNav.RemovePage(lastView);
        }
        #endregion

        #region ClearBackStack
        /// <summary>
        /// Prüft ob sich auf dem Stack mehr als 1 ViewModel Befindet, wenn ja werden alle
        /// darunterliegenden ViewModels gelöscht
        /// </summary>
        public void ClearBackStack()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
            {
                return;
            }

            for (int i = 0; i < XamarinFormsNav.NavigationStack.Count - 1; i++)
            {
                XamarinFormsNav.RemovePage(XamarinFormsNav.NavigationStack[i]);
            }
        }
        #endregion

        #region NavigateToUri
        /// <summary>
        /// Navigiert zur übergebenen URI
        /// </summary>
        /// <param name="uri"></param>
        public void NavigateToUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentException("Invalid URI");
            }

            Device.OpenUri(uri);
        }
        #endregion

        #region NavigateToView
        /// <summary>
        /// Prüft in der <see cref="_map"/> ob sich eine definierte View drin befindet.
        /// Über die Reflection Methode wird der Konstruktor der <paramref name="viewModelType"/> instanziiert
        /// Über <see cref="XamarinFormsNav"/> wird zur Seite navigiert.
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        async Task NavigateToView(Type viewModelType)
        {
            if (!_map.TryGetValue(viewModelType, out Type viewType))
            {
                throw new ArgumentException("No view found in view mapping for " + viewModelType.FullName + ".");
            }

            // Use reflection to geht the View´s constructor and create an instance of the View
            var constructor = viewType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(dc => !dc.GetParameters().Any());

            var view = constructor.Invoke(null) as Page;

            await XamarinFormsNav.PushAsync(view, true);
        }
        #endregion

        void OnCanGoBackChanged() => CanGoBackChanged?.Invoke(this, new PropertyChangedEventArgs("CanGoBack"));
    }
}
