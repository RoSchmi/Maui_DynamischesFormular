
using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_DynamischesFormular.Models;

public partial class BoolTypeContent : ObservableObject
{
    [ObservableProperty]
    private bool? value;
}
