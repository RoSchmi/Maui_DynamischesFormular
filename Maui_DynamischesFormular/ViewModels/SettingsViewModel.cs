using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui_DynamischesFormular.Cells;
using Maui_DynamischesFormular.Common;
using Maui_DynamischesFormular.Helpers;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.Pages;
using Maui_DynamischesFormular.ViewModels;
using RoSchmi.Maui.Helpers;
using RoSchmi.Maui.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Input;



namespace Maui_DynamischesFormular.ViewModels
{
   
    [QueryProperty("InjectedSender", "sender")]


    public partial class SettingsViewModel : ObservableObject, IQueryAttributable
    {
        // Don't forget to register the viewmodel in 'MauiProgram.cs'
        // Don't forget to set the reference in 'SettingsPage.xaml.cs'

        private INavigationService _navigation;

        private const string accountsFileName = "Accounts.txt";
        private const string appFolder = "ChartSluuk";
        private const string profilesFileName = "Profiles.xml";

        private enum SaveCmdMode
        { Add, Rename, NoChange };



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
            //Profile = "TimeSeriesGroup-1",
            DataGroup = "Profile-1",
            TableAccount1 = "",
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

        private static Dictionary<string, string> ProfileMemberNameAssignation;

        private CultureInfo invariantCulture = CultureInfo.InvariantCulture;

        private const string Delimiter = ";";

        private readonly Regex _accountRegex = new(@"^[a-z0-9]+$");

        private static List<string> tables = new();

        private static TableSection section1 = new("Accounts: Select Account / Delete credentials");

        private static List<string> names = new();

        private static SwitchCellSource switchCellSource = new(names, section1);

        private static string lastSelectedProfile = "";

        private static Dictionary<string, SuitCaseProperties> profilesDictionary;

        private const string AddProfileHeader = "Add new Profile";

        private const string RenameProfileHeader = "Rename Profile";


        private SuitCaseProperties tableDetailProperties = new SuitCaseProperties();

        public ICommand Create_Sample_Tables_Command { get; private set; }

        // Holds the combinations of account and profile names of the profiles of the actually selectd account
        private ObservableCollection<string> profilesExtended = new();

        private ProfileSet actProfileSet;

        [ObservableProperty]
        private string navigationState;

        [ObservableProperty]
        private string injectedSender;

        [ObservableProperty]
        private string sender;

        [ObservableProperty]
        private string newProfileName = "";

        [ObservableProperty]
        private string rename_Command_Header = "Add/Rename Analog Values Profile";

        [ObservableProperty]
        private string addedProfile = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedProfileExtended))]
        private static string selectedProfile = "Profile-1";

        public string SelectedProfileExtended
        {
            get => $"Settings ({ActAccount}, {SelectedProfile})";
            set => SetProperty(ref selectedProfile, value);
        }

        [ObservableProperty]
        private int selectedProfileIndex = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedProfileExtended))]
        private static string actAccount = "";

        [ObservableProperty]
        private string selectedAccount;

        [ObservableProperty]
        private string accountEntry = "";

        [ObservableProperty]
        private string keyEntry = "";

        // ******************************************************

        [ObservableProperty]
        private ObservableCollection<string> profile;                               // Binding to old Settingspage
        //private ObservableCollection<ProfileSet> profile;

        [ObservableProperty]
        private ObservableCollection<string> profileNames = new() { "Profile-1" };  // Holds the names of the profiles of the actually selected account,
                                                                                    // Binding to ComboBox

        [ObservableProperty]
        private static ObservableCollection<WorkItem> workItemCollection;   // Is the Binding source of CollectionView of the SettingsPage

        //[ObservableProperty]
        //private static ObservableCollection<WorkItem> workItemShowCollection = new();

        // ******************************************************



        [ObservableProperty]
        private string settingsPageLeftCommand = "< Show Graphs";

        [ObservableProperty]
        private string settingsPageRightCommand = "Select Account >";


        [ObservableProperty]
        private bool settingsPageLeftCommandIsVisible = true;

        [ObservableProperty]
        private bool settingsPageRightCommandIsVisible = true;

        [ObservableProperty]
        private bool navStateRowIsVisible = true;

        [ObservableProperty]
        private bool profilePickerIsVisible = true;

        [ObservableProperty]
        private bool rename_StackLayout_IsVisible = false;

        [ObservableProperty]
        private TableRoot accountsTableRoot = new();

        [ObservableProperty]
        private string senderMessage;

        //[ObservableProperty]
        //private string key = "";

        [ObservableProperty]
        private bool accountsStacklayoutVisible = false;

        [ObservableProperty]
        private bool settingsStacklayoutVisible = true;

    //    [ObservableProperty]
    //    private int appearCounter = 0;

        [ObservableProperty]
        private Color connectionOKBackGround = Colors.LightGrey;

        /*
        [ObservableProperty]
        private Color cardViewEntryBackGroundColor = EntryBackGroundColorDefault;
        */

        [ObservableProperty]
        private int saveButtonBorderWidth = 0;

        private IDictionary<string, object> queryHandle;

        // Here automatically messages from sending page are processed
        #region Region ApplyQueryAttributes
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            queryHandle = query;  // make a handle to clear the dictionary

            //SenderMessage = query["Parameter"].ToString();

            if (query.ContainsKey("sender"))
            {

                SenderMessage = query["sender"].ToString();

                if (query.ContainsKey(nameof(ProfileDetailPage)))
                {
                    #region Region Sending Page was ProfileDetailPage

                    tableDetailProperties = query[nameof(ProfileDetailPage)] as SuitCaseProperties;

                    ActualizeProfilesWithValuesFromDetailPage(WorkItemCollection, tableDetailProperties.PropertiesDictionary);


                    string profileAndAccount = SelectedProfileIndex >= 0 ? FormattableString.Invariant($"{ActAccount}{Delimiter}{ProfileNames[SelectedProfileIndex]}") : string.Empty;

                    if (SelectedProfileIndex < 0) { 
                        _= Application.Current.MainPage.DisplayAlertAsync("Alert", "SelectedProfileIndex was < 0\n --> was set to 0", "OK");
                        SelectedProfileIndex = 0;
                    }

                    ActualizeProfilesAndWriteXmlFile(WorkItemCollection, ref profilesDictionary, profileAndAccount, appFolder, profilesFileName);

                    // RoSchmi
                    // The next 2 lines are inactivated. (can be used to test that everything worked as expected)

                    // profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);  
                    // WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[profileAndAccount].PropertiesDictionary);

                   

                    int breakpoint_34 = 1;
                    #endregion
                }


                if (query.ContainsKey(nameof(MainPage)))
                {
                    #region Region Sending Page was MainPage
                  
                    SenderMessage = query["sender"].ToString() + ",   AppState: " + query[nameof(MainPage)].ToString();

                    int appState = (int)query["MainPage"];

                    // RoSchmi: this is a trick to run the programm without real Azure Account
                    // should be inactivated for normal action
                    // Set it to your needs
                    //ActAccount = "bog128";


                    switch (appState)
                    {
                        case 0:                          // No account selected
                            {

                                break;
                            }
                        case 1:                          // Account selected but not one initialized Profile present, so we create 'Profile-1'
                            {
                                ProfileSet profSet = JsonSerializer.Deserialize<ProfileSet>(JsonSerializer.Serialize(profileSetDefault));
                                profSet.SettingsID = Guid.NewGuid().ToString();
                                profSet.Account = ActAccount;
                                SuitCaseProperties suitCaseProperties = Wrapper.ProfileSetToSuitCaseProperties(profSet);

                                string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{profSet.Profile}");

                                profilesDictionary = new Dictionary<string, SuitCaseProperties>()
                            {
                                {profileAndAccount, suitCaseProperties},
                            };

                                DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);

                                WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary.First().Value.PropertiesDictionary);

                                // Copy to ObservableCollection with less items to avoid empty space in CollectionView
                                /*
                                WorkItemShowCollection = new ObservableCollection<WorkItem>();
                                for (int i = 0; i < 7; i++)
                                {
                                    WorkItemShowCollection.Add(WorkItemCollection[i]);
                                }
                                */

                                if (!ProfileNames.Contains(profSet.Profile))
                                {
                                    ProfileNames.Add(profSet.Profile);
                                }
                                SelectedProfile = profSet.Profile;
                                lastSelectedProfile = profSet.Profile;
                           
                                break;
                            }

                        case 2:         // Account selected and initialized Profile present but none of them selected (should not happen)
                            {
                                _ = Application.Current.MainPage.DisplayAlertAsync("Alert", "No Profile selected. This should not happen!", "OK");
                                break;
                            }

                        case 3:
                            {
                                _ = Application.Current.MainPage.DisplayAlertAsync("Alert", "Profilenames could not be set. This should not happen!", "OK");

                                break;
                            }

                        case 4:
                            {
                                profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);

                                SelectedProfile = Helper.GetSelectedProfileOfThisAccountFromProfilesDictionary(profilesDictionary, ActAccount, Delimiter);

                                WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[FormattableString.Invariant($"{ActAccount}{Delimiter}{SelectedProfile}")].PropertiesDictionary);

                                bool result = FillProfileNamesAndProfilesExtended(WorkItemCollection, profilesDictionary);

                                SelectedProfileIndex = ProfileNames.IndexOf(SelectedProfile);



                                break;
                            }
                    }

                    int breakpoint_34 = 1;
