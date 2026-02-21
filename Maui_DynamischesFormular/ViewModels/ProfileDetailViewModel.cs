using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui_DynamischesFormular.Common;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.Pages;
using Maui_DynamischesFormular.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.ViewModels;

[QueryProperty("TextToShow", "Sender")]


public partial class ProfileDetailViewModel : ObservableObject, IQueryAttributable
{

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        
        #region Region Sender is Settingspage
        if (query.ContainsKey("SettingsPage"))
        {
            LocalSuitCaseProperties = query["SettingsPage"] as SuitCaseProperties;


            // var transportItemDictionary = new Dictionary<string, TransportItem>();

            // transportItemDictionary = LocalSuitCaseProperties.PropertiesDictionary;


            var newItems = Wrapper.TransportItemsToWorkItems(LocalSuitCaseProperties.PropertiesDictionary);
            TableDetailCollection.Clear();

            foreach (var wi in newItems)
            {
                TableDetailCollection.Add(wi);
            }



          //  TableDetailCollection = Wrapper.TransportItemsToWorkItems(LocalSuitCaseProperties.PropertiesDictionary);

            //TableDetailCollection = new ObservableCollection<WorkItem>(Wrapper.TransportItemsToWorkItems(LocalSuitCaseProperties.PropertiesDictionary));

            //TableDetailCollection = Wrapper.TransportItemsToWorkItems(LocalSuitCaseProperties.PropertiesDictionary);

            //RoSchmi
            /*
            ShadowFileProperties = new ShadowFileSettingItems()
            {
                AccountName = TableDetailCollection.First(WorkItem => WorkItem.Name == "TableAccount").StringValue,
                TableName = TableDetailCollection.First(WorkItem => WorkItem.Name == "CloudTableName").StringValue,
                TableAccount = TableDetailCollection.First(WorkItem => WorkItem.Name == "TableAccount").StringValue,
                ColumnType = TableDetailCollection.First(WorkItem => WorkItem.Name == "Type").StringValue,
                Factor = TableDetailCollection.First(WorkItem => WorkItem.Name == "Factor").StringValue,
                Sender = nameof(ProfileDetailPage),
            };
            */

           // var testThing = tableDetailCollection.First(WorkItem => WorkItem.Name == "ColumnName").StringValue;

            //ColumnName = TableDetailCollection.First(WorkItem => WorkItem.Name == "ColumnName").StringValue,

            //TableDetailCollection.First(WorkItem => WorkItem.Name == "CloudTableName").StringValue = factor;

            //TableDetailCollection = Wrapper.TransportItemsToWorkItems(TableDetailCollection, LocalSuitCaseProperties.PropertiesDictionary);

            ItemCollection = new();

            // profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);

            // TableDetailCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary.First().Value.PropertiesDictionary);

            /*
            foreach (string member in LocalDataSourceProperties.Properties.Keys)
            {
                ItemCollection.Add(member);
            }

            MyDict = new();

            foreach(string member in ItemCollection)
            {
                MyDict.Add(member, LocalDataSourceProperties.Properties[member]);
            }
            */
            
        }
        #endregion

    }

    private object Tuple<T1, T2>(T1 empty1, T2 empty2)
    {
        throw new NotImplementedException();
    }


    [ObservableProperty]
    private Dictionary<string, string> myDict;

    [ObservableProperty]
    private SuitCaseProperties localSuitCaseProperties = new();


    [ObservableProperty]
    private DataSourceProperties localDataSourceProperties;
   
    [ObservableProperty]
    private string navigationState;

    [ObservableProperty]
    private Dictionary<string, DataSourceProperties> propertiesDictionary;

    [ObservableProperty]
    private string textToShow;

    [ObservableProperty]
    private ObservableCollection<string> itemCollection; // = new ObservableCollection<string>() { "Name in Azure Table", "Display Name" };

    [ObservableProperty]
    private static ObservableCollection<WorkItem>? tableDetailCollection = new();           // Is the Binding source of CollectionView of the ProfileDetailPage

    [ObservableProperty]
    private static ObservableCollection<string> allowedPickerItems = new();


    // Constructor
    public ProfileDetailViewModel()
    { 
    }

    public async void OnProfileDetailPageNavigatedToCommand(){
        NavigationState = Shell.Current.CurrentState.Location.ToString();
        int breakpoint78 = 1;
    }

    public async void OnProfileDetailPageBackButtonPressedCommand(){
        int breakpoint777 = 1;
    }

    [ObservableProperty]
    private string entryText = "Guckste, was?";

    [ObservableProperty]
    private ShadowFileSettingItems shadowFileProperties = new ShadowFileSettingItems() { Sender = "ProfileDetailPage" };

    [ObservableProperty]
    private bool itemIsChecked = true;

    [ObservableProperty]
    private string tableName;

    [RelayCommand]
    private async Task PickerOpened()

    {
        // await Application.Current.MainPage.DisplayAlertasync("Alert", "Typing long keys on the keyboard is boring!\r\n" +
        //   "Instead you can email the key to your phone and then use copy and paste.", "OK");
        int breakpoint = 1;
    }


    #region RelayCommand GoToShadows
    [RelayCommand]
    private async Task GoToShadows(object s)
    {
        Dictionary<string, object> navigationParameter = new Dictionary<string, object>()
            {
                {((ShadowFileSettingItems)s).Sender, s},
            };

        try
        {
            await Shell.Current.GoToAsync($"{nameof(ShadowFilePage)}?Sender={nameof(ProfileDetailPage)}", navigationParameter);

            // await Shell.Current.GoToAsync($"{nameof(ShadowFilePage)}");

        }
        catch (Exception ex)
        {
            int breakpoint89 = 1;
        }


        // await Shell.Current.Navigation.PushModalAsync(new ShadowFilePage(((ShadowFileSettingItems)s).Sender, navigationParameter));

        int breakpoint = 1;

    }
    #endregion

    /*
    #region RelayCommand Tap
    [RelayCommand]
    private async void Tap(object s)
    {
        Dictionary<string, object> navigationParameter = new Dictionary<string, object>()
            {
                {((SettingItems)s).Sender, s},
                {"Parameter", ((SettingItems)s).Sender}
            };


        //await Shell.Current.GoToAsync($"{nameof(SettingsDetailPage)}?Parameter={((SettingItems)s).Sender}", false, navigationParameter);

        await Shell.Current.Navigation.PushModalAsync(new SettingsDetailPage(((SettingItems)s).Sender, navigationParameter));

        int breakpoint = 1;
    }
    #endregion
    */

    [RelayCommand]
    private void ClickToDebug()
    {
        //var theDictCopy = LocalDataSourceProperties;

        var theDictCopy = MyDict;
        int dummy56 = 1;
    }

    [RelayCommand]
    async Task Back()   // Back-Arrow in NavigationBar is pressed
    {
        // this can propably be deleted, goes over GoBack

        var thePage = Shell.Current.CurrentPage as ProfileDetailPage;

        var theState = Shell.Current.CurrentState;

        //   var navigationExpression = $"///{nameof(SettingsPage)}?Parameter={s}";

        Dictionary<string, TransportItem> transportItemDictionary = Wrapper.WorkItemsToTransportItems(TableDetailCollection);

        string sender = "ProfileDetailPage";
        var suitCaseProperties = new SuitCaseProperties()
        {
            PropertiesDictionary = transportItemDictionary
        };

        var navigationParameter = new Dictionary<string, object>() {
            { "ProfileDetailPage", suitCaseProperties }
        };



        //   await Shell.Current.GoToAsync($"{theName}", navigationParameter);

        //await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Parameter={sender}", navigationParameter);
        await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Sender={sender}", navigationParameter);


        int breakPont643 = 1;

    }

    [RelayCommand]
    private async Task GoBack()         // Back-Button is pressed
    {
        string sendingPage = nameof(ProfileDetailPage);

        Dictionary<string, TransportItem> transportItemDictionary = Wrapper.WorkItemsToTransportItems(TableDetailCollection, true);

        var suitCaseProperties = new SuitCaseProperties()
        {
            PropertiesDictionary = transportItemDictionary
        };

        var navigationParameter = new Dictionary<string, object>() {
                    { nameof(ProfileDetailPage), suitCaseProperties }
            };

      //await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?Parameter={sender}", navigationParameter);
        await Shell.Current.GoToAsync($"///{nameof(SettingsPage)}?sender={sendingPage}", navigationParameter);
        //await _navigation.GoToAsync(nameof(SettingsPage), navigationParameter);

       // var navigationParameter = new Dictionary<string, object>() {
       //        {"sender", nameof(MainPage)},
       //         { nameof(MainPage), appState }
    }
}

