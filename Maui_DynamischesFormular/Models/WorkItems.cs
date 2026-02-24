using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        RsStringFlo,            // Display in Float format
        RsBoolean,
        RsBooleanRo,
        RsBooleanNo,
        RsDateTime,
        RsDateTimeRo,
        RsDateTimeNo,
        RsTimeSpan,
        RsGuid,
        RsDouble,
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

    
    public void InitializeFloatEntry(string invariantCultureString)
    {
        float tempFloat = 0.0f;
        if (float.TryParse(invariantCultureString, NumberStyles.Float, CultureInfo.InvariantCulture, out float result)) {          
            tempFloat = result;      
        }
        StringDisplayCultureFloat = tempFloat.ToString(CultureInfo.CurrentCulture);
    }
    

    partial void OnStringValueChanged(string value)
    {
        if (TypeIdentifier != TypeID.RsStringFlo)
            return;

        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var f))
        {
            StringDisplayCultureFloat = f.ToString(CultureInfo.CurrentCulture);
        }
        else
        {
            StringDisplayCultureFloat = string.Empty;
        }
    }

    partial void OnStringDisplayCultureFloatChanged(string value)
    {
        if (TypeIdentifier != TypeID.RsStringFlo)
            return;

        if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var f))
        {
            StringValue = f.ToString(CultureInfo.InvariantCulture);
        }
        else
        { 
            StringValue = string.Empty; 
        }
    }


    public string? Name { get; set; }
    public TypeID TypeIdentifier { get; set; }

    public int TabNo { get; set; }

    #region Region Properties and functions for Picker handling
    // ItemsSource für den Picker
    public ObservableCollection<string> AllowedPickerItems { get; set; } = new ObservableCollection<string>();



    [ObservableProperty]
    private string selectedPickerItem;


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
    #endregion


    

    [ObservableProperty]
    private string stringDisplayCultureFloat;

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