using CommunityToolkit.Mvvm.ComponentModel;




using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.Models;

public partial class StringTypeContent : ObservableObject
{
   //[ObservableProperty]
   //private string? displayName;

    [ObservableProperty]
    private string? value;
}
