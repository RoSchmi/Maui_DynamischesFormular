//using Android.Media.TV;
//using AndroidX.Navigation;
using Maui_DynamischesFormular.Helpers;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.Pages;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Maui_DynamischesFormular.ViewModels;

public partial class ShadowFileViewModel : ObservableObject, IQueryAttributable
{
    private const string appFolder = "ChartSluuk";
    private string rootPath = FileSystem.Current.AppDataDirectory;

    private IConnectivity connectivity;

    private static readonly IFormatProvider formatProviderInvariantDateTime = CultureInfo.InvariantCulture.DateTimeFormat;
    private static readonly IFormatProvider formatProviderCurrentDateTime = CultureInfo.CurrentCulture.DateTimeFormat;
    private static readonly IFormatProvider formatProviderInvariantNumber = CultureInfo.InvariantCulture.NumberFormat;

    private float[] actYearValues = new float[366];
    private float[] actYearShadowValues = new float[366];

    private ShadowFileSettingItems shadowFileSettingItems { get; set; }
    public object TransmissionObject { get; set; }

    [ObservableProperty]
    private string shadowOriginalCreationDateUtc;

    [ObservableProperty]
    private string navigationState;

    [ObservableProperty]
    private string injectedSender;

    [ObservableProperty]
    private string cloudStorageAccount;

    [ObservableProperty]
    private string tableName;

    [ObservableProperty]
    private string columnName;

    [ObservableProperty]
    private string tableYearString = "0001";

    //[ObservableProperty]
    //private string columnType;

    //[ObservableProperty]
    //private string factor;

    //private Tuple<DateTime, float, float, float> DayValuesTuple; // = new Tuple<DateTime, float, float, float>();

    [ObservableProperty]
    private static bool activityIndicatorIsVisible;

    [ObservableProperty]
    private static bool navStateIsVisible = false;

    [ObservableProperty]
    private static bool upperPartIsVisible = true;

    [ObservableProperty]
    private static bool activityIndicatorIsRunning = false;

    [ObservableProperty]
    private ObservableCollection<DayValuesSet> yearTableValueCollection;

    #region Region ApplyQueryAttributes
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        shadowFileSettingItems = query[nameof(ProfileDetailPage)] as ShadowFileSettingItems;

        CloudStorageAccount = shadowFileSettingItems.AccountName;
        TableName = shadowFileSettingItems.TableName;
        ColumnName = shadowFileSettingItems.ColumnName;


        await InitializeValuesTable();

