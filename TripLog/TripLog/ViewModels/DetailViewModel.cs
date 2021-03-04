using TripLog.Models;

namespace TripLog.ViewModels
{
    public class DetailViewModel : BaseViewModel<TripLogEntry>
    {
        TripLogEntry _entry;
        public TripLogEntry Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                OnPropertyChanged();
            }
        }

        public DetailViewModel() { }

        public override void Init(TripLogEntry parameter)
        {
            Entry = parameter;
        }
    }
}
