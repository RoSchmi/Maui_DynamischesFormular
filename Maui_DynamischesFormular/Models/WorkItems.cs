using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.Models;

public partial class WorkItem : ObservableObject
{
    public WorkItem() { }

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

    public string? Name { get; set; }
    public TypeID TypeIdentifier { get; set; }

    public int TabNo { get; set; }
    
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