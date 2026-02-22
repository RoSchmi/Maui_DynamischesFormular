using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.Models;

public partial class WorkItem : ObservableObject
{
    public enum TypeID
    {
        RsString,               // String editable
        RsStringRo,             // String ReadOnly
        RsStringNo,             // String not displayed
        RsStringSw,             // Display on Swipe View
        RsStringPi,             // Display on Picker
        RsBoolean,
        RsBooleanRo,
        RsBooleanNo,
        RsDateTime,
        RsDateTimeRo,
        RsDateTimeNo,
        RsTimeSpan,
        RsGuid,
        RsDouble,
        RsFloat,
        RsInt,
        RsLong,
        RsShort,
    };

    // Constructor
    public WorkItem() 
    {
    }

    public void InitializePicker(Dictionary<string, List<string>> pickerOptions)
    {
        AllowedPickerItems.Clear();

        var baseName = TabNo == 0
            ? Name
            : string.IsNullOrEmpty(Name) ? string.Empty : Name[..^1];

        if (pickerOptions.TryGetValue(baseName, out var items))
        {
            foreach (var item in items)
                AllowedPickerItems.Add(item);
        }
        else
        {
            AllowedPickerItems.Add(""); // fallback
        }

        // Initialwert setzen
        SelectedPickerItem = StringValue;
    }



    

    public string? Name { get; set; }
    public TypeID TypeIdentifier { get; set; }

    public int TabNo { get; set; }

    // ItemsSource für den Picker
    public ObservableCollection<string> AllowedPickerItems { get; set; } = new ObservableCollection<string>();

    partial void OnSelectedPickerItemChanged(string value)
    {
       // StringValue = value != null ? value : StringValue;
        
        if (value != null)
        {
            StringValue = value;
        }
        else
        {
            SelectedPickerItem =  StringValue;
        }
        
    }

    // Ausgewählter Wert
    [ObservableProperty] 
    private string selectedPickerItem;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string stringValue; 

    [ObservableProperty]
    private DateTime? dateValue;

    [ObservableProperty]
    private bool? boolValue;

    // Add other types if needed
    // e.g. public TimeSpan TimeSpanValue { get; set; }      
}