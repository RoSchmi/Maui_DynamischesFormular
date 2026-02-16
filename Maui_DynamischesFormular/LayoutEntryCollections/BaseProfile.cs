using System;
using System.Collections.Generic;
using System.Text;

namespace Maui_DynamischesFormular.LayoutEntryCollections
{
    public class BaseProfile
    {
        // must expose a parameter-less constructor
        public BaseProfile() { }

        public string? Account { get; set; }

        public string? Profile { get; set; }

        public string? DataGroup { get; set; }

        public string? SettingsID { get; set; }

        public bool? SettingsState { get; set; }
        public DateTime? SettingsDate { get; set; }
        public string? Index { get; set; }
        public string? Selected { get; set; }
    }
}
