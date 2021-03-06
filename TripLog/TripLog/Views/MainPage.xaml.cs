using System;
using System.Linq;


using TripLog.Models;
using TripLog.ViewModels;
using TripLog.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TripLog.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        MainViewModel ViewModel => BindingContext as MainViewModel;
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel(DependencyService.Get<INavService>());     
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Initialize MainViewModel
            ViewModel?.Init();
        }
    }
}