        int breakpoint56 = 1;
    }
    #endregion

    #region RelayCommand StoreChanges to file
    [RelayCommand]
    public async Task StoreChanges()
    {
        var changedItemsDictionary = new Dictionary<int, string>();

        for (int i = 0; i < YearTableValueCollection.Count; i++)
        {
            if (YearTableValueCollection[i].IsCorrected)
            {
                changedItemsDictionary.Add(i, YearTableValueCollection[i].ShadowValueCorrected);
            }
        }

        string xmlChangesFileName = FormattableString.Invariant($"{CloudStorageAccount}.{TableName}.{ColumnName}.Changes.xml");

        Dictionary<string, string> changedYearValuesXmlDictionary = new()
            {
                { "OriginalFileName", xmlChangesFileName },
                { "TargetProgram", "ChartSluuk" },
                { "Version", "1.0.0" },
                { "ShadowOriginalCreationDateUtc", ShadowOriginalCreationDateUtc },
                { "Acount", CloudStorageAccount },
                { "Table", TableName },
                { "TableYear", TableYearString },
                { "Column", ColumnName },
                { "Factor", shadowFileSettingItems.Factor },
                { "Type", shadowFileSettingItems.ColumnType },
                { "JsonContent",  string.Empty}
            };

        changedYearValuesXmlDictionary["JsonContent"] = System.Text.Json.JsonSerializer.Serialize(changedItemsDictionary);

        DictionaryXML.WriteDictionaryStringStringToXmlFile(changedYearValuesXmlDictionary, "CorrectedDayValues", appFolder, xmlChangesFileName);
    }
    #endregion

    #region Region CheckedChanged Can be deleted?
    [RelayCommand]
    public void CheckedChanged()
    {
        int breakpoint59 = 1;
    }
    #endregion

    #region Region RelayCommand Debug
    [RelayCommand]
    public void Debug()
    {
        var theArryCopy = actYearValues;
        var theCopy = YearTableValueCollection;
        int breakpoint61 = 1;
    }
    #endregion

    #region Region RelayCommand Back()
    [RelayCommand]
    public async Task Back()
    {
        string state = Shell.Current.CurrentState.Location.ToString();
        int breakpoint66 = 1;
        
        try
        {
            await Shell.Current.GoToAsync($"///MainPage/SettingsPage/ProfileDetailPage");
        }
        catch (Exception ex)
        {
            int breakpoint56 = 1;
        }
        
    }
    #endregion

    #region Constructor
    // Constructor
    public ShadowFileViewModel(IConnectivity connectivity)
    {
        this.connectivity = connectivity;
        int breakpoint56 = 1;

    }
    #endregion


    public void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            navStateIsVisible = false;
            UpperPartIsVisible = false;
        }
    }

    public void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        UpperPartIsVisible = true;
    }



    #region Region Task CopyCloudToShadow
    [RelayCommand]
    private async Task CopyCloudToShadow()
    {
        bool allValuesAreNull = true;
        foreach (var item in actYearValues)
        {
            if (item != 0)
            {
                allValuesAreNull = false;
                break;
            }
        }

        if (allValuesAreNull)
        {
            if (await Application.Current.MainPage.DisplayAlert("Alert", "All values in the local Cloud-Table Storage are 0.0 ! Click 'OK' to load values from the Cloud or 'Cancel' to abort?", "OK", "Cancel"))
            {
                // Continue
            }
            else
            {
                return;
            }
        }

        string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + CloudStorageAccount + ";AccountKey=" + await SecureStorage.GetAsync(CloudStorageAccount);

        DateTimeOffset? dateOfLastMinusOffsetEntity = await TableHelper.GetTimeStampOfLastMinusOffsetEntity(TableName, ColumnName, connectionString, rowsToLoad: 10, offset: 1);

        TableYearString = dateOfLastMinusOffsetEntity != null
            ? dateOfLastMinusOffsetEntity.Value.Year.ToString(DateTimeFormatInfo.InvariantInfo)
            : "0001";

        string xmlShadowFileName = FormattableString.Invariant($"{CloudStorageAccount}.{TableName}.{ColumnName}.xml");

        Dictionary<string, string> yearShadowValuesXmlDictionary = new()
            {
                { "OriginalFileName", xmlShadowFileName },
                { "TargetProgram", "ChartSluuk" },
                { "Version", "1.0.0" },
                { "OriginalCreationDateUtc", DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo) },
                { "Acount", CloudStorageAccount },
                { "Table", TableName },
                { "TableYear", TableYearString },
                { "Column", ColumnName },
                { "Factor", shadowFileSettingItems.Factor },
                { "Type", shadowFileSettingItems.ColumnType },
                { "JsonContent",  string.Empty}
            };

        if (dateOfLastMinusOffsetEntity == null)
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "File could not be read from the Cloud. Internet Connection? Aborting", "OK");
            return;
        }

        // Load Cloud values

        Dictionary<string, object> respDict = await TableHelper.ActualizeBarChartYearSource(TableHelper.ReturnSelector.YearAndFloatArray, actYearValues, TableName, ColumnName, shadowFileSettingItems.Factor, shadowFileSettingItems.ColumnType, connectionString);

        actYearValues = (TableHelper.ReturnState)respDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? respDict[TableHelper.ReturnKeys.ArrayContent] as float[] : actYearValues;
        TableYearString = (TableHelper.ReturnState)respDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? ((int)respDict[TableHelper.ReturnKeys.Year]).ToString("D4", NumberFormatInfo.InvariantInfo) : "0001";

        yearShadowValuesXmlDictionary["JsonContent"] = System.Text.Json.JsonSerializer.Serialize(actYearValues);

        DictionaryXML.WriteDictionaryStringStringToXmlFile(yearShadowValuesXmlDictionary, "YearDailyValues", appFolder, xmlShadowFileName);

        int year = int.TryParse(TableYearString, out year) ? year : DateTime.MinValue.Year;
        int dayCntOfYear = DateTime.IsLeapYear(year) ? actYearValues.Length : actYearValues.Length - 1;

        for (int i = 0; i < dayCntOfYear; i++)
        {
            YearTableValueCollection[i].CloudValue = actYearValues[i].ToString("#,##0.00", formatProviderInvariantNumber);
            YearTableValueCollection[i].ShadowValue = actYearValues[i].ToString("#,##0.00", formatProviderInvariantNumber);
        }
    }
    #endregion

    #region Region DeleteShadowValuesFile
    [RelayCommand]
    private async Task DeleteShadowValuesFile()
    {
        string xmlShadowFileName = FormattableString.Invariant($"{CloudStorageAccount}.{TableName}.{ColumnName}.xml");
        if (File.Exists(Path.Combine(rootPath, appFolder, xmlShadowFileName)))
        {
            if (await Application.Current.MainPage.DisplayAlert("Alert", FormattableString.Invariant($"Shadow File '{xmlShadowFileName} found! Delete File ?"), "OK", "Cancel"))
            {
                File.Delete(Path.Combine(rootPath, appFolder, xmlShadowFileName));

                foreach (DayValuesSet item in YearTableValueCollection)
                {
                    item.ShadowValue = null;
                }
            }
            else
            {
                return;
            }
        }
    }
    #endregion

    #region Region Task LoadCloudValues
    [RelayCommand]
    private async Task LoadCloudValues()
    {
        if (!await Shell.Current.DisplayAlert("Alert", "Load actual table values from the Cloud. \r\n Shadow values will not be overwrittten", "OK", "Cancel"))
        {
            return;
        }

        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("No connectivity!",
                $"Please check internet and try again.", "OK");
            return;
        }

        string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + CloudStorageAccount + ";AccountKey=" + await SecureStorage.GetAsync(CloudStorageAccount);

        DateTimeOffset? dateOfLastMinusOffsetEntity = await TableHelper.GetTimeStampOfLastMinusOffsetEntity(TableName, ColumnName, connectionString, rowsToLoad: 10, offset: 1);

        TableYearString = dateOfLastMinusOffsetEntity != null
            ? dateOfLastMinusOffsetEntity.Value.Year.ToString(DateTimeFormatInfo.InvariantInfo)
            : "0001";

        string xmlShadowFileName = FormattableString.Invariant($"{CloudStorageAccount}.{TableName}.{ColumnName}.xml");

        Dictionary<string, string> yearShadowValuesXmlDictionary = new()
            {
                { "OriginalFileName", xmlShadowFileName },
                { "TargetProgram", "ChartSluuk" },
                { "Version", "1.0.0" },
                { "OriginalCreationDateUtc", DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo) },
                { "Acount", CloudStorageAccount },
                { "Table", TableName },
                { "TableYear", TableYearString },
                { "Column", ColumnName },
                { "Factor", shadowFileSettingItems.Factor },
                { "Type", shadowFileSettingItems.ColumnType },
                { "JsonContent",  string.Empty}
            };

        // Load Cloud values
        if (dateOfLastMinusOffsetEntity == null)
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "Cloud values could not be read.", "OK");
        }
        else
        {
            Dictionary<string, object> respDict = await TableHelper.ActualizeBarChartYearSource(TableHelper.ReturnSelector.YearAndFloatArray, actYearValues, TableName, ColumnName, shadowFileSettingItems.Factor, shadowFileSettingItems.ColumnType, connectionString);

            actYearValues = (TableHelper.ReturnState)respDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? respDict[TableHelper.ReturnKeys.ArrayContent] as float[] : actYearValues;
            TableYearString = (TableHelper.ReturnState)respDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? ((int)respDict[TableHelper.ReturnKeys.Year]).ToString("D4", NumberFormatInfo.InvariantInfo) : "0001";
        }

        int year = int.TryParse(TableYearString, out year) ? year : DateTime.MinValue.Year;
        int dayCntOfYear = DateTime.IsLeapYear(year) ? actYearValues.Length : actYearValues.Length - 1;

        for (int i = 0; i < dayCntOfYear; i++)
        {
            YearTableValueCollection[i].CloudValue = "0.00";
        }

        await Task.Delay(500);

        for (int i = 0; i < dayCntOfYear; i++)
        {
            YearTableValueCollection[i].CloudValue = actYearValues[i].ToString("#,##0.00", formatProviderInvariantNumber);
        }
    }
    #endregion

    #region Region Task InitializeValuesTable
    private async Task InitializeValuesTable()
    {
        //if (pTableAccount != null && pTableAccount != string.Empty)

        if (!(CloudStorageAccount != null && CloudStorageAccount != string.Empty && (await SecureStorage.GetAsync(CloudStorageAccount)) != null))
        {
            await Shell.Current.DisplayAlert("No valid account entered for this table!",
                    $"Please enter account in the settings.", "OK");
            return;
        }

        string theKey = await SecureStorage.GetAsync(CloudStorageAccount);

        string connectionString = "DefaultEndpointsProtocol=https;AccountName=" + CloudStorageAccount + ";AccountKey=" + await SecureStorage.GetAsync(CloudStorageAccount);

        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("No connectivity!",
                $"Please check internet and try again.", "OK");
            return;
        }

        DateTimeOffset? dateOfLastMinusOffsetEntity = await TableHelper.GetTimeStampOfLastMinusOffsetEntity(TableName, ColumnName, connectionString, rowsToLoad: 10, offset: 1);

        TableYearString = dateOfLastMinusOffsetEntity != null
            ? dateOfLastMinusOffsetEntity.Value.Year.ToString(DateTimeFormatInfo.InvariantInfo)
            : "0001";

        string xmlShadowFileName = FormattableString.Invariant($"{CloudStorageAccount}.{TableName}.{ColumnName}.xml");

        Dictionary<string, string> yearShadowValuesXmlDictionary = new()
            {
                { "OriginalFileName", xmlShadowFileName },
                { "TargetProgram", "ChartSluuk" },
                { "Version", "1.0.0" },
                { "OriginalCreationDateUtc", DateTime.UtcNow.ToString(DateTimeFormatInfo.InvariantInfo) },
                { "Acount", CloudStorageAccount },
                { "Table", TableName },
                { "TableYear", TableYearString },
                { "Column", ColumnName },
                { "Factor", shadowFileSettingItems.Factor },
                { "Type", shadowFileSettingItems.ColumnType },
                { "JsonContent",  string.Empty}
            };

        // If Shadow file doesn't exist we try to read values from the Cloud
        if (!File.Exists(Path.Combine(rootPath, appFolder, xmlShadowFileName)))
        {
            // Load Cloud values
            if (dateOfLastMinusOffsetEntity == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Cloud values could not be read.", "OK");
            }
            else
            {
                Dictionary<string, object> respDict = await TableHelper.ActualizeBarChartYearSource(TableHelper.ReturnSelector.YearAndFloatArray, actYearValues, TableName, ColumnName, shadowFileSettingItems.Factor, shadowFileSettingItems.ColumnType, connectionString);

                actYearValues = (TableHelper.ReturnState)respDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? respDict[TableHelper.ReturnKeys.ArrayContent] as float[] : actYearValues;
                TableYearString = (TableHelper.ReturnState)respDict[TableHelper.ReturnKeys.ReturnState] == TableHelper.ReturnState.Valid ? ((int)respDict[TableHelper.ReturnKeys.Year]).ToString("D4", NumberFormatInfo.InvariantInfo) : "0001";
                int breakpoint_89 = 1;
            }
        }

        // If Shadowvalues file doesn't exist?
        if (!File.Exists(Path.Combine(rootPath, appFolder, xmlShadowFileName)) && (dateOfLastMinusOffsetEntity != null))
        {
            if (await Application.Current.MainPage.DisplayAlert("Alert", "Shadow Table doesn't exist. \r\n Store Cloud-Values as local Shadow Table?", "OK", "Cancel"))
            {
                yearShadowValuesXmlDictionary["JsonContent"] = System.Text.Json.JsonSerializer.Serialize(actYearValues);

                DictionaryXML.WriteDictionaryStringStringToXmlFile(yearShadowValuesXmlDictionary, "YearDailyValues", appFolder, xmlShadowFileName);
            }
        }

        // Read Shadow file  back
        yearShadowValuesXmlDictionary = DictionaryXML.GetDictionaryStringStringFromXmlFile(appFolder, xmlShadowFileName);

        // If successful
        if (yearShadowValuesXmlDictionary != null)
        {
            ShadowOriginalCreationDateUtc = yearShadowValuesXmlDictionary["OriginalCreationDateUtc"];
            actYearShadowValues = JsonSerializer.Deserialize<float[]>(yearShadowValuesXmlDictionary["JsonContent"]);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Alert", "File with Shadow-values could not be read.", "OK");
        }

        // Create a List of DayValueSet and fill with values
        List<DayValuesSet> yearTableValueList = new();
        string shadowValueTemp;

        int year = int.TryParse(TableYearString, out year) ? year : DateTime.MinValue.Year;
        var tableYearFirstDay = new DateTime(year, 1, 1);

        int dayCntOfYear = DateTime.IsLeapYear(year) ? actYearValues.Length : actYearValues.Length - 1;
        for (int i = 0; i < dayCntOfYear; i++)
        {
            shadowValueTemp = actYearShadowValues[i].ToString("#,##0.00", formatProviderInvariantNumber);

            yearTableValueList.Add(new DayValuesSet()
            {
                // RoSchmi
                Date = tableYearFirstDay.AddDays(i - 1).ToShortDateString(),
                CloudValue = actYearValues[i].ToString("#,##0.00", formatProviderInvariantNumber),
                ShadowValue = shadowValueTemp,
                ShadowValueCorrected = shadowValueTemp,
            });
        }


        // Load changes-xml-File (format: account.table.column.changes.xml) containing changed values
        string changesXmlFileName = FormattableString.Invariant($"{CloudStorageAccount}.{TableName}.{ColumnName}.Changes.xml");

        Dictionary<string, string> changedYearValuesXmlDictionary = new()
            {
                { "OriginalFileName", changesXmlFileName },
                { "TargetProgram", "ChartSluuk" },
                { "Version", "1.0.0" },
                { "ShadowOriginalCreationDateUtc", string.Empty },
                { "Acount", CloudStorageAccount },
                { "Table", TableName },
                { "TableYear", TableYearString },
                { "Column", ColumnName },
                { "Factor", shadowFileSettingItems.Factor },
                { "Type", shadowFileSettingItems.ColumnType },
                { "JsonContent",  string.Empty}
            };
        if (File.Exists(Path.Combine(rootPath, appFolder, changesXmlFileName)))
        {
            changedYearValuesXmlDictionary = DictionaryXML.GetDictionaryStringStringFromXmlFile(appFolder, changesXmlFileName);
            if (changedYearValuesXmlDictionary.Count > 0)
            {
                Dictionary<int, string> changedValuesDictionary = JsonSerializer.Deserialize<Dictionary<int, string>>(changedYearValuesXmlDictionary["JsonContent"]);

                if (changedValuesDictionary.Count > 0)
                {
                    foreach (KeyValuePair<int, string> changedValue in changedValuesDictionary)
                    {
                        yearTableValueList[changedValue.Key].ShadowValueCorrected = changedValue.Value;
                        yearTableValueList[changedValue.Key].CorrectValue = changedValue.Value;
                        yearTableValueList[changedValue.Key].IsCorrected = true;
                    }
                }
            }
        }

        // Fill Observable Collection from List
        YearTableValueCollection = new ObservableCollection<DayValuesSet>();
        ActivityIndicatorIsRunning = true;

        int lengthOfList = yearTableValueList.Count;
        int batchSize = 50;
        await FillObservableCollectionAsync(YearTableValueCollection, yearTableValueList, lengthOfList, batchSize);

        ActivityIndicatorIsRunning = false;

        int breakpoint57 = 1;
    }
    #endregion

    #region Region Task FillObservableCollectionAsync
    private static async Task FillObservableCollectionAsync(ObservableCollection<DayValuesSet> collection, List<DayValuesSet> list, int lengthOfList, int batchSize)
    {
        int loopIndex = 0;
        bool isReady = false;

        while (!isReady)
        {
            isReady = await FillObservableCollectionBatch(collection, list, lengthOfList, loopIndex, batchSize);
            loopIndex += batchSize;
        }
    }
    #endregion

    #region Region Task FillObservableCollectionBatch
    private static async Task<bool> FillObservableCollectionBatch(ObservableCollection<DayValuesSet> collection, List<DayValuesSet> list, int lengthOfList, int currentIndex, int batchSize)
    {
        int cntToCopy = (lengthOfList - currentIndex) >= batchSize ? batchSize : (lengthOfList - currentIndex);
        int copied = 0;
        for (int i = currentIndex; i < currentIndex + cntToCopy; i++)
        {
            collection.Add(list[i]);
            copied++;
            //await Task.Delay(2);
            await Task.Delay(10);
        }

        if (currentIndex + copied >= lengthOfList)
        {
            return true;
            //return Task.FromResult(true);
        }
        else
        {
            return false;
            //return Task.FromResult(false);
        }

    }
    #endregion

    #region Region RelayCommand GoBack
    [RelayCommand]
    public async Task GoBack()
    {
        Dictionary<string, object> navigationParameter = new Dictionary<string, object>()
            {
                {nameof(ShadowFilePage), TransmissionObject},
                {"Sender", nameof(ShadowFilePage)}
            };

        // await Shell.Current.Navigation.PopModalAsync(new SettingsDetailPage(((SettingItems)s).Sender, navigationParameter));


        //await Shell.Current.GoToAsync($"{new string("///SettingsPage/ProfileDetailPage")}?Sender={nameof(ShadowFilePage)}", navigationParameter);

        //await Shell.Current.GoToAsync($"{new string("///SettingsPage")}?Sender={nameof(ShadowFilePage)}", navigationParameter);

        var theShadowFilePageName = nameof(ShadowFilePage);

        var theSettingsPageName = nameof(SettingsPage);

        Uri myUri;

        UriBuilder myUriBuilder = new UriBuilder() { Host = "///:////", Path = new string($"{nameof(SettingsPage)}/{nameof(ProfileDetailPage)}") };

        //  await Shell.Current.GoToAsync($"{new string("..")}?Sender={nameof(ShadowFilePage)}", navigationParameter);

        /*
        try
        {
            myUri = new Uri("http://www.contoso.com/");

            myUri = new Uri("//://IMPL_SettingsPage/IMPL_SettingsPage/SettingsPage/ProfileDetailPage");

            myUri = new Uri($"///{nameof(SettingsPage)}/{nameof(ProfileDetailPage)}/");

            myUri = new Uri($"///{nameof(SettingsPage)}/{nameof(ProfileDetailPage)}");

            myUri = new Uri($"////{nameof(SettingsPage)}/{nameof(ProfileDetailPage)}");

            myUri = new Uri("///:////{nameof(SettingsPage)}/{nameof(ProfileDetailPage)}");
        }
        catch (Exception ex)
        {
            int breakpoint67 = 1;
        }
        */

        try
        {


            // await Shell.Current.GoToAsync($"{new string($"/{nameof(SettingsPage)}/{nameof(ProfileDetailPage)}?'uri')")}");

            // await Shell.Current.GoToAsync($"{new string($"///:////SettingsPage/ProfileDetail/")}?Sender={nameof(ShadowFilePage)}", navigationParameter);
            //   await Shell.Current.GoToAsync($"{new string($"///:////SettingsPage/ProfileDetailPage")}?Sender={nameof(ShadowFilePage)}", navigationParameter);
            //   await Shell.Current.GoToAsync($"{new string($"///:////SettingsPage/ProfileDetailPage?'uri'/SettingsPage/ProfileDetailPage?")}", navigationParameter);
            //   await Shell.Current.GoToAsync($"{new string($"///:////SettingsPage/ProfileDetailPage/SettingsPage/ProfileDetailPage")}", navigationParameter);


            // string theParam = new string("Hallo");
            // await Shell.Current.GoToAsync($"..?parameterToPassBack={theParam}");

            await Shell.Current.GoToAsync($"..");


            // _ = await Shell.Current.Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            int breakpoint_78 = 1;
        }


        // await Shell.Current.GoToAsync($"{new string("..")}?Parameter ={nameof(ShadowFilePage)}", navigationParameter);

    }
    #endregion

    #region  Region OnNavigatedTo
    public void OnNavigatedTo(NavigatedToEventArgs e)
    {
        NavigationState = Shell.Current.CurrentState.Location.ToString();

    }
    #endregion

    #region Region OnNavigatedFrom
    /*
    public void OnNavigatingFrom(NavigatingFromEventArgs e)
    {
        string state = Shell.Current.CurrentState.Location.ToString();

        int breakpoint_1 = 1;
    }
    */
    #endregion
}
