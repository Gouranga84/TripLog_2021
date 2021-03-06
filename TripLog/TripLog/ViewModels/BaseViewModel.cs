using System.ComponentModel;
using System.Runtime.CompilerServices;

using TripLog.Services;

namespace TripLog.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected INavService NavService { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseViewModel(INavService navService)
        {
            NavService = navService;
        }

        public virtual void Init() { }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BaseViewModel<TParameter> : BaseViewModel
    {
        protected BaseViewModel(INavService navService) : base(navService)
        {
        }

        public override void Init()
        {
            Init(default(TParameter));
        }

        public virtual void Init(TParameter parameter) { }
    }
}
