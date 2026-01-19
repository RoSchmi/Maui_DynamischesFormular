//using System;
//using System.Globalization;
//using Android.Service.Settings.Preferences;
//using Accounts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui_DynamischesFormular.Common;
using Maui_DynamischesFormular.Helpers;

//using Java.Sql;

//using Java.Sql;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.Pages;
using System.Buffers.Text;
// using Microsoft.OData.Edm; Nuget can be deleted (for now), not used
// Microsoft.Rest.Azure.OData Nuget can be deleted (for now), not used
//using Microsoft.Rest.Azure.OData;



using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Linq;
//using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text.Json;
using RoSchmi.Maui.Interfaces;
using RoSchmi.Maui.Services;
using System.Threading;
using System.Xml;
//using static Android.Media.Audiofx.DynamicsProcessing;
//using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;
//using static Java.Util.Jar.Attributes;


namespace Maui_DynamischesFormular.ViewModels;

// String message from the sending page
//[QueryProperty("SenderMessage", "Parameter")]
[QueryProperty("SenderMessage", "Sender")]

public partial class MainPageViewModel : ObservableObject, IQueryAttributable
{
    private INavigationService _navigation;

    // https://github.com/beto-rodriguez/LiveCharts2/blob/master/docs/cartesianChart/columnseries.md

    public IDictionary<string, object> queryHandle;

    

    private CultureInfo invariantCulture = CultureInfo.InvariantCulture;

    private const string accountsFileName = "Accounts.txt";
    private const string profilesFileName = "Profiles.xml";
    private readonly string rootPath = FileSystem.Current.AppDataDirectory;
    private const string appFolder = "ChartSluuk";

    private const string Delimiter = ";";


    private ObservableCollection<string> profilesExtended = new();


    // Here you can initialize new pairs of variablenames and content
    // The types string, bool and datetime are allowed for now
    // The types have to be defined in the File 'ProfileSet.cs'
    // Besides these default values the code in 'Wrapper.ProfileSetToSuitCaseProperties' has to be changed

    private readonly ProfileSet profileSetDefault = new()
    {
        SettingsID = "",
        Account = "",
        Index = "0",
        Selected = "1",
        Profile = "Profile-1",
        Table1Account = "",
        // The Variablenames above may not be changed and my not be used for naming other variables, only the content can be changed

        // From the following variables the names and the content can be changed
        /*
        SettingsState = false,
        SettingsDate = DateTime.MinValue,
        SettingsTable1 = string.Empty,
        SettingsTable2 = string.Empty,
        SettingsTable3 = string.Empty,
        SettingsTable4 = string.Empty, */
    };

    private float[] actYearValues = new float[366];
    private float[] actYear_minus_1_Values = new float[366];
    private float[] actYear_minus_2_Values = new float[366];
    private float[] actYear_minus_3_Values = new float[366];

    private enum Period { Week, Month, Quarter, Year };

    private enum ValType { NotValid, ValString, ValFloat, ValTimeSpan };

    

    private ISeries[] WeekSeries { get; set; }

    private ISeries[] MonthsSeries { get; set; }

    private ISeries[] QuartersSeries { get; set; }

    private ISeries[] YearsSeries { get; set; }

    public struct UpdateChartsFromCloud
    {
        public UpdateChartsFromCloud() { }
        public DateTime lastUpdateTime { get; set; } = DateTime.MinValue;
        public bool shallUpdate { get; set; } = true;
    }

    private UpdateChartsFromCloud updateChartsFromCloud;

    private static readonly IFormatProvider formatProviderInvariantDateTime = CultureInfo.InvariantCulture.DateTimeFormat;
    private static readonly IFormatProvider formatProviderInvariantNumber = CultureInfo.InvariantCulture.NumberFormat;


    private static Dictionary<string, string> ProfileMemberNameAssignation;

    private static Dictionary<string, SuitCaseProperties> profilesDictionary;

    SuitCaseProperties suitCaseProperties = new();

   

    //private List<SettingItem> settingItems = new List<SettingItem>();

    // [ObservableProperty]
    // private string vorname;



