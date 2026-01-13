using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui_DynamischesFormular.Models;

public partial class DayValuesSet : ObservableObject
{
    // must expose a parameter-less constructor
    private static readonly IFormatProvider formatProviderInvariantNumber = CultureInfo.InvariantCulture.NumberFormat;

    private bool _isCorrected;
    public DayValuesSet() { }
    public string Date { get; set; }

    [ObservableProperty]
    //public string CloudValue { get; set; }
    private string cloudValue;


    [ObservableProperty]
    //public string ShadowValue { get; set; }
    private string shadowValue;

    [ObservableProperty]
    private string shadowValueCorrected;


    [ObservableProperty]
    private string correctValue;
    public bool IsCorrected
    {
        get => _isCorrected;
        set
        {
            if (value)
            {
                string processedCorrectValue = CorrectValue;
                processedCorrectValue = processedCorrectValue != null ? processedCorrectValue.Replace(',', '.') : "0.00";
                float parseResult;
                if (float.TryParse(processedCorrectValue, formatProviderInvariantNumber, out parseResult))
                {
                    ShadowValueCorrected = processedCorrectValue;
                    CorrectValue = processedCorrectValue;
                    SetProperty(ref _isCorrected, value);
                }
                else
                {
                    SetProperty(ref _isCorrected, true);
                    Task.Delay(500).Wait();
                    SetProperty(ref _isCorrected, false);
                }
            }
            else
            {
                ShadowValueCorrected = ShadowValue;
                _isCorrected = false;
            }
        }
    }
}
/*
using System.Globalization;
using CommunityToolkit.Maui.Core.Extensions;


namespace ChartSluuk.ViewModels;
public partial class ShadowFileViewModel : ObservableObject, IQueryAttributable
{
    private static readonly IFormatProvider formatProviderInvariantDateTime = CultureInfo.InvariantCulture.DateTimeFormat;
    private static readonly IFormatProvider formatProviderCurrentDateTime = CultureInfo.CurrentCulture.DateTimeFormat;
    private static readonly IFormatProvider formatProviderInvariantNumber = CultureInfo.InvariantCulture.NumberFormat;
*/