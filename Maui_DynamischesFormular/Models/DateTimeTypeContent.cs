using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.Models;

public partial class DateTimeTypeContent : ObservableObject
{
    [ObservableProperty]
    private DateTime? value;
}