    #region Region Create example ObservableCollection containing three PersonRecords
    public ObservableCollection<PersonDataRecord> PersonRecords { get; }
    = new ObservableCollection<PersonDataRecord>
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
             },
             new PersonDataRecord
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
            }
    };
    #endregion

    [ObservableProperty] 
    private PersonDataRecord selectedRecord;




    #region Constructor
    public MainPageViewModel(INavigationService navigation)
    {
        _navigation = navigation;
       
        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;

        selectedRecord = PersonRecords.First();

        ProfileSet profSet = JsonSerializer.Deserialize<ProfileSet>(JsonSerializer.Serialize(profileSetDefault));
      

        profSet.SettingsID = Guid.NewGuid().ToString();
        profSet.SettingsID = Guid.NewGuid().ToString();

        string ActAccount = "myaccount";
        profSet.Account = ActAccount;

        suitCaseProperties = Wrapper.ProfileSetToSuitCaseProperties(profSet);

        string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{profSet.Profile}");

        profilesDictionary = new Dictionary<string, SuitCaseProperties>()
                                {
                                {profileAndAccount, suitCaseProperties},
                                };

        //DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);

        WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary.First().Value.PropertiesDictionary);

        int breakPoint175 = 1;

        // Copy to ObservableCollection with less items to avoid empty space in CollectionView
        /*
        WorkItemShowCollection = new ObservableCollection<WorkItem>();
        for (int i = 0; i < 7; i++)
        {
            WorkItemShowCollection.Add(WorkItemCollection[i]);
        }
        */

        /*

        if (!ProfileNames.Contains(profSet.Profile))
        {
            ProfileNames.Add(profSet.Profile);
        }
        SelectedProfile = profSet.Profile;
        lastSelectedProfile = profSet.Profile;
        break;
        */





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
        //AppState = 4;
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
    [ObservableProperty]
    private bool navStateRowIsVisible = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedProfileExtended))]
    private static string commonUnit;


    [ObservableProperty]
    private int appState;

    [ObservableProperty]
    private string navigationState;

    [ObservableProperty]
    private string injectedSender;

    [ObservableProperty]
    private string sender;

    [ObservableProperty]
    private static ObservableCollection<WorkItem> workItemCollection = new ObservableCollection<WorkItem>();           // Is the Binding source of CollectionView 


    [ObservableProperty]
    private ObservableCollection<string> profileNames = new() { "Profile-1" };  // Holds the names of the profiles of the actually selected account,

    [ObservableProperty]
    private static DateTime currentDate = DateTime.Today;

    [ObservableProperty]
    private static string actAccount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedProfileExtended))]
    private static string selectedProfile = "Profile-1";

    public string SelectedProfileExtended
    {
        get => $"Charts ({ActAccount}, {SelectedProfile}) : {CommonUnit}";
        set => SetProperty(ref selectedProfile, value);
    }

    #endregion





    [RelayCommand]
    private async Task Button2Clicked(object s)     
    {
        string sender = nameof(MainPage);
        object appState = (object)AppState;


        var navigationParameter = new Dictionary<string, object>() {
                {"sender", nameof(MainPage)},       
                { nameof(MainPage), appState }
            };
       

        
       // await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Sender={sender}", navigationParameter);

        await _navigation.GoToAsync(nameof(SettingsPage), navigationParameter);

    }


    [RelayCommand]
    private async Task Button3Clicked(object s)
    {
        await _navigation.GoToAsync(nameof(PersonEditPage), null);
        
       // await Shell.Current.GoToAsync(nameof(PersonEditPage));
    }
   

    public void OnNavigatedFrom(NavigatedFromEventArgs e)
    {
        int breakpointOnMavigatedFrom = 1;
    }


    #region GraphPageOnAppearingCommand

    public async void GraphPageOnAppearingCommand()
    {
        
    }

    #endregion

    public async void OnMainPageNavigatedToCommand(NavigatedToEventArgs e)
    {
        NavigationState = Shell.Current.CurrentState.Location.ToString();

        Sender = InjectedSender;
        InjectedSender = null;
        if (queryHandle != null)
        {
            queryHandle.Clear();
        }

        //************************************

        // RoSchmi Only for Tests
        // AccountHelper.DeleteAccountsFile(appFolder, accountsFileName);

        // Get Accountslist from file

        //Current_MainDisplayInfoChanged

        // ActAccount = AccountHelper.GetActAccountFromFile(appFolder, accountsFileName) ?? string.Empty;

        ActAccount = "myAccount";
        AppState = ActAccount != string.Empty ? 1 : AppState;

        if (AppState < 1)
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "No Account selected!\r\nGo to '< Set Data Sources' and Click 'Select Account >'on the upper right corner", "OK");
            return;
        }

        // Load ProfilesDictionary from file
        profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);

        AppState = (profilesDictionary != null && profilesDictionary.Count > 0) ? 2 : AppState;

        if (AppState < 2)
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "No Profiles found!\r\nGo to SettingsPage", "OK");
            return;
        }

        SelectedProfile = Helper.GetSelectedProfileOfThisAccountFromProfilesDictionary(profilesDictionary, ActAccount, Delimiter);

        AppState = (SelectedProfile != null && SelectedProfile != string.Empty) ? 3 : AppState;

        if (AppState < 3)
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "Could not retrieve selected Profile!\r\nGo to SettingsPage", "OK");
            return;
        }

        WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[FormattableString.Invariant($"{ActAccount}{Delimiter}{SelectedProfile}")].PropertiesDictionary);

        AppState = FillProfileNamesAndProfilesExtended(WorkItemCollection, profilesDictionary) ? 4 : AppState;

        if (AppState < 4)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Could not assign ProfileNames!\r\nGo to SettingsPage", "OK");
            return;
        }

        await RefreshAllYearValuesFromShadowOrCloud(rootPath, appFolder, pForceRefreshFromCloud: false, applyChanges: true);

        /*
        if (!(actAccount == null || actAccount == string.Empty))
        {

            string theKey = await SecureStorage.GetAsync(ActAccount);

            if ((updateChartsFromCloud.shallUpdate || (DateTime.Now - updateChartsFromCloud.lastUpdateTime > new TimeSpan(0, 0, 5))) && (theKey != null))
            {
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + ActAccount + ";AccountKey=" + await SecureStorage.GetAsync(ActAccount);

                var table1Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable1"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable1"]).StringValue : string.Empty;
                var table2Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable2"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable2"]).StringValue : string.Empty;
                var table3Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable3"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable3"]).StringValue : string.Empty;
                var table4Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable4"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable4"]).StringValue : string.Empty;

                var table1ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable1ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable1ColumnName"]).StringValue : string.Empty;
                var table2ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable2ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable2ColumnName"]).StringValue : string.Empty;
                var table3ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable3ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable3ColumnName"]).StringValue : string.Empty;
                var table4ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable4ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable4ColumnName"]).StringValue : string.Empty;

                var table1Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Factor").StringValue : "1";
                var table2Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Factor").StringValue : "1";
                var table3Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Factor").StringValue : "1";
                var table4Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Factor").StringValue : "1";

                var table1Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Unit").StringValue : string.Empty;
                var table2Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Unit").StringValue : string.Empty;
                var table3Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Unit").StringValue : string.Empty;
                var table4Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Unit").StringValue : string.Empty;

                var table1Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Type").StringValue : string.Empty;
                var table2Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Type").StringValue : string.Empty;
                var table3Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Type").StringValue : string.Empty;
                var table4Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Type").StringValue : string.Empty;

                var table1Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Account").StringValue : string.Empty;
                var table2Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Account").StringValue : string.Empty;
                var table3Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Account").StringValue : string.Empty;
                var table4Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Account").StringValue : string.Empty;

                var table1SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1SortField").StringValue : string.Empty;
                var table2SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2SortField").StringValue : string.Empty;
                var table3SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3SortField").StringValue : string.Empty;
                var table4SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4SortField").StringValue : string.Empty;

                // Set CommonUnit if Unitis for all displayes tables are the same (or following are null or empty) (otherwise the unit is not displayed)
                CommonUnit = table1Unit;
                CommonUnit = (table1Unit == table2Unit) || string.IsNullOrEmpty(table2Unit) ? CommonUnit : "";
                CommonUnit = (table1Unit == table3Unit) || string.IsNullOrEmpty(table3Unit) ? CommonUnit : "";
                CommonUnit = (table1Unit == table4Unit) || string.IsNullOrEmpty(table4Unit) ? CommonUnit : "";

                                  actYearValues = await FillYearValuesFromShadowOrCloud(actYearValues, rootPath, appFolder, table1Account, table1Name, table1ColumnName, table1SortField, table1Factor, table1Type, refreshFromCloud: false, applyChanges: true);
                actYear_minus_1_Values = await FillYearValuesFromShadowOrCloud(actYear_minus_1_Values, rootPath, appFolder, table2Account, table2Name, table2ColumnName, table2SortField, table2Factor, table2Type, refreshFromCloud: false, applyChanges: true);
                actYear_minus_2_Values = await FillYearValuesFromShadowOrCloud(actYear_minus_2_Values, rootPath, appFolder, table3Account, table3Name, table3ColumnName, table3SortField, table3Factor, table3Type, refreshFromCloud: false, applyChanges: true);
                actYear_minus_3_Values = await FillYearValuesFromShadowOrCloud(actYear_minus_3_Values, rootPath, appFolder, table4Account, table4Name, table4ColumnName, table4SortField, table4Factor, table4Type, refreshFromCloud: false, applyChanges: true);

                WeekSeries = ActualizeWeekSeries(DateTime.Today, ref actYearValues, ref actYear_minus_1_Values, ref actYear_minus_2_Values, ref actYear_minus_3_Values);
                Series = WeekSeries;

                updateChartsFromCloud = new UpdateChartsFromCloud() { shallUpdate = false, lastUpdateTime = DateTime.Now };
            }
        }
        */
    }


    private async Task RefreshAllYearValuesFromShadowOrCloud(string rootPath, string appFolder, bool pForceRefreshFromCloud = false, bool applyChanges = true)
    {
        // Reset flag indicating that values from actual year shall be refreshed anyway
        // forceRefreshFromCloud = false;

        if (!(actAccount == null || actAccount == string.Empty))
        {

            string theKey = await SecureStorage.GetAsync(ActAccount);

            if ((updateChartsFromCloud.shallUpdate || (DateTime.Now - updateChartsFromCloud.lastUpdateTime > new TimeSpan(0, 0, 5))) && (theKey != null))
            {
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + ActAccount + ";AccountKey=" + await SecureStorage.GetAsync(ActAccount);

                var table1Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable1"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable1"]).StringValue : string.Empty;
                var table2Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable2"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable2"]).StringValue : string.Empty;
                var table3Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable3"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable3"]).StringValue : string.Empty;
                var table4Name = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable4"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiDataSourceTable4"]).StringValue : string.Empty;

                var table1ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable1ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable1ColumnName"]).StringValue : string.Empty;
                var table2ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable2ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable2ColumnName"]).StringValue : string.Empty;
                var table3ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable3ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable3ColumnName"]).StringValue : string.Empty;
                var table4ColumnName = WorkItemCollection.Any(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable4ColumnName"]) ? WorkItemCollection.First(WorkItem => WorkItem.Name == ProfileMemberNameAssignation["PiTable4ColumnName"]).StringValue : string.Empty;

                var table1Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Factor").StringValue : "1";
                var table2Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Factor").StringValue : "1";
                var table3Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Factor").StringValue : "1";
                var table4Factor = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Factor") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Factor").StringValue : "1";

                var table1Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Unit").StringValue : string.Empty;
                var table2Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Unit").StringValue : string.Empty;
                var table3Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Unit").StringValue : string.Empty;
                var table4Unit = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Unit") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Unit").StringValue : string.Empty;

                var table1Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Type").StringValue : string.Empty;
                var table2Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Type").StringValue : string.Empty;
                var table3Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Type").StringValue : string.Empty;
                var table4Type = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Type") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Type").StringValue : string.Empty;

                var table1Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1Account").StringValue : string.Empty;
                var table2Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2Account").StringValue : string.Empty;
                var table3Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3Account").StringValue : string.Empty;
                var table4Account = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4Account") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4Account").StringValue : string.Empty;

                var table1SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table1SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table1SortField").StringValue : string.Empty;
                var table2SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table2SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table2SortField").StringValue : string.Empty;
                var table3SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table3SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table3SortField").StringValue : string.Empty;
                var table4SortField = WorkItemCollection.Any(WorkItem => WorkItem.Name == "Table4SortField") ? WorkItemCollection.First(WorkItem => WorkItem.Name == "Table4SortField").StringValue : string.Empty;

                // Set CommonUnit if Unitis for all displayes tables are the same (or following are null or empty) (otherwise the unit is not displayed)
                CommonUnit = table1Unit;
                CommonUnit = (table1Unit == table2Unit) || string.IsNullOrEmpty(table2Unit) ? CommonUnit : "";
                CommonUnit = (table1Unit == table3Unit) || string.IsNullOrEmpty(table3Unit) ? CommonUnit : "";
                CommonUnit = (table1Unit == table4Unit) || string.IsNullOrEmpty(table4Unit) ? CommonUnit : "";

                actYearValues = await FillYearValuesFromShadowOrCloud(actYearValues, rootPath, appFolder, table1Account, table1Name, table1ColumnName, table1SortField, table1Factor, table1Type, pForceRefreshFromCloud, applyChanges = true);
                actYear_minus_1_Values = await FillYearValuesFromShadowOrCloud(actYear_minus_1_Values, rootPath, appFolder, table2Account, table2Name, table2ColumnName, table2SortField, table2Factor, table2Type, pForceRefreshFromCloud, applyChanges = true);
                actYear_minus_2_Values = await FillYearValuesFromShadowOrCloud(actYear_minus_2_Values, rootPath, appFolder, table3Account, table3Name, table3ColumnName, table3SortField, table3Factor, table3Type, pForceRefreshFromCloud, applyChanges = true);
                actYear_minus_3_Values = await FillYearValuesFromShadowOrCloud(actYear_minus_3_Values, rootPath, appFolder, table4Account, table4Name, table4ColumnName, table4SortField, table4Factor, table4Type, pForceRefreshFromCloud, applyChanges = true);

                WeekSeries = ActualizeWeekSeries(DateTime.Today, ref actYearValues, ref actYear_minus_1_Values, ref actYear_minus_2_Values, ref actYear_minus_3_Values);
                Series = WeekSeries;

                updateChartsFromCloud = new UpdateChartsFromCloud() { shallUpdate = false, lastUpdateTime = DateTime.Now };
            }
        }
    }

    #region Region private ISeries[] series = new ISeries[]
    private ISeries[] series = new ISeries[]
    {
        new ColumnSeries<int>
        {
            Values = new [] { 0 },
        }
    };
    #endregion

    public ISeries[] Series { get => series; set { _ = SetProperty(ref series, value); } }

    #region Region private Axis[] xAxes = new Axis[]
    private Axis[] xAxes = new Axis[]
        {
            new Axis
            {
                Name= "Days of Week",
                NamePaint = new SolidColorPaint(SKColors.Black),

                LabelsPaint = new SolidColorPaint(SKColors.CornflowerBlue),
                TextSize = 20,
                UnitWidth = 1,
                MinStep = 1,
                Labeler = value =>  _ =  string.Concat("  ", currentDate.AddDays(-((int)currentDate.DayOfWeek - 1) + (int)value).DayOfWeek.ToString().AsSpan(0,3), "\r\n", currentDate.AddDays(-((int)currentDate.DayOfWeek - 1) + (int)value).ToString("dd.MMM", formatProviderInvariantDateTime )),
            }
        };
    #endregion

    public Axis[] XAxes { get => xAxes; set { _ = SetProperty(ref xAxes, value); } }

    #region Region private Axis[] yAxes = new Axis[]
    private Axis[] yAxes = new Axis[]
    {
         new Axis
         {}
    };
    #endregion
    public Axis[] YAxes { get => yAxes; set { _ = SetProperty(ref yAxes, value); } }

    #region Region Method ActualizeWeekSeries
    private static ISeries[] ActualizeWeekSeries(DateTime currentDate,
                                        ref float[] actYearValues,
                                        ref float[] actYear_minus_1_Values,
                                        ref float[] actYear_minus_2_Values,
                                        ref float[] actYear_minus_3_Values
                                        )
    {
        int firstDayOfWeek = currentDate.DayOfYear - ((int)currentDate.DayOfWeek - 1);

        int firstDayOfThisWeek = DateTime.Today.DayOfYear - ((int)DateTime.Today.DayOfWeek - 1);

        //RoSchmi

        firstDayOfWeek = 1;

        int todaysDayOfYear = DateTime.Today.DayOfYear;

        var actWeekColumnSeries = new ColumnSeries<int>();
        var actWeek_Year_Minus_1_ColumnSeries = new ColumnSeries<int>();
        var actWeek_Year_Minus_2_ColumnSeries = new ColumnSeries<int>();
        var actWeek_Year_Minus_3_ColumnSeries = new ColumnSeries<int>();

        actWeekColumnSeries.Values = new[] { (int)actYearValues[firstDayOfWeek] ,
                                            firstDayOfWeek + 1 <= todaysDayOfYear ? (int)actYearValues[firstDayOfWeek + 1] : 0,
                                            firstDayOfWeek + 2 <= todaysDayOfYear ? (int)actYearValues[firstDayOfWeek + 2] : 0,
                                            firstDayOfWeek + 3 <= todaysDayOfYear ? (int)actYearValues[firstDayOfWeek + 3] : 0,
                                            firstDayOfWeek + 4 <= todaysDayOfYear ? (int)actYearValues[firstDayOfWeek + 4] : 0,
                                            firstDayOfWeek + 5 <= todaysDayOfYear ? (int)actYearValues[firstDayOfWeek + 5] : 0,
                                            firstDayOfWeek + 6 <= todaysDayOfYear ? (int)actYearValues[firstDayOfWeek + 6] : 0
                                            };

        actWeek_Year_Minus_1_ColumnSeries.Values = new[] { (int)actYear_minus_1_Values[firstDayOfWeek],
                                            (int)actYear_minus_1_Values[firstDayOfWeek + 1],
                                            (int)actYear_minus_1_Values[firstDayOfWeek + 2],
                                            (int)actYear_minus_1_Values[firstDayOfWeek + 3],
                                            (int)actYear_minus_1_Values[firstDayOfWeek + 4],
                                            (int)actYear_minus_1_Values[firstDayOfWeek + 5],
                                            (int)actYear_minus_1_Values[firstDayOfWeek + 6]
                                            };

        actWeek_Year_Minus_2_ColumnSeries.Values = new[] { (int)actYear_minus_2_Values[firstDayOfWeek],
                                            (int)actYear_minus_2_Values[firstDayOfWeek + 1],
                                            (int)actYear_minus_2_Values[firstDayOfWeek + 2],
                                            (int)actYear_minus_2_Values[firstDayOfWeek + 3],
                                            (int)actYear_minus_2_Values[firstDayOfWeek + 4],
                                            (int)actYear_minus_2_Values[firstDayOfWeek + 5],
                                            (int)actYear_minus_2_Values[firstDayOfWeek + 6]
                                            };

        actWeek_Year_Minus_3_ColumnSeries.Values = new[] { (int)actYear_minus_3_Values[firstDayOfWeek],
                                            (int)actYear_minus_3_Values[firstDayOfWeek + 1],
                                            (int)actYear_minus_3_Values[firstDayOfWeek + 2],
                                            (int)actYear_minus_3_Values[firstDayOfWeek + 3],
                                            (int)actYear_minus_3_Values[firstDayOfWeek + 4],
                                            (int)actYear_minus_3_Values[firstDayOfWeek + 5],
                                            (int)actYear_minus_3_Values[firstDayOfWeek + 6]
                                            };

        var localWeekSeries = new ISeries[]
        {
            new ColumnSeries<int>
            {
                //Values = new [] { 4, 4, 7, 2, 8, 4, 3 },               
                Values = actWeek_Year_Minus_3_ColumnSeries.Values,
                //Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 }, // mark
                MaxBarWidth = 15, // mark
                Padding = 1,
                Stroke = null,
                Fill = new SolidColorPaint(SKColors.Yellow) { },
                //Fill = null,
            },
            new ColumnSeries<int>
            {
                Values = actWeek_Year_Minus_2_ColumnSeries.Values,
                MaxBarWidth = 15, // mark
                Padding = 1,
                Fill = new SolidColorPaint(SKColors.Green),

            },
            new ColumnSeries<int>
            {
                Values = actWeek_Year_Minus_1_ColumnSeries.Values,
                MaxBarWidth = 15, // mark
                Padding = 1,
                Fill = new SolidColorPaint(SKColors.Blue),
            },
            new ColumnSeries<int>
            {
                Values = actWeekColumnSeries.Values,
                MaxBarWidth = 15, // mark
                Padding = 1,
                Fill = new SolidColorPaint(SKColors.Red),
            }
        };
        return localWeekSeries;
    }
    #endregion

    #region Region Method ActualizeMonthsSeries

    #endregion


    private async Task<float[]> FillYearValuesFromShadowOrCloud(float[] periodValuesArray, string rootPath, string appFolder, string pTableAccount, string pTableName, string pColumnName, string pSortField, string pFactor, string pType, bool forceRefreshFromCloud, bool applyChanges)
    {
        // First get values from Shadow file
        Dictionary<string, object> shadowDict = await TableHelper.GetYearValuesFromShadowFile(actYearValues, rootPath, appFolder, pTableAccount, pTableName, pColumnName);
        periodValuesArray = (TableHelper.ReturnState)shadowDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? shadowDict[TableHelper.ReturnKeys.ArrayContent] as float[] : periodValuesArray;
        string TableYearString = (TableHelper.ReturnState)shadowDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? shadowDict[TableHelper.ReturnKeys.Year] as string : DateTime.MinValue.Year.ToString(formatProviderInvariantDateTime);
        int TableYearInt = int.TryParse(TableYearString, out TableYearInt) ? TableYearInt : DateTime.MinValue.Year;
        bool shadowFileExists = (TableHelper.ReturnState)shadowDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid;

        // find last day (+1) which doesn't have a value ( means: is < 0.000001)
        int indexLastValidPlusOne = periodValuesArray.Length - 1;
        if (shadowFileExists)
        {
            while (indexLastValidPlusOne > 0 && periodValuesArray[indexLastValidPlusOne] <= 0.000001)
            {
                indexLastValidPlusOne--;
            }
        }

        // Make sure that Azure Key is valid
        bool validAzureKey = pTableAccount != null && pTableAccount != string.Empty && (await SecureStorage.GetAsync(pTableAccount)) != null;

        // If refresh is forced or shadowfile doesn't exist or table is from this year and the values from the last day are not in Shadow file, we read data from the Cloud
        if (validAzureKey && (forceRefreshFromCloud || !shadowFileExists || (TableYearInt == DateTime.Today.Year && new DateTime(TableYearInt, 1, 1).AddDays(indexLastValidPlusOne) < DateTime.Today)))
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + pTableAccount + ";AccountKey=" + await SecureStorage.GetAsync(pTableAccount);
            Dictionary<string, object> cloudDict = await TableHelper.ActualizeBarChartYearSource(TableHelper.ReturnSelector.YearAndFloatArray, periodValuesArray, pTableName, pColumnName, pFactor, pType, connectionString);
            periodValuesArray = (TableHelper.ReturnState)cloudDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? cloudDict[TableHelper.ReturnKeys.ArrayContent] as float[] : periodValuesArray;

            string xmlShadowFileName = FormattableString.Invariant($"{pTableAccount}.{pTableName}.{pColumnName}.xml");

            string shadowFileCreationDate = DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo);

            Dictionary<string, string> yearShadowValuesXmlDictionary = new()
            {
                { "OriginalFileName", xmlShadowFileName },
                { "TargetProgram", "ChartSluuk" },
                { "Version", "1.0.0" },
                { "OriginalCreationDateUtc", shadowFileCreationDate },
                { "Acount", pTableAccount },
                { "Table", pTableName },
                { "TableYear", TableYearString },
                { "Column", pColumnName },
                { "Factor", pFactor },
                { "Type", pType },
                { "JsonContent",  string.Empty}
            };

            yearShadowValuesXmlDictionary["JsonContent"] = System.Text.Json.JsonSerializer.Serialize(periodValuesArray);

            // Write Shadow to file
            DictionaryXML.WriteDictionaryStringStringToXmlFile(yearShadowValuesXmlDictionary, "YearDailyValues", appFolder, xmlShadowFileName);

            // if file with changes exist, read the file, set 'ShadowOriginalCreationDateUtc' to the date when the shadow file was saved and store back to filesystem
            string changesXmlFileName = FormattableString.Invariant($"{pTableAccount}.{pTableName}.{pColumnName}.Changes.xml");

            Dictionary<string, string> changedYearValuesXmlDictionary = new()
            {
                { "OriginalFileName", string.Empty },
                { "TargetProgram", string.Empty },
                { "Version", string.Empty },
                { "ShadowOriginalCreationDateUtc", string.Empty },
                { "Acount", string.Empty },
                { "Table", string.Empty },
                { "TableYear", string.Empty },
                { "Column", string.Empty },
                { "Factor", string.Empty },
                { "Type", string.Empty },
                { "JsonContent",  string.Empty}
            };

            if (File.Exists(Path.Combine(rootPath, appFolder, changesXmlFileName)))
            {
                changedYearValuesXmlDictionary = DictionaryXML.GetDictionaryStringStringFromXmlFile(appFolder, changesXmlFileName);

                if (changedYearValuesXmlDictionary.Count > 0)
                {
                    changedYearValuesXmlDictionary["ShadowOriginalCreationDateUtc"] = shadowFileCreationDate;

                    DictionaryXML.WriteDictionaryStringStringToXmlFile(changedYearValuesXmlDictionary, "CorrectedDayValues", appFolder, changesXmlFileName);
                }
            }

            int breakPoint = 1;
        }

        return periodValuesArray;
    }


    private bool FillProfileNamesAndProfilesExtended(ObservableCollection<WorkItem> pWorkItemCollection, Dictionary<string, SuitCaseProperties> pProfilesDictionary)
    {
        try
        {
            ProfileMemberNameAssignation = new Dictionary<string, string>
                {
                    { "PiAccount", pWorkItemCollection[0].Name },            // Account
                    { "PiDisplayName", pWorkItemCollection[1].Name },        // DisplayName
                    { "PiDataSourceTable1", pWorkItemCollection[2].Name },   // DataSourceTable1            
                    { "PiDataSourceTable2", pWorkItemCollection[3].Name },   // DataSourceTable2
                    { "PiDataSourceTable3", pWorkItemCollection[4].Name },   // DataSourceTable3
                    { "PiDataSourceTable4", pWorkItemCollection[5].Name },   // DataSourceTable4
                    { "PiTable1ColumnName", pWorkItemCollection[6].Name },
                    { "PiTable2ColumnName", pWorkItemCollection[7].Name },
                    { "PiTable3ColumnName", pWorkItemCollection[8].Name },
                    { "PiTable4ColumnName", pWorkItemCollection[9].Name },
                };

            ProfileNames = new ObservableCollection<string>();
            profilesExtended = new ObservableCollection<string>();
            Dictionary<string, SuitCaseProperties>.KeyCollection theKeys = pProfilesDictionary.Keys;
            foreach (string myKey in theKeys)
            {
                if (myKey.StartsWith(ActAccount, comparisonType: StringComparison.InvariantCulture))
                {
                    string[] splitted = myKey.Split(Delimiter);
                    if (splitted[0] == ActAccount)
                    {
                        ProfileNames.Add(myKey.Substring(splitted[0].Length + Delimiter.Length));
                        profilesExtended.Add(myKey);
                    }
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    }




   


