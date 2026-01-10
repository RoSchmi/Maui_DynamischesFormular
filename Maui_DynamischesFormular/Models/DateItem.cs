using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_DynamischesFormular.Models
{


    public partial class DateItem : BaseItem
    {
        [ObservableProperty]
        private DateTime value;
    }
}

