using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_DynamischesFormular.Models
{

    public partial class TextItem : BaseItem
    {     
        [ObservableProperty]
        private string value;
    }
}

