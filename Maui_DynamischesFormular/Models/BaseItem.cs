using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_DynamischesFormular.Models
{

    public abstract partial class BaseItem : ObservableObject
    {
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string labelText;
        
    }
}
