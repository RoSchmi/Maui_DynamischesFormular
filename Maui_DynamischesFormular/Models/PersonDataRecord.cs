using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Maui_DynamischesFormular.Models
{
    public partial class PersonDataRecord : ObservableObject
    {
        [ObservableProperty]
        private int? personIndex;

        [ObservableProperty]

        private string personGuid;
        public ObservableCollection<BaseItem> Items { get; } = 
        new ObservableCollection<BaseItem>();
    }
}
