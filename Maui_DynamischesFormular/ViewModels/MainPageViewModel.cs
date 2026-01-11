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
using System.Data;
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



    public ObservableCollection<PersonDataRecord> PersonRecords { get; }

    [ObservableProperty] 
    private PersonDataRecord selectedRecord;




    #region Constructor
    public MainPageViewModel()
    {
       
        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;

        PersonRecords = new ObservableCollection<PersonDataRecord>
        {
             new PersonDataRecord
             {
                 PersonIndex = 0,
                 PersonGuid = Guid.NewGuid().ToString("D"),

                 Items =
                 {
                     //new TextItem   {Name = "personGuid", LabelText = "GUID", Value = Guid.NewGuid().ToString("D") },
                     new TextItem   { Name = "firstName", LabelText = "Vorname", Value = "Max" },
                     new TextItem   { Name = "lastnameName", LabelText = "Nachname", Value = "Mustermann" },
                     new DateItem   { Name = "birthDate", LabelText = "Geburtstag",  Value = DateTime.Today },
                     new BooleanItem {Name = "isAdult", LabelText = "Erwachsener", Value = true },
                 }
             },
             new PersonDataRecord
             {
                 PersonIndex = 1,
                 PersonGuid = Guid.NewGuid().ToString("D"),

                 Items =
                 {
                     //new TextItem   {Name = "personGuid", LabelText = "GUID", Value = Guid.NewGuid().ToString("D") },
                     new TextItem   { Name = "firstName", LabelText = "Vorname", Value = "Monika" },
                     new TextItem   { Name = "lastnameName", LabelText = "Nachname", Value = "Musterfrau" },
                     new DateItem   { Name = "birthDate", LabelText = "Geburtstag",  Value = DateTime.Today },
                     new BooleanItem {Name = "isAdult", LabelText = "Erwachsener", Value = true },
                 }
             }
        };

       

        

        PersonRecords.Add(new PersonDataRecord
        { 
            PersonIndex = 2,
            PersonGuid = Guid.NewGuid().ToString("D"),
            Items =
            {
                     //new TextItem   {Name = "personGuid", LabelText = "GUID", Value = Guid.NewGuid().ToString("D") },
                     new TextItem   { Name = "firstName", LabelText = "Vorname", Value = "Lisa" },
                     new TextItem   { Name = "lastnameName", LabelText = "Nachname", Value = "Musterkind" },
                     new DateItem   { Name = "birthDate", LabelText = "Geburtstag",  Value = DateTime.Today },
                     new BooleanItem {Name = "isAdult", LabelText = "Erwachsener", Value = true },
            }

        });

        selectedRecord = PersonRecords.FirstOrDefault();



       

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
                    { nameof(MainPage), PersonRecords }
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