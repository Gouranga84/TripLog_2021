using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TripLog.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseViewModel() { }

        public virtual void Init() { }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BaseViewModel<TParameter> : BaseViewModel
    {
        protected BaseViewModel() { }

        public override void Init()
        {
            Init(default(TParameter));
        }

        public virtual void Init(TParameter parameter) { }
    }
}
