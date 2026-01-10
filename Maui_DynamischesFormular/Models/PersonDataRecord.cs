using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Maui_DynamischesFormular.Models
{
    public class PersonDataRecord
    {
        public ObservableCollection<BaseItem> Items { get; } = 
        new ObservableCollection<BaseItem>();
    }
}