#endregion
                }
            }
        }
        #endregion

        #region Region FillProfileNamesAndProfilesExtended
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

                   // { "PiTable1ColumnName", pWorkItemCollection[6].Name },
                   // { "PiTable2ColumnName", pWorkItemCollection[18].Name },
                   // { "PiTable3ColumnName", pWorkItemCollection[30].Name },
                   // { "PiTable4ColumnName", pWorkItemCollection[42].Name },
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
#endregion


        #region Region ActualizeProfilesWithValuesFromDetailPage
        private void ActualizeProfilesWithValuesFromDetailPage(ObservableCollection<WorkItem> pWorkItemCollection, Dictionary<string, TransportItem> pPropertiesDictionary)
        {
            foreach(var entry in pPropertiesDictionary)
            {
               // string reconstructedName = entry.Value.TabNo == 0 ? entry.Value.Name : $"{entry.Value.Name}{entry.Value.TabNo}";

                string reconstructedName = entry.Value.TabNo == 0 ? entry.Key : $"{entry.Key}{entry.Value.TabNo}";


                switch (entry.Value.TypeIdentifier)
                {
                    case WorkItem.TypeID.RsString:
                    case WorkItem.TypeID.RsStringRo:
                    case WorkItem.TypeID.RsStringNo:
                    case WorkItem.TypeID.RsStringSw:
                    case WorkItem.TypeID.RsStringPi:
                        {
                            var No3 = pWorkItemCollection.First();
                            var N04 = pPropertiesDictionary;


                            var item = pWorkItemCollection.FirstOrDefault(WorkItem => WorkItem.Name == reconstructedName);
                            if (item != null)
                            {
                                item.StringValue = Wrapper.TransportItemToWorkItem(pPropertiesDictionary[entry.Key]).StringValue;
                                //var No2 = Wrapper.TransportItemToWorkItem(pPropertiesDictionary[entry.Value.Name]).StringValue = Wrapper.TransportItemToWorkItem(pPropertiesDictionary[entry.Value.Name]).StringValue;
                            }
                            else
                            {
                                int debug447 = 1;
                            }
                               // pWorkItemCollection.First(WorkItem => WorkItem.Name == reconstructedName).StringValue = Wrapper.TransportItemToWorkItem(pPropertiesDictionary[entry.Value.Name]).StringValue;
                            break;
                        }

                    case WorkItem.TypeID.RsBoolean:
                    case WorkItem.TypeID.RsBooleanRo:
                    case WorkItem.TypeID.RsBooleanNo:
                        {
                            pWorkItemCollection.First(WorkItem => WorkItem.Name == reconstructedName).BoolValue = Wrapper.TransportItemToWorkItem(pPropertiesDictionary[entry.Value.Name]).BoolValue; ;
                            break;
                        }
                    case WorkItem.TypeID.RsDateTime:
                    case WorkItem.TypeID.RsDateTimeRo:
                    case WorkItem.TypeID.RsDateTimeNo:

                        {
                            pWorkItemCollection.First(WorkItem => WorkItem.Name == reconstructedName).DateValue = Wrapper.TransportItemToWorkItem(pPropertiesDictionary[entry.Value.Name]).DateValue; ;
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException("Not supported DataSourceTable");
                        }
                }
            }    
        }
        #endregion

        #region Region ActualizeProfilesAndWriteXmlFile
        private void ActualizeProfilesAndWriteXmlFile(
                ObservableCollection<WorkItem> pWorkItemCollection, 
                ref Dictionary<string, SuitCaseProperties> pProfilesDictionary, 
                string pProfileAndAccount, 
                string pAppFolder, 
                string pProfilesFileName)
        {
            Dictionary<string, TransportItem> transportItemDictionary = Wrapper.WorkItemsToTransportItems(pWorkItemCollection);

            if (pProfilesDictionary.ContainsKey(pProfileAndAccount))
            {
                bool _ = pProfilesDictionary.Remove(pProfileAndAccount);

                pProfilesDictionary.Add(pProfileAndAccount, new SuitCaseProperties() { PropertiesDictionary = transportItemDictionary });
            }

            DictionaryXML.WriteProfilesDictionaryToXmlFile(pProfilesDictionary, appFolder, profilesFileName);         
        }
        #endregion


        #region constructor
        public SettingsViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            PopulateAccountFilesAction();  // This sets ActAccount
            switchCellSource.SwitchCellSourceSend += SwitchCellSource_SwitchCellSourceSend;

            profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);
            int breakpoint67 = 1;
        }
        #endregion

        private int credentialCounter = 0;

        #region Region Event OnSelectedProfileIndexChange(..)
        partial void OnSelectedProfileIndexChanged(int value)
        {
            if (value >= 0)
            {
                string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{ProfileNames[value]}");
                string lastProfileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{lastSelectedProfile}");

                string localnewProfileName = ProfileNames[value];

                if (profilesDictionary.ContainsKey(profileAndAccount))
                {
                    if (profilesDictionary.ContainsKey(lastProfileAndAccount))
                    {
                        var suitCaseProp = new SuitCaseProperties();

                        if (profilesDictionary.TryGetValue(lastProfileAndAccount, out suitCaseProp))
                        {
                            (suitCaseProp.PropertiesDictionary["Selected"].Content as StringTypeContent).Value = "0";

                            profilesDictionary.Remove(lastProfileAndAccount);
                            profilesDictionary.Add(lastProfileAndAccount, suitCaseProp);
                        }
                    }

                    if (profilesDictionary.ContainsKey(profileAndAccount))
                    {
                        var suitCaseProp = new SuitCaseProperties();

                        if (profilesDictionary.TryGetValue(profileAndAccount, out suitCaseProp))
                        {
                            (suitCaseProp.PropertiesDictionary["Selected"].Content as StringTypeContent).Value = "1";

                            profilesDictionary.Remove(profileAndAccount);
                            profilesDictionary.Add(profileAndAccount, suitCaseProp);
                        }
                    }

                    lastSelectedProfile = localnewProfileName;
                    SelectedProfile = localnewProfileName;

                    var suitCaseProperties = new SuitCaseProperties();

                    if (profilesDictionary.TryGetValue(profileAndAccount, out suitCaseProperties))
                    {

                        WorkItemCollection = Wrapper.TransportItemsToWorkItems(suitCaseProperties.PropertiesDictionary);
                        //WorkItemCollection = Wrapper.TransportItemsToWorkItems(WorkItemCollection, suitCaseProperties.PropertiesDictionary);
                    }

                    DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);
                }
            }
        }
        #endregion


        #region RelayCommand ButtonAddProfileClicked()

        [RelayCommand]
        private async Task Button_Tip_clicked_()
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "Typing long keys on the keyboard is boring!\r\n" +
               "Instead you can email the key to your phone and then use copy and paste.", "OK");
            int breakpoint = 1;
        }

        [RelayCommand]
        private void ButtonAddProfileClicked()
        {
            Rename_Command_Header = AddProfileHeader;
            NewProfileName = string.Empty;
            Rename_StackLayout_IsVisible = true;
            SettingsStacklayoutVisible = false;
        }
        #endregion

        #region RelayCommand_ButtonRenProfileClicked()
        [RelayCommand]
        private async void ButtonRenProfileClicked()
        {
            NewProfileName = "";
            Rename_Command_Header = RenameProfileHeader;
            Rename_StackLayout_IsVisible = true;
            SettingsStacklayoutVisible = false;
        }
        #endregion

        #region RelayCommand ButtonDelProfileClicked()
        [RelayCommand]
        private async void ButtonDelProfileClicked()
        {
            string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{ProfileNames[SelectedProfileIndex]}");

            Rename_StackLayout_IsVisible = false;
            SettingsStacklayoutVisible = false;

            SelectedProfile = ProfileNames[SelectedProfileIndex];  // can be deleted ?

            if (ProfileNames.Count >= 2)
            {
                if (await Application.Current.MainPage.DisplayAlert("Alert", "Delete Profile >" + SelectedProfile + "< ?", "OK", "Cancel"))
                {
                    if (profilesDictionary.ContainsKey(profileAndAccount))
                    {
                        bool _ = profilesDictionary.Remove(profileAndAccount);
                    }

                    if (ProfileNames.Contains(SelectedProfile))
                    {
                        // SelectedProfile must be saved before removing
                        string profileToDelete = SelectedProfile;
                        selectedProfileIndex = -1;
                        SelectedProfileIndex = 0;
                        try
                        {
                            ProfileNames.Remove(profileToDelete);
                            profilesExtended.Remove(profileAndAccount);

                            SelectedProfile = ProfileNames.First();
                            SelectedProfileIndex = ProfileNames.IndexOf(SelectedProfile);

                            string newProfileAndAccount = FormattableString.Invariant($"{actAccount}{Delimiter}{SelectedProfile}");

                            if (profilesDictionary.ContainsKey(newProfileAndAccount))
                            {
                                var suitCaseProp = new SuitCaseProperties();

                                if (profilesDictionary.TryGetValue(newProfileAndAccount, out suitCaseProp))
                                {
                                    (suitCaseProp.PropertiesDictionary["Selected"].Content as StringTypeContent).Value = "1";
                                    profilesDictionary.Remove(newProfileAndAccount);
                                    profilesDictionary.Add(newProfileAndAccount, suitCaseProp);
                                }
                            }
                            DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);

                            profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);
                            WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[newProfileAndAccount].PropertiesDictionary);
                            //WorkItemCollection = Wrapper.TransportItemsToWorkItems(WorkItemCollection, profilesDictionary[newProfileAndAccount].PropertiesDictionary);


                        }
                        catch (Exception ex)
                        {
                            string mess = ex.Message;
                        }
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Last remaining Profile cannot be deleted. Use Rename function instead.", "OK");
            }
        }
        #endregion

        #region RelayCommand Button_RenProfile_Accept_clicked_()
        [RelayCommand]
        private async void Button_RenProfile_Accept_clicked_()
        {

            if (Rename_Command_Header == AddProfileHeader && NewProfileName != string.Empty)
            {
                if (ProfileNames.Contains(NewProfileName))
                {
                    await Shell.Current.DisplayAlert("Not allowed operation", "Profile already exists!", "Ok");
                    return;
                }

                AddedProfile = NewProfileName;
                SaveProfile(SaveCmdMode.Add);
                Rename_StackLayout_IsVisible = false;

            }

            if (Rename_Command_Header == RenameProfileHeader && NewProfileName != string.Empty)
            {
                string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{SelectedProfile}");
                string newProfileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{NewProfileName}");

                if (profilesDictionary.ContainsKey(profileAndAccount))
                {

                    var suitCaseProp = new SuitCaseProperties();

                    // RoSchmi ToDo Try to get rid of DisplayName bzw. Profile

                    if (profilesDictionary.TryGetValue(profileAndAccount, out suitCaseProp))
                    {
                        //(suitCaseProp.PropertiesDictionary["DisplayName"].Content as StringTypeContent).Value = NewProfileName;
                        (suitCaseProp.PropertiesDictionary["Profile"].Content as StringTypeContent).Value = NewProfileName;


                        profilesDictionary.Remove(profileAndAccount);
                        profilesDictionary.Add(newProfileAndAccount, suitCaseProp);

                        ProfileNames.Remove(SelectedProfile);
                        ProfileNames.Add(newProfileName);
                        SelectedProfile = newProfileName;
                        SelectedProfileIndex = ProfileNames.IndexOf(NewProfileName);

                        profilesExtended.Remove(profileAndAccount);
                        profilesExtended.Add(FormattableString.Invariant($"{ActAccount}{Delimiter}{NewProfileName}"));
                        int breakpoint75762 = 1;
                    }
                }
                // RoSchmi
                DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);

                profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);

                WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[newProfileAndAccount].PropertiesDictionary);
                //WorkItemCollection = Wrapper.TransportItemsToWorkItems(WorkItemCollection, profilesDictionary[newProfileAndAccount].PropertiesDictionary);

                Rename_StackLayout_IsVisible = false;
            }
        }
        #endregion

        #region RelayCommand Button_RenProfile_Cancel_clicked_()
        [RelayCommand]
        private async void Button_RenProfile_Cancel_clicked_()
        {
            Rename_StackLayout_IsVisible = false;
            NewProfileName = "";
        }
        #endregion

        #region Region Tap
        [RelayCommand]
        private async void Tap(WorkItem actWorkItem)
        {
            // SelectedWorkItemCollection.Clear();
            // SelectedWorkItemCollection.Add(s);

            string targetPage = nameof(ProfileDetailPage);
            string injectedParameter = actWorkItem.Name;

            
            var ID = Guid.NewGuid().ToString();
            
            var selectedItems = new Dictionary<string, TransportItem>()
            {
                {"Account", new TransportItem()        {Name = "Account",               DisplayName = "Account",         TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = string.Empty } } },
                {"SettingsID", new TransportItem()     {Name = nameof(actWorkItem.Name),DisplayName = "SettingsID",      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = ID } } },
                {"Profile", new TransportItem()        {Name = "Profile",               DisplayName = "Profile",         TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = SelectedProfile } } },
                {"DataGroup", new TransportItem()      {Name = string.Empty,            DisplayName = "Daten Gruppe",     TabNo = 0,     TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent(){ Value = SelectedProfile } } },
                {"Table-ID", new TransportItem()       {Name = "Table-ID",              DisplayName = "Table-ID",        TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = actWorkItem.Name } } },            
                {"TableAccount", new TransportItem()   {Name = string.Empty,            DisplayName = "TableAccount",    TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"DataSourceTable", new TransportItem(){Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"TableProperty", new TransportItem()  {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"TableSortField", new TransportItem() {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"TableFactor", new TransportItem()    {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"TableOffset", new TransportItem()    {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"TableType", new TransportItem()      {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsStringPi, Content = new StringTypeContent() { Value = string.Empty } } },
                {"TableUnit", new TransportItem()      {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsString,   Content = new StringTypeContent() { Value = string.Empty } } },
                {"SettingsState", new TransportItem()  {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsBoolean,  Content = new BoolTypeContent()  { Value = null } } },
                {"SettingsDate", new TransportItem()   {Name = string.Empty,            DisplayName = string.Empty,      TabNo = 0,      TypeIdentifier = WorkItem.TypeID.RsDateTime, Content = new DateTimeTypeContent() { Value = null } } },
             };

            int selector = 0;
            switch (actWorkItem.Name)
            {
                case "DataSourceTable1": { selector = 1; break; }
                case "DataSourceTable2": { selector = 2; break; }
                case "DataSourceTable3": { selector = 3; break; }
                case "DataSourceTable4": { selector = 4; break; }         
                default:
                    {
                        throw new NotSupportedException("Not supported DataSourceTable");
                    } 
            }

            #region Region foreach(....) Fill selected items for DetailsPage with values from ProfileSet
            foreach (var actItem in WorkItemCollection)
            {
                if (actItem.TabNo != 0 && actItem.TabNo != selector)
                    continue;

                if (actItem.TabNo == 0)
                {
                    int breakpoint22 = 1;
                }

                // Wenn TabNo nicht 0 ist, nach dem um eine Stelle gekürzten String suchen (letzte Stelle repräsentiert Tabellenzugehörigkeit)  
                var baseName = actItem.TabNo == 0 ? actItem.Name : string.IsNullOrEmpty(actItem.Name) ? string.Empty : actItem.Name[..^1];

                if (selectedItems.TryGetValue(baseName, out var cop))
                {
                    cop.TabNo = actItem.TabNo;
                    cop.Name = actItem.Name;
                    cop.DisplayName = actItem.DisplayName;

                    switch (actItem.TypeIdentifier)
                    {
                        case WorkItem.TypeID.RsString:
                        case WorkItem.TypeID.RsStringRo:
                        case WorkItem.TypeID.RsStringNo:
                        case WorkItem.TypeID.RsStringSw:
                        case WorkItem.TypeID.RsStringPi:
                            {
                                cop.Content = new StringTypeContent() { Value = actItem.StringValue };
                                break;
                            }
                        case WorkItem.TypeID.RsBoolean:
                        case WorkItem.TypeID.RsBooleanRo:
                        case WorkItem.TypeID.RsBooleanNo:
                            {
                                cop.Content = new BoolTypeContent() { Value = actItem.BoolValue };
                                break;
                            }
                        case WorkItem.TypeID.RsDateTime:
                        case WorkItem.TypeID.RsDateTimeRo:
                        case WorkItem.TypeID.RsDateTimeNo:
                            {
                                cop.Content = new DateTimeTypeContent() { Value = actItem.DateValue < DateTime.Now.AddDays(-1500) ? DateTime.Now : actItem.DateValue };
                                break;
                            }
                        default:
                            {
                                break;
                                //throw new ArgumentOutOfRangeException("SettingsViewModel:" + actItem.TypeIdentifier.ToString());
                            }
                    }
                }
            }
            #endregion



            //var ID = Guid.NewGuid().ToString();

            tableDetailProperties = new SuitCaseProperties()
            {
                PropertiesDictionary = new Dictionary<string, TransportItem>(selectedItems)
            };
                // properties get wrapped in this inner Dictionary

                /*
                PropertiesDictionary = new Dictionary<string, TransportItem>()
                {
                    { "ColumnName", new TransportItem() { Name = "ColumnName", DisplayName =  selectedItems["columnName"].DisplayName, TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = selectedItems["columnName"].Name } } },

                }
                */
                /*
                PropertiesDictionary = new Dictionary<string, TransportItem>() {
                    { "SettingsID", new TransportItem() { Name = "SettingsID", TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = ID } } },
                    { "Table-ID", new TransportItem() { Name = "Table-ID", TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = actWorkItem.Name } } },
                    { "TableAccount", new TransportItem() { Name = "TableAccount", TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = tableAccount.Name } } },
                    { "CloudTableName", new TransportItem() { Name = "CloudTableName", TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = tableName.Name } } },
                    { "ColumnName", new TransportItem() { Name = "ColumnName", DisplayName = columnName.DisplayName, TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = columnName.Name } } },
                    { "Factor", new TransportItem() { Name = "Factor", TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = factor.Name } } },
                    { "Offset", new TransportItem() { Name = "Offset", TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = factor.Name } } },
                    { "Unit", new TransportItem() { Name = "Unit", TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = unit.Name } } },
                    { "Type", new TransportItem() { Name = "Type", TypeIdentifier = WorkItem.TypeID.RsStringPi, Content = new StringTypeContent() { Value = type.Name } } },
                    { "SortField", new TransportItem() { Name = "SortField", TypeIdentifier = WorkItem.TypeID.RsString, Content = new StringTypeContent() { Value = sortField.Name } } },
                    { "SettingsState", new TransportItem() { Name = "SettingsState", TypeIdentifier = WorkItem.TypeID.RsBoolean, Content = new BoolTypeContent() { Value = null } } },
                    { "SettingsDate", new TransportItem() { Name = "SettingsDate", TypeIdentifier = WorkItem.TypeID.RsDateTime, Content = new DateTimeTypeContent() { Value = null } } },
                }
                */

            

            var navigationParameter = new Dictionary<string, object>();

            navigationParameter = new Dictionary<string, object>() {
                    { "SettingsPage", tableDetailProperties }
            };


            await Shell.Current.GoToAsync($"{nameof(ProfileDetailPage)}?Sender={actWorkItem.Name}", navigationParameter);

            int breakpoint793 = 1;
        }
        #endregion

        #region Method add Profile to Dictinoary

        private Dictionary<string, SuitCaseProperties> AddProfileToDictionary(Dictionary<string, SuitCaseProperties> dictionary, string pProfileName, string pAccountName, string pDelimiter, ProfileSet pProfile)
        {
            string profileNamePlusAccount = FormattableString.Invariant($"{pAccountName}{pDelimiter}{pProfileName}");


            if (profilesDictionary.ContainsKey(profileNamePlusAccount))
            {
                profilesDictionary.Remove(profileNamePlusAccount);
            }

            // Aufgabe: aus ProfileSet transportItem machen

            ProfileSet newProfileSet = new();

            newProfileSet = JsonSerializer.Deserialize<ProfileSet>(JsonSerializer.Serialize(pProfile));

            SuitCaseProperties suitCaseProperties = Wrapper.ProfileSetToSuitCaseProperties(newProfileSet);

            dictionary.Add(profileNamePlusAccount, suitCaseProperties);

            return dictionary;
            int breakPoint756546 = 1;
        }
        #endregion


        #region Region Method getProfileSet
        ProfileSet getProfileSet(string pName, string pAccount, string pSelected, string pIndex = "0")
        {
            ProfileSet NewProfile = new ProfileSet()
            {
                SettingsID = Guid.NewGuid().ToString(),
                Profile = pName,
                Account = pAccount,
                Selected = pSelected,
                Index = pIndex,

            };

            return NewProfile;
        }
        #endregion

        #region Region SaveProfile
        async void SaveProfile(SaveCmdMode saveCmdMode = SaveCmdMode.NoChange)
        {
            switch (saveCmdMode)
            {
                case SaveCmdMode.Add:
                    {
                        var newProfile = getProfileSet(addedProfile, ActAccount, "1");
                        // wir haben jetzt ein neues ProfileSet mit einigen besetzten members

                        profilesDictionary = AddProfileToDictionary(profilesDictionary, AddedProfile, ActAccount, Delimiter, newProfile);

                        SuitCaseProperties suitCaseProperties = Wrapper.ProfileSetToSuitCaseProperties(newProfile);

                        WorkItemCollection = Wrapper.TransportItemsToWorkItems(suitCaseProperties.PropertiesDictionary);

                        ProfileNames.Add(AddedProfile);
                        // RoSchmi
                        selectedProfileIndex = -1;
                        SelectedProfileIndex = ProfileNames.IndexOf(AddedProfile);
                        SelectedProfile = AddedProfile;

                        break;
                    }
                case SaveCmdMode.NoChange:
                case SaveCmdMode.Rename:
                    {
                        var newProfile = getProfileSet(SelectedProfile, ActAccount, "1");
                        profilesDictionary = AddProfileToDictionary(profilesDictionary, SelectedProfile, ActAccount, ".", newProfile);
                        break;
                    }
            }

            // Write Dictionary to XML-File

            DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);



            //  WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[AddedProfile].PropertiesDictionary);

            int dummy3 = 1;
            //DictionaryXML.WriteProfilesDictionaryToXmlFile   WriteDictionaryToXML(profilesDictionary, "Profiles.xml");

            // activate for tests
            //profilesDictionary = MyDictionaryXML.GetDictionaryFromXML("Profiles.xml");
            //int dummy3 = 1;
        }
        #endregion


        [RelayCommand]
        private async void Button_Debug_clicked_()
        {

            var theIndex = SelectedProfileIndex;
            var theCopy = tableDetailProperties;
            var accountCopy = ActAccount;
            int breakpoint8627 = 1;
        }


        #region Region RelayCommand ButtonSaveChangesClicked
        [RelayCommand]
        private async void ButtonSaveChangesClicked()
        {
            try
            {
                string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{ProfileNames[SelectedProfileIndex]}");
                string lastProfileAndAccount = FormattableString.Invariant($"{ActAccount}.{lastSelectedProfile}");

                ActualizeProfilesAndWriteXmlFile(WorkItemCollection, ref profilesDictionary, profileAndAccount, appFolder, profilesFileName);

                profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);

                //WorkItemCollection = Wrapper.TransportItemsToWorkItems(WorkItemCollection, profilesDictionary[profileAndAccount].PropertiesDictionary);
                WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[profileAndAccount].PropertiesDictionary);


                


                int breakpoint56 = 1;
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
        }
        #endregion


        #region ButtonTestConnectionClicked()  outcommented
        /*
        [RelayCommand]
        private async void ButtonTestConnectionClicked()
        {
            ConnectionOKBackGround = Colors.LightGrey;
            if (names.Count > 0 && ActAccount != string.Empty)
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;

                string outString = string.Empty;
                bool testResult = true;

                if (accessType != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Network", "No Internet", "Ok");
                    ConnectionOKBackGround = Colors.DarkRed;
                    await Task.Delay(3000);
                    ConnectionOKBackGround = Colors.Gray;
                    return;
                }


                string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + ActAccount + ";AccountKey=" + await SecureStorage.GetAsync(ActAccount);


                TableServiceClient serviceClient = null;

                try
                {
                    serviceClient = new TableServiceClient(connectionString);
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("TableServiceClient", "Error: " + ex.Message, "Ok");
                    ConnectionOKBackGround = Colors.DarkRed;
                    await Task.Delay(3000);
                    ConnectionOKBackGround = Colors.Gray;
                    return;
                }

                List<string> tableNameList = new();

                string continuationToken = null;

                var cancellationTokenSource = new CancellationTokenSource(10000);


                // See how it works
                //https://briancaos.wordpress.com/2022/11/11/c-azure-table-storage-queryasync-paging-and-filtering/

                Azure.AsyncPageable<Azure.Data.Tables.Models.TableItem> tablesList;
                try
                {
                    tablesList = serviceClient.QueryAsync(filter: "", maxPerPage: 20, cancellationTokenSource.Token);
                    await foreach (Azure.Page<Azure.Data.Tables.Models.TableItem> page in tablesList.AsPages(continuationToken))
                    {
                        foreach (Azure.Data.Tables.Models.TableItem tableItem in page.Values)
                        {
                            tableNameList.Add(tableItem.Name);
                        }

                        continuationToken = page.ContinuationToken;
                        if (tableNameList.Count > 500)
                        {
                            cancellationTokenSource.Cancel(); // As we only want to test, we stop after the first batch
                        }
                    }

                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("TableServiceClient", ex.Message, "Ok");
                    ConnectionOKBackGround = Colors.DarkRed;
                    await Task.Delay(3000);
                    ConnectionOKBackGround = Colors.Gray;
                    return;
                }

                

                ConnectionOKBackGround = Colors.LightGreen;
                await Task.Delay(3000);
                ConnectionOKBackGround = Colors.Gray;
            }
            else
            {
                await Shell.Current.DisplayAlert("Access", "No Account selected", "Ok");
                ConnectionOKBackGround = Colors.DarkRed;
                await Task.Delay(3000);
                ConnectionOKBackGround = Colors.Gray;
            }


        }
        */
        #endregion


        [RelayCommand]
        private async void Button_AddAbitrary_clicked_()
        {
            credentialCounter++;
            AccountEntry = "roschmi" + credentialCounter.ToString();
            KeyEntry = "MyKey" + credentialCounter.ToString();
        }

        [RelayCommand]
        private async void Button_AddCredentials_clicked_()
        {
            AccountEntry = AccountEntry.TrimEnd(new char[1] { ' ' });
            if (_accountRegex.IsMatch(AccountEntry) && (AccountEntry.Length > 3))
            {
                if (names != null && names.Contains(AccountEntry))
                {
                    names.Remove(AccountEntry);
                }

                names.Insert(0, AccountEntry);

                bool removeResult = SecureStorage.Remove(names[0]);

                await SecureStorage.SetAsync(names[0], KeyEntry);

                // var readBack = await SecureStorage.GetAsync(names[0]);

                KeyEntry = "";
                AccountEntry = "";
                AccountsTableRoot.Clear();
                switchCellSource.Populate(names);
                AccountsTableRoot.Add(section1);
                SelectedAccount = names.First();

                //RoSchmi
                ActAccount = SelectedAccount;

                await WriteListToFile(names, appFolder, accountsFileName);

                // RoSchmi
                ProfileSet profSet = JsonSerializer.Deserialize<ProfileSet>(JsonSerializer.Serialize(profileSetDefault));
                profSet.SettingsID = Guid.NewGuid().ToString();
                profSet.Account = ActAccount;
                SuitCaseProperties suitCaseProperties = Wrapper.ProfileSetToSuitCaseProperties(profSet);
                string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{profSet.Profile}");
                profilesDictionary = AddProfileToDictionary(profilesDictionary, profSet.Profile, ActAccount, Delimiter, profSet);

                DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);
                WorkItemCollection = Wrapper.TransportItemsToWorkItems(suitCaseProperties.PropertiesDictionary);
                ProfileNames.Clear();
                ProfileNames.Add(profSet.Profile);
                SelectedProfileIndex = ProfileNames.IndexOf(profSet.Profile);
                SelectedProfile = AddedProfile;
                var theCopy = WorkItemCollection;

                int breakpoint = 1;



                PopulateAccountFilesAction();

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Wrong format for Account Name: Must be 3 - 24 lowercase letters and numbers", "OK");
            }
        }

        #region Method PopulateAccountFilesAction
        
        public void PopulateAccountFilesAction()
        {
            names = new List<string>();
            names = AccountHelper.GetAccountsListFromFile(appFolder, accountsFileName);
            KeyEntry = string.Empty;
            AccountEntry = string.Empty;

            AccountsTableRoot.Clear();
            switchCellSource.Populate(names);
            AccountsTableRoot.Add(section1);
            SelectedAccount = names.Count > 0 ? names.First() : "Not selected";
            ActAccount = (names.Count > 0) ? names[0] : string.Empty;
        }
        
        #endregion

        #region Event SwitchCellSource_SwitchCellSourceSend
        
        private async void SwitchCellSource_SwitchCellSourceSend(SwitchCellSource sender, SwitchCellSource.SwitchCellSourceEventArgs e)
        {
            switch (e.Action)
            {
                case SwitchCellSource.CellAction.select:
                    {
                        ActAccount = e.ItemName;

                        if (names.Contains(ActAccount))
                        {
                            names.Remove(ActAccount);
                        }

                        names.Remove(ActAccount);
                        names.Insert(0, ActAccount);
                        switchCellSource.Populate(names);
                        SelectedAccount = names.First();
                        ConnectionOKBackGround = Colors.LightGray;
                        await WriteListToFile(names, appFolder, accountsFileName);

                        // Actualize ProfileNames and WorkItemCollection
                        SelectedProfile = Helper.GetSelectedProfileOfThisAccountFromProfilesDictionary(profilesDictionary, ActAccount, Delimiter);

                        if (selectedProfile != null)
                        {

                            WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[FormattableString.Invariant($"{ActAccount}{Delimiter}{SelectedProfile}")].PropertiesDictionary);
                        }

                        bool result = FillProfileNamesAndProfilesExtended(WorkItemCollection, profilesDictionary);

                        SelectedProfileIndex = SelectedProfile != null ? ProfileNames.IndexOf(SelectedProfile) : 0;

                        int breakpoint = 1;

                    }

                    break;

                case SwitchCellSource.CellAction.delete:
                    {
                        bool OkCancelResult = await Application.Current.MainPage.DisplayAlert("Alert", "Delete Item ?", "OK", "Cancel");
                        if (OkCancelResult)
                        {
                            string theItem = "";

                            if (e.ItemName != null)
                            {
                                if (e.ItemName.Length > 6)
                                {
                                    theItem = e.ItemName.Substring(0, length: e.ItemName.Length - 6);
                                }
                            }
                            try
                            {
                                if (!SecureStorage.Remove(ActAccount))
                                {
                                    await Application.Current.MainPage.DisplayAlert("Alert", "Key could not be deleted", "OK");
                                }
                                else
                                {
                                    var OkCancelResult_2 = await Application.Current.MainPage.DisplayAlert("Alert", "Preserve Profiles of deleted Account ?", "Preserve", "Delete");
                                    if (OkCancelResult_2 == false)
                                    {
                                        // Delete all Profiles of the deleted account                               
                                        var theKeys = profilesDictionary.Keys;
                                        List<string> keysToDelete = new List<string>();
                                        foreach (string myKey in theKeys)
                                        {
                                            if (myKey.IndexOf(FormattableString.Invariant($"{ActAccount}{Delimiter}")) >= 0)
                                            {
                                                keysToDelete.Add(myKey);
                                            }
                                        }
                                        foreach (string deleteKey in keysToDelete)
                                        {
                                            if (profilesDictionary.ContainsKey(deleteKey))
                                            {
                                                profilesDictionary.Remove(deleteKey);
                                            }
                                        }
                                        DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);

                                        // RoSchmi

                                        int breakpoint = 1;


                                    }
                                    // RoSchmi
                                    //var namesCopy = names[0];
                                    //SelectedProfile = Helper.GetSelectedProfileOfThisAccountFromProfilesDictionary(profilesDictionary, ActAccount, Delimiter);
                                    //int breakpoint2 = 1;
                                }
                            }
                            catch (Exception ex)
                            {
#if DEBUG
                                Console.WriteLine(ex.Message);
#endif
                            }

                            int index = names.FindIndex(x => x == e.ItemName);

                            if ((index < names.Count) && (index != -1))
                            {
                                names.RemoveAt(index);
                            }

                            AccountsTableRoot.Clear();
                            switchCellSource.Populate(names);
                            AccountsTableRoot.Add(section1);


                            ActAccount = names.Count > 0 ? names.First() : "Not selected";
                            SelectedAccount = names.Count > 0 ? names.First() : "Not selected ";

                            await WriteListToFile(names, appFolder, accountsFileName);


                            SelectedProfile = Helper.GetSelectedProfileOfThisAccountFromProfilesDictionary(profilesDictionary, ActAccount, Delimiter);

                            WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[FormattableString.Invariant($"{ActAccount}{Delimiter}{SelectedProfile}")].PropertiesDictionary);

                            //WorkItemCollection[0] = new WorkItem() { Name= "Account",  StringValue = "ropok01", TypeIdentifier = WorkItem.TypeID.RsStringRo };

                            string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{SelectedProfile}");

                            ActualizeProfilesAndWriteXmlFile(WorkItemCollection, ref profilesDictionary, profileAndAccount, appFolder, profilesFileName);

                            // profilesDictionary = DictionaryXML.GetProfilesDictionaryFromXmlFile(appFolder, profilesFileName);

                            // WorkItemCollection = Wrapper.TransportItemsToWorkItems(profilesDictionary[profileAndAccount].PropertiesDictionary);

                            bool result = FillProfileNamesAndProfilesExtended(WorkItemCollection, profilesDictionary);

                            SelectedProfileIndex = ProfileNames.IndexOf(SelectedProfile);


                            int breakpoint56 = 1;
                        }
                    }

                    break;

                case SwitchCellSource.CellAction.leave:
                    { }  // do nothing

                    break;

                default:
                    { } // do nothing

                    break;
            }
        }
        
        #endregion

        #region Task Get List of tables
        private async Task<List<string>> GetListOfTables(string pAccount)
        {
            /*
            string sessionToken = await SecureStorage.GetAsync(pAccount);
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + pAccount + ";AccountKey=" + sessionToken;
            bool validStorageAccount = false;
            CloudStorageAccount storageAccount = null;

            Exception CreateStorageAccountException = null;
            try
            {
                storageAccount = Common.CreateStorageAccountFromConnectionString(connectionString);
                validStorageAccount = true;
                connectionString = "555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555";
            }
            catch (Exception ex0)
            {
                CreateStorageAccountException = ex0;
            }
            if (validStorageAccount)
            {
                var tableQueryResponse = new TableQueryResponse() { ErrorMessage = "", ListResult = new List<string>() };

                CloudTableClient tableClient;
                try
                {
                    tableClient = storageAccount.CreateCloudTableClient();
                    try
                    {
                        tableQueryResponse = await Common.ListTablesWithPrefix(tableClient, 200, "");
                    }
                    catch (Exception ex7)
                    {

                        GetTableListIsRunning = false;
                    }

                    List<string> tables = tableQueryResponse.ListResult;

                    return tables;

                }
                catch (Exception ex1)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Could not get Table List\r\n" + ex1.Message, "OK");
                    ActAccount = "Not selected";

                    return tables;
                }
            }
            else
            {
                
                // tables.Clear();
                // ListView_Tables.ItemsSource = tables;
                // await DisplayAlert("Alert", "Couldn't open Storageaccount\r\n" + CreateStorageAccountException.Message, "OK");
                
                return null;
            }
            */
            return null;
        }
        #endregion

        [RelayCommand]
        private void ButtonSettingsRightCommandClicked()
        {
            if (SettingsStacklayoutVisible)
            {
                SettingsPageRightCommand = "Ready";
                SettingsPageLeftCommandIsVisible = false;
                AccountsStacklayoutVisible = true;
                SettingsStacklayoutVisible = false;
            }
            else
            {
                SettingsPageRightCommand = "Select Account >";
                SettingsPageLeftCommand = "< Show Graphs";
                SettingsPageLeftCommandIsVisible = true;
                AccountsStacklayoutVisible = false;
                SettingsStacklayoutVisible = true;

            }


            //SettingsPageRightCommandIsVisible = false;
            //SettingsPageRightCommand = "<Ready";
            //SettingsPageLeftCommand = "< Table Settings";
        }

        [RelayCommand]
        private async Task ButtonSettingsLeftCommandClicked()
        {
            if (SettingsStacklayoutVisible)
            {
                ShowGraphs();
            }
            else
            {
                AccountsStacklayoutVisible = false;
                SettingsStacklayoutVisible = true;
                SettingsPageLeftCommand = "< Show Graphs";
                SettingsPageRightCommandIsVisible = true;

                Dictionary<string, SuitCaseProperties>.KeyCollection theKeys = profilesDictionary.Keys;

                bool accountProfileFound = false;
                foreach (string myKey in theKeys)
                {
                    if (myKey.StartsWith(ActAccount, comparisonType: StringComparison.InvariantCulture))
                    {
                        string[] splitted = myKey.Split(Delimiter);
                        if (splitted[0] == ActAccount)
                        {
                            accountProfileFound = true;
                        }
                    }
                }

                if (!accountProfileFound)
                {
                    ProfileSet profSet = JsonSerializer.Deserialize<ProfileSet>(JsonSerializer.Serialize(profileSetDefault));
                    profSet.SettingsID = Guid.NewGuid().ToString();
                    profSet.Account = ActAccount;
                    SuitCaseProperties suitCaseProperties = Wrapper.ProfileSetToSuitCaseProperties(profSet);

                    string profileAndAccount = FormattableString.Invariant($"{ActAccount}{Delimiter}{profSet.Profile}");

                    profilesDictionary = AddProfileToDictionary(profilesDictionary, "Profile_1", ActAccount, Delimiter, profSet);

                  //  DictionaryXML.WriteProfilesDictionaryToXmlFile(profilesDictionary, appFolder, profilesFileName);


                    int breakpoint = 0;
                }
            }
        }

        [RelayCommand]
        private void Button_Pupulate()
        {
           // PopulateAccountFilesAction();
        }

        [RelayCommand]
        private async Task Back()
        {

            var theState = Shell.Current.CurrentState;
            var thePage = Shell.Current.CurrentPage as SettingsPage;
            int breakpoint = 1;
            /*
            await Shell.Current.Navigation.PopToRootAsync();
            await Shell.Current.Navigation.PushAsync(thePage);
            theState = Shell.Current.CurrentState;
            await Shell.Current.GoToAsync($"//MainPage");
            */
        }

        [RelayCommand]
        private async void ShowGraphs()
        {
            var theState = Shell.Current.CurrentState;
            var thePage = Shell.Current.CurrentPage as SettingsPage;


            Dictionary<string, object> navigationParameter = new()
            {
                {nameof(SettingsPage), new object()}

            };

           // await _navigation.GoToAsync(nameof(MainPage), navigationParameter);

            await Shell.Current.GoToAsync($"///{nameof(MainPage)}?sender={nameof(SettingsPage)}", false, navigationParameter);
        }

        #region Region Task WriteListToFile
        // Writes the Account list to a file
        private static async Task WriteListToFile(List<string> pNames, string pFolderName, string pFileName)
        {
            string rootPath = FileSystem.Current.AppDataDirectory;
            string folderPath = Path.Combine(rootPath, pFolderName);
            string filePath = Path.Combine(folderPath, pFileName);

            string accountsString = string.Empty;
            for (int i = 0; i < pNames.Count; i++)
            {
                accountsString += pNames[i] + ',';
            }

            if (accountsString.Length > 0)
            {
                accountsString = accountsString.Remove(accountsString.Length - 1);
            }

            DirectoryInfo _ = Directory.CreateDirectory(folderPath);

            try
            {
                File.WriteAllText(filePath, accountsString);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Could not store Accounts-List \r\n" + ex.Message, "OK");
            }
        }
        #endregion

        

        public void OnNavigatedTo(NavigatedToEventArgs e)
        {
            NavigationState = Shell.Current.CurrentState.Location.ToString();

            Sender = InjectedSender;
            InjectedSender = string.Empty;

            /*
            if (queryHandle != null)
            {
                queryHandle.Clear();
            }
            */
            int dummy45 = 1;
        }


        

        #region EventArgs
        public class DisplayAlertEventArgs : EventArgs
        {
            private string _id;
            private string _caption;
            private string _message;
            private string _acceptString;
            private string _cancelString;

            public DisplayAlertEventArgs(string pId, string pCaption, string pMessage, string pAcceptString, string pCancelString)
            {
                _id = pId;
                _caption = pCaption;
                _message = pMessage;
                _acceptString = pAcceptString;
                _cancelString = pCancelString;
            }

            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string Caption
            {
                get { return _caption; }
                set { _caption = value; }

            }
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }

            public string AcceptString
            {
                get { return _acceptString; }
                set { _acceptString = value; }
            }

            public string CancelString
            {
                get { return _cancelString; }
                set { _cancelString = value; }
            }

        }
        #endregion
    }

}
