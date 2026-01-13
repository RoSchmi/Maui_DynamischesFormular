
using System.Collections.ObjectModel;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.ViewModels;


using CommunityToolkit.Maui.Core.Extensions;

namespace Maui_DynamischesFormular.Common;

/// <summary>
/// Methods:
///  WorkItemsToTransportItems,
///  TransportItemsToWorkItems,
///  ProfileSetToSuitCaseProperties,
///  TransportItemToWorkItem
///  (WorkItemToTransportItem is in folder Helpers ItemTypeConverter)
/// </summary>

public static class Wrapper
{
    /// <summary>
    /// Converts an ObservableCollection of Type WorkItem into a Dictionary&lt;string, TransportItem>
    /// The keys in the Dictionary are WorkItem.Name
    /// </summary>
    /// <param name="pWorkItems"></param>
    /// <returns>Dictionary&lt;string, TransportItem></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>

    #region Region WorkItemsToTransportItems
    public static Dictionary<string, TransportItem> WorkItemsToTransportItems(ObservableCollection<WorkItem> pWorkItems)
    {
        var PropertiesDictionary = new Dictionary<string, TransportItem>();

        // Add the TransportItems from the WorkItems
        foreach (WorkItem workItem in pWorkItems)
        {

            // RoSchmi ToDo: evtl put cases for regular, Ro and No together
            switch (workItem.TypeIdentifier)
            {
                case WorkItem.TypeID.RsString:
                case WorkItem.TypeID.RsStringNo:
                case WorkItem.TypeID.RsStringRo:
                case WorkItem.TypeID.RsStringSw:
                case WorkItem.TypeID.RsStringPi:
                    {
                        PropertiesDictionary.Add(workItem.Name, new TransportItem() { Name = workItem.Name, TypeIdentifier = workItem.TypeIdentifier, Content = new StringTypeContent() { Value = workItem.StringValue } });
                        break;
                    }

                case WorkItem.TypeID.RsBoolean:
                case WorkItem.TypeID.RsBooleanNo:
                case WorkItem.TypeID.RsBooleanRo:
                    {
                        PropertiesDictionary.Add(workItem.Name, new TransportItem() { Name = workItem.Name, TypeIdentifier = workItem.TypeIdentifier, Content = new BoolTypeContent() { Value = workItem.BoolValue } });
                        break;
                    }


                case WorkItem.TypeID.RsDateTime:
                case WorkItem.TypeID.RsDateTimeRo:
                case WorkItem.TypeID.RsDateTimeNo:
                    {
                        PropertiesDictionary.Add(workItem.Name, new TransportItem() { Name = workItem.Name, TypeIdentifier = workItem.TypeIdentifier, Content = new DateTimeTypeContent() { Value = workItem.DateValue } });
                        break;
                    }
                /*
            case WorkItem.TypeID.RsDateTimeRo:
                {
                    PropertiesDictionary.Add(workItem.Name, new TransportItem() { Name = workItem.Name, TypeIdentifier = workItem.TypeIdentifier, Content = new DateTimeTypeContent() { Value = workItem.DateValue } });
                    break;
                }
            case WorkItem.TypeID.RsDateTimeNo:
                {
                    PropertiesDictionary.Add(workItem.Name, new TransportItem() { Name = workItem.Name, TypeIdentifier = workItem.TypeIdentifier, Content = new DateTimeTypeContent() { Value = workItem.DateValue } });
                    break;
                }
                */

                default:
                    {
                        PropertiesDictionary.Add(workItem.Name, new TransportItem() { Name = workItem.Name, TypeIdentifier = workItem.TypeIdentifier, Content = new StringTypeContent() { Value = workItem.StringValue } });
                        throw new ArgumentOutOfRangeException("WorkItemsToTransportItems: " + workItem.TypeIdentifier);
                        break;
                    }
            }
        }

        return PropertiesDictionary;
    }
    #endregion

    #region Region TransportItemsToWorkItems
    public static ObservableCollection<WorkItem> TransportItemsToWorkItems(Dictionary<string, TransportItem> pTransportItems)
    {
        var workItemsList = new List<WorkItem> { };

        foreach (KeyValuePair<string, TransportItem> property in pTransportItems)
        {
            switch (property.Value.TypeIdentifier)
            {
                case WorkItem.TypeID.RsString:
                case WorkItem.TypeID.RsStringRo:  // RoSchmi:Made changes here
                case WorkItem.TypeID.RsStringNo:
                case WorkItem.TypeID.RsStringSw:
                case WorkItem.TypeID.RsStringPi:
                    {
                        workItemsList.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, StringValue = ((StringTypeContent)property.Value.Content).Value });
                        //SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, StringValue = ((StringTypeContent)property.Value.Content).Value });
                        break;
                    }

                case WorkItem.TypeID.RsBoolean:
                case WorkItem.TypeID.RsBooleanRo:
                case WorkItem.TypeID.RsBooleanNo:

                    {
                        workItemsList.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });

