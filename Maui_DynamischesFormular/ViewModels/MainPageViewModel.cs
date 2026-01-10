//using System;
//using System.Globalization;
//using Android.Service.Settings.Preferences;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
//using Java.Sql;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.Pages;
//using Maui_DynamischesFormular.Common;
using System.Buffers.Text;
// using Microsoft.OData.Edm; Nuget can be deleted (for now), not used
// Microsoft.Rest.Azure.OData Nuget can be deleted (for now), not used
//using Microsoft.Rest.Azure.OData;



using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
//using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text.Json;
using System.Threading;
using System.Xml;
//using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;
//using static Java.Util.Jar.Attributes;


namespace Maui_DynamischesFormular.ViewModels;

// String message from the sending page
//[QueryProperty("SenderMessage", "Parameter")]
[QueryProperty("SenderMessage", "Sender")]

public partial class MainPageViewModel : ObservableObject, IQueryAttributable
{

    // https://github.com/beto-rodriguez/LiveCharts2/blob/master/docs/cartesianChart/columnseries.md


    public IDictionary<string, object> queryHandle;

    //private List<SettingItem> settingItems = new List<SettingItem>();

    [ObservableProperty]
    private string vorname;



    public ObservableCollection<BaseItem> Items { get; set; }
        
    


    #region Constructor
    public MainPageViewModel()
    {
       
        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;

        Items = new ObservableCollection<BaseItem>
        {
            new TextItem { Name = "Benutzername", Value = "Roland" },
            new BooleanItem { Name = "Dark Mode", Value = true },
            new DateItem { Name = "Geburtsdatum", Value = DateTime.Today }
        };

        // var Item = Items.FirstOrDefault();

        BaseItem Item0 = Items[0];
        BaseItem Item1 = Items[1];
        BaseItem Item2 = Items[2];

        Items.Add(new TextItem
        {
            Name = "Benutzername",
            Value = "Monika"
        });

       

        int breakPoint71 = 1;

       // workItems.Add(new WorkItem() { Name = "Nachname", TypeIdentifier = WorkItem.TypeID.RsStringRo, StringValue = "Schmidt" });
      

        /*
        DisplayWidth = 700.0;
        DisplayHeight = 700.0;
        WidthFactor = 1.0;
        */


       
    }
    #endregion

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        queryHandle = query;
        var injectedDictionary = query;
        int breakpoint = 1;
    }


    #region Region event Current_MainDisplayInfoChanged
    private void Current_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
       // DisplayWidth = e.DisplayInfo.Width;
       // DisplayHeight = e.DisplayInfo.Height;
       // screenOrientation = e.DisplayInfo.Orientation;
    }
    #endregion


    #region Region Observable Properties


    #endregion


    [RelayCommand]
    private async Task Button2Clicked(object s)         // Back-Button is pressed
    {
       // Dictionary<string, TransportItem> transportItemDictionary = Wrapper.WorkItemsToTransportItems(TableDetailCollection);

       // string sender = "ProfileDetailPage";
       /*
        var suitCaseProperties = new SuitCaseProperties()
        {
            PropertiesDictionary = transportItemDictionary
        };
        */

        /*
        var navigationParameter = new Dictionary<string, object>() {
            { "ProfileDetailPage", suitCaseProperties }
        };
        */


        //   await Shell.Current.GoToAsync($"{theName}", navigationParameter);

        //await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Parameter={sender}", navigationParameter);
       // await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Sender={sender}", navigationParameter);



        string sender = nameof(MainPage);
        var navigationParameter = new Dictionary<string, object>() {
                    { nameof(MainPage), Items }
            };

        //await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Parameter={sender}", navigationParameter);
        await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Sender={sender}", navigationParameter);
        
    }

    public void OnNavigatedFrom(NavigatedFromEventArgs e)
    {
        int breakpointOnMavigatedFrom = 1;
    }


    #region GraphPageOnAppearingCommand

    public async void GraphPageOnAppearingCommand()
    {
        
    }

    public async void OnMainPageNavigatedToCommand(NavigatedToEventArgs e)
    {

    }

    #endregion

    
}