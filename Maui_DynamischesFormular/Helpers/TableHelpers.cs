using System;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using Azure.Data.Tables;
using Maui_DynamischesFormular;
using Common.Models.EdmTypes;
using Common.Models.ValTypes;


namespace Maui_DynamischesFormular.Helpers;

public static class TableHelper
{

    private static readonly IFormatProvider formatProviderInvariantDateTime = CultureInfo.InvariantCulture.DateTimeFormat;
    private static readonly IFormatProvider formatProviderInvariantNumber = CultureInfo.InvariantCulture.NumberFormat;

    public enum ReturnSelector
    {
        YearAndFloatArray,
        DateTimeAndFloatArray
    };

    public enum ReturnState
    {
        NotValid,
        Valid
    };
    public enum ReturnType
    {
        YearAndFloatArray,
        DateTimeAndFloatArray
    };

    public static class ReturnKeys
    {
        public static string ReturnState { get; } = "ReturnState";
        public static string ReturnType { get; } = "ReturnType";
        public static string Reason { get; } = "Reason";
        public static string Year { get; } = "Year";
        public static string FirstDayOfYearDate { get; } = "FirstDayOfYearDate";
        public static string ArrayContent { get; } = "ArrayContent";
    }

    public static async Task<DateTimeOffset?> GetTimeStampOfLastMinusOffsetEntity(string pTableName, string pColumnName, string pConnectionString, int rowsToLoad = 10, int offset = 0)
    {
        TableClient tableClient = null;
        try
        {
            if (pTableName != null && pTableName != string.Empty)
            {
                tableClient = new TableClient(pConnectionString, pTableName);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("TableClient Exception", "Error: " + ex.Message, "Ok");
            return null;
        }

        string filterString = null;
        var selection = new List<string>() { "Timestamp", pColumnName };
        var cancellationTokenSource = new CancellationTokenSource(4 * 1000);
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        string continuationToken = null;

        List<TableEntity> TableQueryResult = new();
        Azure.AsyncPageable<TableEntity> entityList;
        try
        {
            entityList = tableClient.QueryAsync<TableEntity>(filter: filterString, maxPerPage: rowsToLoad, selection, cancellationToken);
            await foreach (Azure.Page<TableEntity> page in entityList.AsPages(continuationToken))
            {
                foreach (TableEntity entity in page.Values)
                {
                    TableQueryResult.Add(entity);
                }
                cancellationTokenSource.Cancel();
                continuationToken = page.ContinuationToken;
                continuationToken = null;
            }
        }
        catch (Exception ex)
        {
            int breakpoint_68 = 1;
        }

        int breakpoint_67 = 1;

        return TableQueryResult[offset].Timestamp;
    }


    public static async Task<Dictionary<string, object>> GetYearValuesFromShadowFile(float[] periodValuesArray, string rootPath, string appFolder, string pTableAccount, string pTableName, string pColumnName, bool applyChanges = true)
    {
        string xmlShadowFileName = FormattableString.Invariant($"{pTableAccount}.{pTableName}.{pColumnName}.xml");
        string xmlChangesFileName = FormattableString.Invariant($"{pTableAccount}.{pTableName}.{pColumnName}.Changes.xml");

        Dictionary<string, string> yearShadowValuesXmlDictionary = new()
                {
                    { "OriginalFileName", string.Empty },
                    { "TargetProgram", string.Empty },
                    { "Version", string.Empty },
                    { "OriginalCreationDateUtc", string.Empty },
                    { "Acount", string.Empty },
                    { "Table", string.Empty },
                    { "TableYear", string.Empty },
                    { "Column", string.Empty },
                    { "Factor", string.Empty },
                    { "Type", string.Empty },
                    { "JsonContent",  string.Empty}
                };

        if (File.Exists(Path.Combine(rootPath, appFolder, xmlShadowFileName)))
        {
            // Read Shadow file  back
            yearShadowValuesXmlDictionary = DictionaryXML.GetDictionaryStringStringFromXmlFile(appFolder, xmlShadowFileName);
            // If successful
            if (yearShadowValuesXmlDictionary == null)
            {
                return new Dictionary<string, object> {
                    { ReturnKeys.ReturnState, ReturnState.NotValid },
                    { ReturnKeys.Reason, new string("Shadow values could not be read") },
                };
            }
            else
            {
                periodValuesArray = JsonSerializer.Deserialize<float[]>(yearShadowValuesXmlDictionary["JsonContent"]);
                int breakpoint = 1;
            }
        }
        else
        {
            return new Dictionary<string, object> {
                    { ReturnKeys.ReturnState, ReturnState.NotValid },
                    { ReturnKeys.Reason, new string("Shadow file not found") },
                };
        }

        if (applyChanges)
        {

            if (File.Exists(Path.Combine(rootPath, appFolder, xmlChangesFileName)))
            {
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

                changedYearValuesXmlDictionary = DictionaryXML.GetDictionaryStringStringFromXmlFile(appFolder, xmlChangesFileName);

                if (changedYearValuesXmlDictionary.Count > 0)
                {
                    Dictionary<int, string> changedValuesDictionary = JsonSerializer.Deserialize<Dictionary<int, string>>(changedYearValuesXmlDictionary["JsonContent"]);

                    if (changedValuesDictionary.Count > 0)
                    {
                        foreach (KeyValuePair<int, string> changedValue in changedValuesDictionary)
                        {
                            if (float.TryParse(changedValue.Value, NumberStyles.AllowDecimalPoint, formatProviderInvariantNumber, out float parseResult))
                            {
                                periodValuesArray[changedValue.Key] = parseResult;
                            }

                            int breakpoint = 1;
                        }
                    }
                }
            }
        }

        string ShadowOriginalCreationDateUtc = yearShadowValuesXmlDictionary["OriginalCreationDateUtc"];

        return new Dictionary<string, object> {
                    { ReturnKeys.ReturnState, ReturnState.Valid },
                    { ReturnKeys.ReturnType, ReturnType.YearAndFloatArray },
                    { ReturnKeys.Year, yearShadowValuesXmlDictionary["TableYear"] },
                    { ReturnKeys.ArrayContent, periodValuesArray},
                };

    }


    #region Region static Task ActualizeBarChartYearSource(..)
    //public static async Task<float[]> ActualizeBarChartYearSource(float[] periodValuesArray, string pTableName, string pColumnName, string pFactor, string pType, string pConnectionString)
    public static async Task<Dictionary<string, object>> ActualizeBarChartYearSource(ReturnSelector returnSelector, float[] periodValuesArray, string pTableName, string pColumnName, string pFactor, string pType, string pConnectionString)
    {
        TableClient tableClient = null;
        try
        {
            if (pTableName != null && pTableName != string.Empty)
            {
                tableClient = new TableClient(pConnectionString, pTableName);
            }
            else
            {
                // return periodValuesArray;
                return new Dictionary<string, object> {
                    { ReturnKeys.ReturnState, ReturnState.NotValid },
                    { ReturnKeys.Reason, new string("TableName was NullOrEmpty") },
                };
            }

            int breakpoint658 = 1;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("TableClient Exception", "Error: " + ex.Message, "Ok");
            // return periodValuesArray;
            return new Dictionary<string, object>() {
               {ReturnKeys.ReturnState, ReturnState.NotValid },
               {ReturnKeys.Reason, new string("Could not create TableClient") }
           };
        }

        // See how it works
        //https://briancaos.wordpress.com/2022/11/11/c-azure-table-storage-queryasync-paging-and-filtering/

        string filterString = null;
        var selection = new List<string>() { "Timestamp", pColumnName };

        // Load Entities from the Azure Cloud
        List<TableEntity> entityList = await PartitionRangeQueryAsync(tableClient, filterString, selection, pMaxRows: 50000, pTakeCount: 100, timeOutInSeconds: 40);

        EdmTypes.EdmType edmType = (entityList == null || entityList.Count <= 0) ? EdmTypes.EdmType.NotSupported : Maui_DynamischesFormular.Helpers.EdmTypeIdentifier.Detect(entityList.First(), pColumnName);

        float parseFloatResult = 1.0f;
        bool parseBoolResult = float.TryParse(pFactor, NumberStyles.AllowDecimalPoint, formatProviderInvariantNumber, out parseFloatResult);

        parseFloatResult = parseBoolResult ? parseFloatResult : 1.0f;

        List<int> dayPointerList = new();

        ValTypes.ValType valType = ValTypes.ValType.ValString;

        string typeGroup = pType;

        if (typeGroup != null)
        {
            typeGroup = typeGroup.StartsWith("TimeSpan", StringComparison.InvariantCulture) ? "TimeSpan" : typeGroup;
        }

        switch (typeGroup)
        {
            case "String":
            case "string":
            case null:
                {
                    valType = ValTypes.ValType.ValString;
                    break;
                }
            case "Float":
            case "float":
                {
                    valType = ValTypes.ValType.ValFloat;
                    break;
                }
            case "TimeSpan":
                {
                    valType = ValTypes.ValType.ValTimeSpan;
                    break;
                }
            default:
                {
                    valType = ValTypes.ValType.NotValid;
                    break;
                }
        }
        if (valType == ValTypes.ValType.NotValid)
        {
            throw new ArgumentException("Selected Type for DataSourceTable not valid");
        }

        switch (returnSelector)
        {
            case ReturnSelector.YearAndFloatArray:
                {
                    Tuple<DateTime, float[]> yearAndValues = FillYearsArrayFromQueryResult(entityList, pColumnName, periodValuesArray.Length, parseFloatResult, valType, defaultValue: 0.0f, decimalSeparator: '.');
                    return new Dictionary<string, object>
                    {
                        { ReturnKeys.ReturnState, ReturnState.Valid },              // ReturnState
                        { ReturnKeys.ReturnType, ReturnType.YearAndFloatArray },    // ReturnType
                        { ReturnKeys.Year, yearAndValues.Item1.Year },              // Year
                        { ReturnKeys.ArrayContent, yearAndValues.Item2 },           // 
                    };
                    break;
                }
            case ReturnSelector.DateTimeAndFloatArray:
                {
                    Tuple<DateTime, float[]> yearAndValues = FillYearsArrayFromQueryResult(entityList, pColumnName, periodValuesArray.Length, parseFloatResult, valType, defaultValue: 0.0f, decimalSeparator: '.');

                    return new Dictionary<string, object>
                    {
                        { ReturnKeys.ReturnState, ReturnState.Valid },
                        { ReturnKeys.ReturnType, ReturnType.DateTimeAndFloatArray },
                        { ReturnKeys.FirstDayOfYearDate, yearAndValues.Item1 },
                        { ReturnKeys.ArrayContent, yearAndValues.Item2 },

                    };
                    break;
                }
            default:
                {
                    return new Dictionary<string, object>
                    {
                        { ReturnKeys.ReturnState, ReturnState.NotValid},
                        { ReturnKeys.Reason, new string("Not allowed Type was requested") }

                    };
                    break;
                }
        }

        //periodValuesArray = FillYearsArrayFromQueryResult(entityList, pColumnName, periodValuesArray.Length, parseFloatResult, valType, defaultValue: 0.0f, decimalSeparator: '.');

        //return periodValuesArray;
    }
    #endregion

    #region PartitionRangeQueryAsync
    /// <summary>
    /// Demonstrate a partition range query that searches within a partition for a set of entities that are within a 
    /// specific range. The async APIs require that the user handle the segment size and return the next segment 
    /// using continuation tokens. 
    /// </summary>
    /// <param name="table">Sample table name</param>
    /// <param name="partitionKey">The partition within which to search</param>
    /// <param name="startRowKey">The lowest bound of the row key range within which to search</param>
    /// <param name="endRowKey">The highest bound of the row key range within which to search</param>
    /// <returns>A Task object</returns>
    private static async Task<List<TableEntity>> PartitionRangeQueryAsync(TableClient pTableClient, string pFilterString = null, List<string> pColumns = null, int pMaxRows = 500, int pTakeCount = 50, int timeOutInSeconds = 10)
    {
        // Request 50 results at a time from the server. 
        var cancellationTokenSource = new CancellationTokenSource(timeOutInSeconds * 1000);
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        string continuationToken = null;

        List<TableEntity> TableQueryResult = new();
        Azure.AsyncPageable<TableEntity> entityList;
        try
        {
            List<string> selection = new();
            selection = pColumns;

            entityList = pTableClient.QueryAsync<TableEntity>(filter: pFilterString, maxPerPage: pTakeCount, selection, cancellationToken);

            int breakpoint = 1;

            await foreach (Azure.Page<TableEntity> page in entityList.AsPages(continuationToken))
            {
                foreach (TableEntity entity in page.Values)
                {
                    TableQueryResult.Add(entity);
                }

                continuationToken = page.ContinuationToken;
                if (TableQueryResult.Count > pMaxRows)
                {
                    cancellationTokenSource.Cancel(); // 
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("TableClient Exception", ex.Message, "Ok");

            return null;
        }

        return TableQueryResult;
    }
    #endregion

    #region FillYearsArrayFromQueryResult
    //private static float[] FillYearsArrayFromQueryResult(List<TableEntity> entityList, string selectedProperty, int returnArrayLength, float factor, ValTypes.ValType valType, float defaultValue = 0.0f, char decimalSeparator = '.')
    private static Tuple<DateTime, float[]> FillYearsArrayFromQueryResult(List<TableEntity> entityList, string selectedProperty, int returnArrayLength, float factor, ValTypes.ValType valType, float defaultValue = 0.0f, char decimalSeparator = '.')

    {
        // initialize all elements of array with 0.0f or defaultValue
        float[] daysValuesArray = new float[returnArrayLength];
        if (defaultValue != 0.0f)
        {
            for (int i = 0; i < returnArrayLength; i++)
            {
                daysValuesArray[i] = defaultValue;
            }
        }

        if (entityList == null || entityList.Count < 1)
        {
            //return daysValuesArray;
            return new Tuple<DateTime, float[]>(new DateTime(1, 1, 1), daysValuesArray);
        }

        EdmTypes.EdmType edmType = Helpers.EdmTypeIdentifier.Detect(entityList.First(), selectedProperty);

        DateTimeOffset lastReadDateTime = new DateTime(DateTime.Now.Year, 12, 31);

        var ValuesOfCurrentDay = new List<Tuple<DateTimeOffset, string>>();

        var ValuesOfThisYear = new List<Tuple<int, List<Tuple<DateTimeOffset, string>>>>();

        DateTimeOffset localDateTimeOffset = new DateTime(DateTime.Now.Year, 1, 1);

        int startOffset = 2;
        // 
        foreach (TableEntity entity in entityList)
        {
            localDateTimeOffset = entity.GetDateTimeOffset("Timestamp").GetValueOrDefault().ToLocalTime();

            /*
            if (localDateTimeOffset.DayOfYear < 2)
            {
                int breakpoint_3 = 1;
            }
            */

            switch (edmType)
            {
                case EdmTypes.EdmType.EdmString:
                    {
                        if (localDateTimeOffset.DayOfYear != lastReadDateTime.DayOfYear + startOffset)        // New day
                        {
                            lastReadDateTime = localDateTimeOffset;

                            if (startOffset == 0)
                            {
                                ValuesOfThisYear.Add(new Tuple<int, List<Tuple<DateTimeOffset, string>>>(localDateTimeOffset.DayOfYear + 1, JsonSerializer.Deserialize<List<Tuple<DateTimeOffset, string>>>(JsonSerializer.Serialize(ValuesOfCurrentDay))));
                            }

                            startOffset = 0;
                            ValuesOfCurrentDay.Clear();
                            ValuesOfCurrentDay.Add(new Tuple<DateTimeOffset, string>(localDateTimeOffset, entity.GetString(selectedProperty)));

                            int breakpoint = 1;
                        }
                        else                // Same day
                        {
                            var theValue = entity.GetString(selectedProperty);
                            ValuesOfCurrentDay.Add(new Tuple<DateTimeOffset, string>(localDateTimeOffset, entity.GetString(selectedProperty)));
                            int breakpoint_2 = 1;
                        }

                        break;
                    }
            }

            int breakpoint35 = 1;
        }

        // Add the values of the first day of the year (is not handled in the foreach loop
        ValuesOfThisYear.Add(new Tuple<int, List<Tuple<DateTimeOffset, string>>>(localDateTimeOffset.DayOfYear, JsonSerializer.Deserialize<List<Tuple<DateTimeOffset, string>>>(JsonSerializer.Serialize(ValuesOfCurrentDay))));

        int breakpoint36 = 1;

        // Get the first day of the year
        DateTimeOffset StartDay = DateTimeOffset.MinValue;
        if (ValuesOfThisYear.Count > 0)
        {
            if (ValuesOfThisYear.Count > 1)
            {
                StartDay = ValuesOfThisYear[ValuesOfThisYear.Count - 2].Item2.First().Item1;
            }
            else
            {
                if (ValuesOfThisYear.Last().Item2.Count > 0)
                {
                    StartDay = ValuesOfThisYear.Last().Item2.First().Item1;
                }
            }
            //StartDay = ValuesOfThisYear.Count > 1 ? ValuesOfThisYear[ValuesOfThisYear.Count -2].Item2.First().Item1 : ValuesOfThisYear.Last().Item2.First().Item1;

            int breakpoint23 = 1;
        }



        foreach (Tuple<int, List<Tuple<DateTimeOffset, string>>> dayValues in ValuesOfThisYear)
        {
            // RoSchmi
            //if (dayValues.Item1 >= 2)
            if (dayValues.Item1 >= 1)
            {
                if (dayValues.Item1 == 2)
                {
                    int breakpoint38 = 1;
                }

                var dayFloatTuples = new List<Tuple<DateTimeOffset, float>>();
                foreach (Tuple<DateTimeOffset, string> dayValue in dayValues.Item2)
                {
                    bool parseResult = false;
                    float floatResult = defaultValue;
                    switch (valType)
                    {

                        case ValTypes.ValType.ValString:
                            {
                                parseResult = float.TryParse(dayValue.Item2, NumberStyles.AllowDecimalPoint, formatProviderInvariantNumber, out floatResult);
                                break;
                            }
                        case ValTypes.ValType.ValTimeSpan:
                            {
                                var duration = TimeSpan.FromSeconds(0);
                                parseResult = TimeSpan.TryParseExact(dayValue.Item2, @"ddd\-hh\:mm\:ss", formatProviderInvariantDateTime, out duration);
                                {
                                    floatResult = (float)duration.TotalSeconds;
                                }

                                break;
                            }
                    }

                    dayFloatTuples.Add(new Tuple<DateTimeOffset, float>(dayValue.Item1, parseResult ? floatResult * factor : defaultValue));
                }

                if (dayFloatTuples.Count > 1)
                {
                    float lastValueOfThisDay = dayFloatTuples[0].Item2;
                    daysValuesArray[dayFloatTuples[0].Item1.DayOfYear] = lastValueOfThisDay;

                    float firstValueOfThisDay = dayFloatTuples.Last().Item2;

                    daysValuesArray[dayFloatTuples.Last().Item1.DayOfYear - 1] = firstValueOfThisDay;

                    daysValuesArray[dayFloatTuples.Last().Item1.DayOfYear] = (lastValueOfThisDay > daysValuesArray[dayFloatTuples.Last().Item1.DayOfYear]) ? lastValueOfThisDay : daysValuesArray[dayFloatTuples.Last().Item1.DayOfYear];

                    int breakpoint_1 = 1;
                }
                else
                {
                    if (dayFloatTuples.Count > 0)
                    {
                        float OneAndOnlyValueOfThisDay = dayFloatTuples[0].Item2;

                        if (dayFloatTuples[0].Item1.Hour == 0)
                        {
                            if (dayFloatTuples[0].Item1.DayOfYear > 1)
                            {
                                daysValuesArray[dayFloatTuples[0].Item1.DayOfYear - 1] = dayFloatTuples[0].Item2;
                            }
                        }
                        else
                        {
                            daysValuesArray[dayFloatTuples[0].Item1.DayOfYear] = daysValuesArray[dayFloatTuples[0].Item1.DayOfYear] > OneAndOnlyValueOfThisDay ? daysValuesArray[dayFloatTuples[0].Item1.DayOfYear] : OneAndOnlyValueOfThisDay;
                        }
                        int breakpoint_2 = 1;
                    }
                }
            }
        }

        return new Tuple<DateTime, float[]>(new DateTime(StartDay.LocalDateTime.Year, 1, 1), daysValuesArray);


        // return daysValuesArray;
    }
    #endregion
}
