﻿using System;
using Xamarin.Forms;

using TripLog.Models;
using TripLog.Services;
using System.Threading.Tasks;

namespace TripLog.ViewModels
{
    public class NewEntryViewModel : BaseValidationViewModel
    {
        #region Felder lesen/schreiben
        string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Validate(() => !string.IsNullOrWhiteSpace(_title), "Title must be provided");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        double _latitude;
        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        double _longitude;
        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }

        DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        int _rating;
        public int Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                Validate(() => _rating >= 1 && _rating <= 5, "Rating must be between 1 and 5.");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Konstruktor
        public NewEntryViewModel(INavService navService) : base(navService)
        {
            Date = DateTime.Today;
            Rating = 1;
        }

        public override void Init() { }
        #endregion

        #region Button Save
        Command _saveCommand;
        public Command SaveCommand => _saveCommand ?? (_saveCommand = new Command(async () => await  Save(), CanSave));

        async Task Save()
        {
            var newItem = new TripLogEntry
            {
                Title = Title,
                Latitude = Latitude,
                Longitude = Longitude,
                Date = Date,
                Rating = Rating,
                Notes = Notes
            };

            // TODO: Persist entry in a later chapter

            await NavService.GoBack();
        }

        bool CanSave() => !string.IsNullOrWhiteSpace(Title) && !HasErrors;
        #endregion
    }
}
