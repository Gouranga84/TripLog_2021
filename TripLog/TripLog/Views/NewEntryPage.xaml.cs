using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TripLog.ViewModels;

namespace TripLog.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEntryPage : ContentPage
    {
        public NewEntryPage()
        {
            InitializeComponent();

            BindingContext = new NewEntryViewModel();
        }
    }
}