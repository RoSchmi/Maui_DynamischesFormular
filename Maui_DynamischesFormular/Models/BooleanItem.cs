using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_DynamischesFormular.Models
{

    public partial class BooleanItem : BaseItem
    {
        [ObservableProperty]
        private bool value;
    }
}

