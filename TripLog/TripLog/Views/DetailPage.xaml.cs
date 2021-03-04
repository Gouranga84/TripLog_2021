﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

using TripLog.Models;
using TripLog.ViewModels;

namespace TripLog.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        DetailViewModel ViewModel => BindingContext as DetailViewModel;

        public DetailPage()
        {
            InitializeComponent();

            BindingContext = new DetailViewModel();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(ViewModel.Entry.Latitude, ViewModel.Entry.Longitude), Distance.FromMiles(.5)));

            map.Pins.Add(new Pin
            {
                Type = PinType.Place,
                Label = ViewModel.Entry.Title,
                Position = new Position(ViewModel.Entry.Latitude, ViewModel.Entry.Longitude)
            });
                        
        }
    }
}