                        // SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });
                        break;
                    }

                case WorkItem.TypeID.RsDateTime:
                case WorkItem.TypeID.RsDateTimeRo:
                case WorkItem.TypeID.RsDateTimeNo:
                    {
                        workItemsList.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, DateValue = ((DateTimeTypeContent)property.Value.Content).Value });

                        // SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, DateValue = ((DateTimeTypeContent)property.Value.Content).Value });
                        break;
                    }
                default:
                    {
                        int breakpoint = 1;
                        break;
                        //throw new ArgumentOutOfRangeException("TransportItemsToWorkItems:" + property.Value.TypeIdentifier);
                    }
            }
        }

        //  workItemsList.RemoveRange(6, workItemsList.Count - 6);

        //workItemsList.RemoveRange(0, 2);



        return workItemsList.Count > 0 ? workItemsList.ToObservableCollection<WorkItem>() : new ObservableCollection<WorkItem>();
    }
    #endregion

    #region Region ProfileSetToSuitCaseProperties
    public static SuitCaseProperties ProfileSetToSuitCaseProperties(ProfileSet profSet)
    {
        var propertiesDictionary = new Dictionary<string, TransportItem>()
                {
           /* 0 */  {nameof(profSet.Account),            new TransportItem() { Name = nameof(ProfileSet.Account),            TypeIdentifier = WorkItem.TypeID.RsStringRo,   Content = new StringTypeContent()   { Value = profSet.Account } } },
           /* 1 */  {nameof(profSet.Profile),            new TransportItem() { Name = nameof(ProfileSet.Profile),            TypeIdentifier = WorkItem.TypeID.RsStringRo,   Content = new StringTypeContent()   { Value = profSet.Profile } } },

           /* 2 */  {nameof(profSet.DataSourceTable1),   new TransportItem() { Name = nameof(ProfileSet.DataSourceTable1),   TypeIdentifier = WorkItem.TypeID.RsStringSw,   Content = new StringTypeContent()   { Value = profSet.DataSourceTable1 } } },
           /* 3 */  {nameof(profSet.DataSourceTable2),   new TransportItem() { Name = nameof(ProfileSet.DataSourceTable2),   TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable2 } } },
           /* 4 */  {nameof(profSet.DataSourceTable3),   new TransportItem() { Name = nameof(ProfileSet.DataSourceTable3),   TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable3 } } },
           /* 5 */  {nameof(profSet.DataSourceTable4),   new TransportItem() { Name = nameof(ProfileSet.DataSourceTable4),   TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable4 } } },

           /* 6 */  {nameof(profSet.Table1Property),     new TransportItem() { Name = nameof(ProfileSet.Table1Property),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Property } } },
           /* 7 */  {nameof(profSet.Table2Property),     new TransportItem() { Name = nameof(ProfileSet.Table2Property),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Property } } },
           /* 8 */  {nameof(profSet.Table3Property),     new TransportItem() { Name = nameof(ProfileSet.Table3Property),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Property } } },
           /* 9 */  {nameof(profSet.Table4Property),     new TransportItem() { Name = nameof(ProfileSet.Table4Property),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Property } } },

           /* 10 */ {nameof(profSet.Table1AutomaticYear),new TransportItem() { Name = nameof(ProfileSet.Table1AutomaticYear),TypeIdentifier = WorkItem.TypeID.RsBooleanNo,  Content = new BoolTypeContent()     { Value = profSet.Table1AutomaticYear } } },
           /* 11 */ {nameof(profSet.Table1Provider),     new TransportItem() { Name = nameof(ProfileSet.Table1Provider),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Provider } } },
           /* 12 */ {nameof(profSet.Table1Type),         new TransportItem() { Name = nameof(ProfileSet.Table1Type),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Type } } },
           /* 13 */ {nameof(profSet.Table1Displacement), new TransportItem() { Name = nameof(ProfileSet.Table1Displacement), TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Displacement } } },
           /* 14 */ {nameof(profSet.Table1Unit),         new TransportItem() { Name = nameof(ProfileSet.Table1Unit),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Unit } } },
           /* 15 */ {nameof(profSet.Table1Factor),       new TransportItem() { Name = nameof(ProfileSet.Table1Factor),       TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Factor } } },
           /* 16 */ {nameof(profSet.Table1StartDate),    new TransportItem() { Name = nameof(ProfileSet.Table1StartDate),    TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table1StartDate } } },
           /* 17 */ {nameof(profSet.Table1EndDate),      new TransportItem() { Name = nameof(ProfileSet.Table1EndDate),      TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table1EndDate } } },
           /* 18 */ {nameof(profSet.Table1Account),      new TransportItem() { Name = nameof(ProfileSet.Table1Account),      TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1Account } } },
           /* 19 */ {nameof(profSet.Table1SortField),    new TransportItem() { Name = nameof(ProfileSet.Table1SortField),    TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table1SortField } } },
           /* 20 */ {nameof(profSet.T1Ph3),              new TransportItem() { Name = nameof(ProfileSet.T1Ph3),              TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.T1Ph3 } } },

           /* 21 */ {nameof(profSet.Table2AutomaticYear),new TransportItem() { Name = nameof(ProfileSet.Table2AutomaticYear),TypeIdentifier = WorkItem.TypeID.RsBooleanNo,  Content = new BoolTypeContent()     { Value = profSet.Table2AutomaticYear } } },
                    {nameof(profSet.Table2Provider),     new TransportItem() { Name = nameof(ProfileSet.Table2Provider),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Provider } } },
                    {nameof(profSet.Table2Type),         new TransportItem() { Name = nameof(ProfileSet.Table2Type),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Type } } },
                    {nameof(profSet.Table2Displacement), new TransportItem() { Name = nameof(ProfileSet.Table2Displacement), TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Displacement } } },
                    {nameof(profSet.Table2Unit),         new TransportItem() { Name = nameof(ProfileSet.Table2Unit),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Unit } } },
                    {nameof(profSet.Table2Factor),       new TransportItem() { Name = nameof(ProfileSet.Table2Factor),       TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Factor } } },
                    {nameof(profSet.Table2StartDate),    new TransportItem() { Name = nameof(ProfileSet.Table2StartDate),    TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table2StartDate } } },
                    {nameof(profSet.Table2EndDate),      new TransportItem() { Name = nameof(ProfileSet.Table2EndDate),      TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table2EndDate } } },
                    {nameof(profSet.Table2Account),      new TransportItem() { Name = nameof(ProfileSet.Table2Account),      TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2Account } } },
                    {nameof(profSet.Table2SortField),    new TransportItem() { Name = nameof(ProfileSet.Table2SortField),    TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table2SortField } } },
                    {nameof(profSet.T2Ph3),              new TransportItem() { Name = nameof(ProfileSet.T2Ph3),              TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.T2Ph3 } } },


           /* 32 */ {nameof(profSet.Table3AutomaticYear),new TransportItem() { Name = nameof(ProfileSet.Table3AutomaticYear),TypeIdentifier = WorkItem.TypeID.RsBooleanNo,  Content = new BoolTypeContent()     { Value = profSet.Table3AutomaticYear } } },
                    {nameof(profSet.Table3Provider),     new TransportItem() { Name = nameof(ProfileSet.Table3Provider),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Provider } } },
                    {nameof(profSet.Table3Type),         new TransportItem() { Name = nameof(ProfileSet.Table3Type),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Type } } },
                    {nameof(profSet.Table3Displacement), new TransportItem() { Name = nameof(ProfileSet.Table3Displacement), TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Displacement } } },
                    {nameof(profSet.Table3Unit),         new TransportItem() { Name = nameof(ProfileSet.Table3Unit),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Unit } } },
                    {nameof(profSet.Table3Factor),       new TransportItem() { Name = nameof(ProfileSet.Table3Factor),       TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Factor } } },
                    {nameof(profSet.Table3StartDate),    new TransportItem() { Name = nameof(ProfileSet.Table3StartDate),    TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table3StartDate } } },
                    {nameof(profSet.Table3EndDate),      new TransportItem() { Name = nameof(ProfileSet.Table3EndDate),      TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table3EndDate } } },
                    {nameof(profSet.Table3Account),      new TransportItem() { Name = nameof(ProfileSet.Table3Account),      TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3Account } } },
                    {nameof(profSet.Table3SortField),    new TransportItem() { Name = nameof(ProfileSet.Table3SortField),    TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table3SortField } } },
                    {nameof(profSet.T3Ph3),              new TransportItem() { Name = nameof(ProfileSet.T3Ph3),              TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.T3Ph3 } } },


           /* 43 */ {nameof(profSet.Table4AutomaticYear),new TransportItem() { Name = nameof(ProfileSet.Table4AutomaticYear),TypeIdentifier = WorkItem.TypeID.RsBooleanNo,  Content = new BoolTypeContent()     { Value = profSet.Table4AutomaticYear } } },
                    {nameof(profSet.Table4Provider),     new TransportItem() { Name = nameof(ProfileSet.Table4Provider),     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Provider } } },
                    {nameof(profSet.Table4Type),         new TransportItem() { Name = nameof(ProfileSet.Table4Type),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Type } } },
                    {nameof(profSet.Table4Displacement), new TransportItem() { Name = nameof(ProfileSet.Table4Displacement), TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Displacement } } },
                    {nameof(profSet.Table4Unit),         new TransportItem() { Name = nameof(ProfileSet.Table4Unit),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Unit } } },
                    {nameof(profSet.Table4Factor),       new TransportItem() { Name = nameof(ProfileSet.Table4Factor),       TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Factor } } },
                    {nameof(profSet.Table4StartDate),    new TransportItem() { Name = nameof(ProfileSet.Table4StartDate),    TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table4StartDate } } },
                    {nameof(profSet.Table4EndDate),      new TransportItem() { Name = nameof(ProfileSet.Table4EndDate),      TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.Table4EndDate } } },
                    {nameof(profSet.Table4Account),      new TransportItem() { Name = nameof(ProfileSet.Table4Account),      TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4Account } } },
                    {nameof(profSet.Table4SortField),    new TransportItem() { Name = nameof(ProfileSet.Table4SortField),    TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Table4SortField } } },
                    {nameof(profSet.T4Ph3),              new TransportItem() { Name = nameof(ProfileSet.T4Ph3),              TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.T4Ph3 } } },

           /* 54 */ {nameof(profSet.SettingsState),    new TransportItem() { Name = nameof(ProfileSet.SettingsState),    TypeIdentifier = WorkItem.TypeID.RsBooleanNo,    Content = new BoolTypeContent()     { Value = profSet.SettingsState } } },
           /* 55 */ {nameof(profSet.SettingsDate),     new TransportItem() { Name = nameof(ProfileSet.SettingsDate),     TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent() { Value = profSet.SettingsDate } } },
           /* 56 */ {nameof(profSet.SettingsID),       new TransportItem() { Name = nameof(ProfileSet.SettingsID),       TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.SettingsID } } },
           /* 57 */ {nameof(profSet.Index),            new TransportItem() { Name = nameof(ProfileSet.Index),            TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Index } } },
           /* 58 */ {nameof(profSet.Selected),         new TransportItem() { Name = nameof(ProfileSet.Selected),         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Selected } } },
        };

        return new SuitCaseProperties() { PropertiesDictionary = propertiesDictionary };
    }
    #endregion

    #region Region TransportItemToWorkItem
    public static WorkItem TransportItemToWorkItem(TransportItem transportItem)
    {
        WorkItem returnWorkItem = null;

        switch (transportItem.TypeIdentifier)
        {
            case WorkItem.TypeID.RsString:
            case WorkItem.TypeID.RsStringRo:  // RoSchmi:Made changes here
            case WorkItem.TypeID.RsStringNo:
            case WorkItem.TypeID.RsStringSw:
            case WorkItem.TypeID.RsStringPi:
                {
                    returnWorkItem = new WorkItem() { Name = transportItem.Name, TypeIdentifier = transportItem.TypeIdentifier, StringValue = ((StringTypeContent)transportItem.Content).Value };
                    // return new WorkItem() { Name = transportItem.Name, TypeIdentifier = transportItem.TypeIdentifier, StringValue = ((StringTypeContent)transportItem.Content).Value };
                }
                break;

            case WorkItem.TypeID.RsBoolean:
            case WorkItem.TypeID.RsBooleanRo:
            case WorkItem.TypeID.RsBooleanNo:

                {


                    returnWorkItem = new WorkItem() { Name = transportItem.Name, TypeIdentifier = transportItem.TypeIdentifier, BoolValue = ((BoolTypeContent)transportItem.Content).Value };
                    //SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });
                    break;
                }

            case WorkItem.TypeID.RsDateTime:
            case WorkItem.TypeID.RsDateTimeRo:
            case WorkItem.TypeID.RsDateTimeNo:
                {
                    returnWorkItem = new WorkItem() { Name = transportItem.Name, TypeIdentifier = transportItem.TypeIdentifier, DateValue = ((DateTimeTypeContent)transportItem.Content).Value };
                    //SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, DateValue = ((DateTimeTypeContent)property.Value.Content).Value });
                    break;
                }
            default:
                {
                    throw new ArgumentOutOfRangeException("TransportItemsToWorkItems:" + transportItem.TypeIdentifier);
                }
        }

        return returnWorkItem;
    }
    #endregion

    #region Note: 'WorkItemToTransportItem' is in folder Helpers ItemTypeConverter
    #endregion
}

