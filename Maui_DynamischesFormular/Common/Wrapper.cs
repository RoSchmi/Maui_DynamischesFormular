
using CommunityToolkit.Maui.Core.Extensions;
using Maui_DynamischesFormular.LayoutEntryCollections;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
    public static Dictionary<string, TransportItem> WorkItemsToTransportItems(ObservableCollection<WorkItem> pWorkItems, bool clipDigit = false)
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
                case WorkItem.TypeID.RsStringRo:  //actItem.Name : string.IsNullOrEmpty(actItem.Name) ? string.Empty : actItem.Name[..^1];
                case WorkItem.TypeID.RsStringSw:
                case WorkItem.TypeID.RsStringPi:
                    {
                        String? baseName = clipDigit && (workItem.TabNo > 0) ? string.IsNullOrEmpty(workItem.Name) ? string.Empty : workItem.Name[..^1] : workItem.Name;
                        if (baseName != string.Empty)
                        {
                            //PropertiesDictionary.Add(baseName, new TransportItem() { Name = workItem.Name, DisplayName = workItem.DisplayName, TabNo = workItem.TabNo, TypeIdentifier = workItem.TypeIdentifier, Content = new StringTypeContent() { Value = workItem.StringValue } });

                            // In case of Picker: SelectedPickerItem is transferred to the 'StringValue' because it could be altered by Picker action.
                            // For simple Strings the eventually altered StringValue is taken
                            //var alteredString = workItem.TypeIdentifier == WorkItem.TypeID.RsStringPi ? workItem.SelectedPickerItem : workItem.StringValue;
                            var alteredString = workItem.StringValue;
                            PropertiesDictionary.Add(baseName, new TransportItem() { Name = workItem.Name, DisplayName = workItem.DisplayName, TabNo = workItem.TabNo, TypeIdentifier = workItem.TypeIdentifier, Content = new StringTypeContent() { Value = alteredString } });

                        }
                        break;
                    }

                case WorkItem.TypeID.RsBoolean:
                case WorkItem.TypeID.RsBooleanNo:
                case WorkItem.TypeID.RsBooleanRo:
                    {
                        String? baseName = clipDigit && (workItem.TabNo > 0) ? string.IsNullOrEmpty(workItem.Name) ? string.Empty : workItem.Name[..^1] : workItem.Name;
                        if (baseName != string.Empty)
                        {
                            PropertiesDictionary.Add(baseName, new TransportItem() { Name = workItem.Name, DisplayName = workItem.DisplayName, TabNo = workItem.TabNo, TypeIdentifier = workItem.TypeIdentifier, Content = new BoolTypeContent() { Value = workItem.BoolValue } });
                        }
                            break;
                    }


                case WorkItem.TypeID.RsDateTime:
                case WorkItem.TypeID.RsDateTimeRo:
                case WorkItem.TypeID.RsDateTimeNo:
                    {
                        String? baseName = clipDigit && (workItem.TabNo > 0) ? string.IsNullOrEmpty(workItem.Name) ? string.Empty : workItem.Name[..^1] : workItem.Name;
                        if (baseName != string.Empty)
                        {
                            PropertiesDictionary.Add(baseName, new TransportItem() { Name = workItem.Name, DisplayName = workItem.DisplayName, TabNo = workItem.TabNo, TypeIdentifier = workItem.TypeIdentifier, Content = new DateTimeTypeContent() { Value = workItem.DateValue } });
                        }
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
                        // arriving StringValue must be transered to StringValue and SelectedPickerItem (needed for binding to Picker)
                        var arrivingStringValue = ((StringTypeContent)property.Value.Content).Value;
                        workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier,  StringValue = arrivingStringValue });
                        
                        break;
                    }    // SelectedPickerItem = arrivingStringValue,

                case WorkItem.TypeID.RsBoolean:
                case WorkItem.TypeID.RsBooleanRo:
                case WorkItem.TypeID.RsBooleanNo:

                    {
                        //workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = (bool?)property.Value.Content }); //{   Value = ((BoolTypeContent)property.Value.Content).Value } };   // ((BoolTypeContent)property.Value.Content).Value });

                       workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });



                        // SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });
                        break;
                    }

                case WorkItem.TypeID.RsDateTime:
                case WorkItem.TypeID.RsDateTimeRo:
                case WorkItem.TypeID.RsDateTimeNo:
                    {
                        workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier, DateValue = ((DateTimeTypeContent)property.Value.Content).Value });

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

    #region Region ProfileToSuitCaseProperties
    // Is now in Folder LayoutEntryCollections
    /*
    public static SuitCaseProperties ProfileToSuitCaseProperties(CollectionProfile profSet, List<string> profileList)
    {
        var propertiesDictionary = new Dictionary<string, TransportItem>()
        { 
        
                    {nameof(profSet.Account),            new TransportItem() { TabNo = 0, Name = nameof(profSet.Account),          DisplayName =  "Logged Account",                     TypeIdentifier = WorkItem.TypeID.RsStringRo,     Content = new StringTypeContent()   { Value = profSet.Account } } },
                    {nameof(profSet.DataGroup),          new TransportItem() { TabNo = 0, Name = nameof(profSet.DataGroup),        DisplayName = nameof(profSet.DataGroup),             TypeIdentifier = WorkItem.TypeID.RsStringRo,     Content = new StringTypeContent()   { Value = profSet.DataGroup } } },
                    {nameof(profSet.SettingsState),      new TransportItem() { TabNo = 0, Name = nameof(profSet.SettingsState),    DisplayName = nameof(profSet.SettingsState),         TypeIdentifier = WorkItem.TypeID.RsBooleanNo,    Content = new BoolTypeContent()     { Value = profSet.SettingsState } } },
                    {nameof(profSet.SettingsDate),       new TransportItem() { TabNo = 0, Name = nameof(profSet.SettingsDate),     DisplayName = nameof(profSet.SettingsDate),          TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent() { Value = profSet.SettingsDate } } },
                    {nameof(profSet.SettingsID),         new TransportItem() { TabNo = 0, Name = nameof(profSet.SettingsID),       DisplayName = nameof(profSet.SettingsID),            TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.SettingsID } } },
                    {nameof(profSet.Index),              new TransportItem() { TabNo = 0, Name = nameof(profSet.Index),            DisplayName = nameof(profSet.Index),                 TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Index } } },
                    {nameof(profSet.Selected),           new TransportItem() { TabNo = 0, Name = nameof(profSet.Selected),         DisplayName = nameof(profSet.Selected),              TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.Selected } } },


                    {nameof(profSet.DataSourceTable1),   new TransportItem() { TabNo = 1, Name = nameof(profSet.DataSourceTable1),   DisplayName = "Storage Table",                      TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable1 } } },
                    {nameof(profSet.DataSourceTable2),   new TransportItem() { TabNo = 2, Name = nameof(profSet.DataSourceTable2),   DisplayName = "Storage Table",                      TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable3 } } },
                    {nameof(profSet.DataSourceTable3),   new TransportItem() { TabNo = 3, Name = nameof(profSet.DataSourceTable3),   DisplayName = "Storage Table",                      TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable4 } } },
                    {nameof(profSet.DataSourceTable4),   new TransportItem() { TabNo = 4, Name = nameof(profSet.DataSourceTable4),   DisplayName = "Storage Table",                      TypeIdentifier = WorkItem.TypeID.RsStringSw,     Content = new StringTypeContent()   { Value = profSet.DataSourceTable4 } } },


                    {nameof(profSet.TableProperty1),     new TransportItem() { TabNo = 1, Name = nameof(profSet.TableProperty1),     DisplayName = "Tabellenspalte",                     TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProperty1 } } },
                    {nameof(profSet.TableProperty2),     new TransportItem() { TabNo = 2, Name = nameof(profSet.TableProperty2),     DisplayName = "Tabellenspalte",                     TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProperty2 } } },
                    {nameof(profSet.TableProperty3),     new TransportItem() { TabNo = 3, Name = nameof(profSet.TableProperty3),     DisplayName = "Tabellenspalte",                     TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProperty3 } } },
                    {nameof(profSet.TableProperty4),     new TransportItem() { TabNo = 4, Name = nameof(profSet.TableProperty4),     DisplayName = "Tabellenspalte",                     TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProperty4 } } },

                    {nameof(profSet.TableAutomaticYear1),new TransportItem() { TabNo = 1, Name = nameof(profSet.TableAutomaticYear1),DisplayName = "Jahr-Suffix?",                       TypeIdentifier = WorkItem.TypeID.RsBooleanNo,    Content = new BoolTypeContent()     { Value = profSet.TableAutomaticYear1 } } },
                    {nameof(profSet.TableAutomaticYear2),new TransportItem() { TabNo = 2, Name = nameof(profSet.TableAutomaticYear2),DisplayName = "Jahr-Suffix?",                       TypeIdentifier = WorkItem.TypeID.RsBooleanNo,    Content = new BoolTypeContent()     { Value = profSet.TableAutomaticYear2 } } },
                    {nameof(profSet.TableAutomaticYear3),new TransportItem() { TabNo = 3, Name = nameof(profSet.TableAutomaticYear3),DisplayName = "Jahr-Suffix?",                       TypeIdentifier = WorkItem.TypeID.RsBooleanNo,    Content = new BoolTypeContent()     { Value = profSet.TableAutomaticYear3 } } },
                    {nameof(profSet.TableAutomaticYear4),new TransportItem() { TabNo = 4, Name = nameof(profSet.TableAutomaticYear4),DisplayName = "Jahr-Suffix?",                       TypeIdentifier = WorkItem.TypeID.RsBooleanNo,    Content = new BoolTypeContent()     { Value = profSet.TableAutomaticYear4 } } },

                    {nameof(profSet.TableProvider1),     new TransportItem() { TabNo = 1, Name = nameof(profSet.TableProvider1),     DisplayName = "Data-Provider",                      TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProvider1 } } },
                    {nameof(profSet.TableProvider2),     new TransportItem() { TabNo = 2, Name = nameof(profSet.TableProvider2),     DisplayName = "Data-Provider",                      TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProvider2 } } },
                    {nameof(profSet.TableProvider3),     new TransportItem() { TabNo = 3, Name = nameof(profSet.TableProvider3),     DisplayName = "Data-Provider",                      TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProvider3 } } },
                    {nameof(profSet.TableProvider4),     new TransportItem() { TabNo = 4, Name = nameof(profSet.TableProvider4),     DisplayName = "Data-Provider",                      TypeIdentifier = WorkItem.TypeID.RsStringNo,     Content = new StringTypeContent()   { Value = profSet.TableProvider4 } } },

                    {nameof(profSet.TableType1),         new TransportItem() { TabNo = 1, Name = nameof(profSet.TableType1),         DisplayName = "Typ der Werte",                      TypeIdentifier = WorkItem.TypeID.RsStringPi,     Content = new StringTypeContent()   { Value = profSet.TableType1 } } },
                    {nameof(profSet.TableType2),         new TransportItem() { TabNo = 2, Name = nameof(profSet.TableType2),         DisplayName = "Typ der Werte",                      TypeIdentifier = WorkItem.TypeID.RsStringPi,     Content = new StringTypeContent()   { Value = profSet.TableType2 } } },
                    {nameof(profSet.TableType3),         new TransportItem() { TabNo = 3, Name = nameof(profSet.TableType3),         DisplayName = "Typ der Werte",                      TypeIdentifier = WorkItem.TypeID.RsStringPi,     Content = new StringTypeContent()   { Value = profSet.TableType3 } } },
                    {nameof(profSet.TableType4),         new TransportItem() { TabNo = 4, Name = nameof(profSet.TableType4),         DisplayName = "Typ der Werte",                      TypeIdentifier = WorkItem.TypeID.RsStringPi,     Content = new StringTypeContent()   { Value = profSet.TableType4 } } },


                    {nameof(profSet.TableDisplacement1), new TransportItem() { TabNo = 1, Name = nameof(profSet.TableDisplacement1), DisplayName = "Ersatzwert",                         TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableDisplacement1 } } },
                    {nameof(profSet.TableDisplacement2), new TransportItem() { TabNo = 2, Name = nameof(profSet.TableDisplacement2), DisplayName = "Ersatzwert",                TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableDisplacement2 } } },
                    {nameof(profSet.TableDisplacement3), new TransportItem() { TabNo = 3, Name = nameof(profSet.TableDisplacement3), DisplayName = "Ersatzwert",                TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableDisplacement3 } } },
                    {nameof(profSet.TableDisplacement4), new TransportItem() { TabNo = 4, Name = nameof(profSet.TableDisplacement4), DisplayName = "Ersatzwert",                TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableDisplacement4 } } },


                    {nameof(profSet.TableUnit1),         new TransportItem() { TabNo = 1, Name = nameof(profSet.TableUnit1),         DisplayName = "Unit",          TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableUnit1 } } },
                    {nameof(profSet.TableUnit2),         new TransportItem() { TabNo = 2, Name = nameof(profSet.TableUnit2),         DisplayName = "Unit",          TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableUnit2 } } },
                    {nameof(profSet.TableUnit3),         new TransportItem() { TabNo = 3, Name = nameof(profSet.TableUnit3),         DisplayName = "Unit",          TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableUnit3 } } },
                    {nameof(profSet.TableUnit4),         new TransportItem() { TabNo = 4, Name = nameof(profSet.TableUnit4),         DisplayName = "Unit",          TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableUnit4 } } },


                    {nameof(profSet.TableFactor1),       new TransportItem() { TabNo = 1, Name = nameof(profSet.TableFactor1),       DisplayName = "Faktor",                             TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableFactor1 } } },
                    {nameof(profSet.TableFactor2),       new TransportItem() { TabNo = 2, Name = nameof(profSet.TableFactor2),       DisplayName = "Faktor", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableFactor2 } } },
                    {nameof(profSet.TableFactor3),       new TransportItem() { TabNo = 3, Name = nameof(profSet.TableFactor3),       DisplayName = "Faktor", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableFactor3 } } },
                    {nameof(profSet.TableFactor4),       new TransportItem() { TabNo = 4, Name = nameof(profSet.TableFactor4),       DisplayName = "Faktor", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableFactor4 } } },

                    {nameof(profSet.TableOffset1),       new TransportItem() { TabNo = 1, Name = nameof(profSet.TableOffset1),       DisplayName = "Offset", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableOffset1 } } },
                    {nameof(profSet.TableOffset2),       new TransportItem() { TabNo = 2, Name = nameof(profSet.TableOffset2),       DisplayName = "Offset", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableOffset2 } } },
                    {nameof(profSet.TableOffset3),       new TransportItem() { TabNo = 3, Name = nameof(profSet.TableOffset3),       DisplayName = "Offset", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableOffset3 } } },
                    {nameof(profSet.TableOffset4),       new TransportItem() { TabNo = 4, Name = nameof(profSet.TableOffset4),       DisplayName = "Offset", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableOffset4 } } },

                    {nameof(profSet.TableStartDate1),    new TransportItem() { TabNo = 1, Name = nameof(profSet.TableStartDate1),    DisplayName = "Start Date",      TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableStartDate1 } } },
                    {nameof(profSet.TableStartDate2),    new TransportItem() { TabNo = 2, Name = nameof(profSet.TableStartDate2),    DisplayName = "Start Date",                                   TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableStartDate2 } } },
                    {nameof(profSet.TableStartDate3),    new TransportItem() { TabNo = 3, Name = nameof(profSet.TableStartDate3),    DisplayName = "Start Date",                                   TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableStartDate3 } } },
                    {nameof(profSet.TableStartDate4),    new TransportItem() { TabNo = 4, Name = nameof(profSet.TableStartDate4),    DisplayName = "Start Date",                                   TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableStartDate4 } } },


                    {nameof(profSet.TableEndDate1),      new TransportItem() { TabNo = 1, Name = nameof(profSet.TableEndDate1),      DisplayName = "End Date",       TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableEndDate1 } } },
                    {nameof(profSet.TableEndDate2),      new TransportItem() { TabNo = 2, Name = nameof(profSet.TableEndDate2),      DisplayName = "End Date",                                 TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableEndDate2 } } },
                    {nameof(profSet.TableEndDate3),      new TransportItem() { TabNo = 3, Name = nameof(profSet.TableEndDate3),      DisplayName = "End Date",                                 TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableEndDate3 } } },
                    {nameof(profSet.TableEndDate4),      new TransportItem() { TabNo = 4, Name = nameof(profSet.TableEndDate4),      DisplayName = "End Date",                                 TypeIdentifier = WorkItem.TypeID.RsDateTimeNo, Content = new DateTimeTypeContent()   { Value = profSet.TableEndDate4 } } },

                    {nameof(profSet.TableAccount1),      new TransportItem() { TabNo = 1, Name = nameof(profSet.TableAccount1),      DisplayName = "Storage Account", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableAccount1 } } },
                    {nameof(profSet.TableAccount2),      new TransportItem() { TabNo = 2, Name = nameof(profSet.TableAccount2),      DisplayName = "Storage Account", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableAccount2 } } },
                    {nameof(profSet.TableAccount3),      new TransportItem() { TabNo = 3, Name = nameof(profSet.TableAccount3),      DisplayName = "Storage Account", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableAccount3 } } },
                    {nameof(profSet.TableAccount4),      new TransportItem() { TabNo = 4, Name = nameof(profSet.TableAccount4),      DisplayName = "Storage Account", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableAccount4 } } },

                    {nameof(profSet.TableCloudTable1),   new TransportItem() { TabNo = 1, Name = nameof(profSet.TableCloudTable1),   DisplayName = "Cloud Table Name",                   TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableCloudTable1 } } },
                    {nameof(profSet.TableCloudTable2),   new TransportItem() { TabNo = 2, Name = nameof(profSet.TableCloudTable2),   DisplayName = "Cloud Table Name",                   TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableCloudTable1 } } },
                    {nameof(profSet.TableCloudTable3),   new TransportItem() { TabNo = 3, Name = nameof(profSet.TableCloudTable3),   DisplayName = "Cloud Table Name",                   TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableCloudTable3 } } },
                    {nameof(profSet.TableCloudTable4),   new TransportItem() { TabNo = 3, Name = nameof(profSet.TableCloudTable4),   DisplayName = "Cloud Table Name",                   TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableCloudTable3 } } },

                    {nameof(profSet.TableSortField1),    new TransportItem() { TabNo = 1, Name = nameof(profSet.TableSortField1),    DisplayName = "Sortierspalte",                      TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableSortField1 } } },
                    {nameof(profSet.TableSortField2),    new TransportItem() { TabNo = 2, Name = nameof(profSet.TableSortField2),    DisplayName = "Sortierspalte", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableSortField2 } } },
                    {nameof(profSet.TableSortField3),    new TransportItem() { TabNo = 3, Name = nameof(profSet.TableSortField3),    DisplayName = "Sortierspalte", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableSortField3 } } },
                    {nameof(profSet.TableSortField4),    new TransportItem() { TabNo = 4, Name = nameof(profSet.TableSortField4),    DisplayName = "Sortierspalte", TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TableSortField4 } } },

                    {nameof(profSet.TablePh31),          new TransportItem() { TabNo = 1, Name = nameof(profSet.TablePh31),          DisplayName = "Vacant Position",                    TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TablePh31 } } },
                    {nameof(profSet.TablePh32),          new TransportItem() { TabNo = 2, Name = nameof(profSet.TablePh32),          DisplayName = "Vacant Position",    TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TablePh32 } } },
                    {nameof(profSet.TablePh33),          new TransportItem() { TabNo = 3, Name = nameof(profSet.TablePh33),          DisplayName = "Vacant Position",       TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TablePh33 } } },
                    {nameof(profSet.TablePh34),          new TransportItem() { TabNo = 4, Name = nameof(profSet.TablePh34),          DisplayName = "Vacant Position",     TypeIdentifier = WorkItem.TypeID.RsStringNo,   Content = new StringTypeContent()   { Value = profSet.TablePh34 } } },

            };
    

        //Substituted by profSet.DataGroup{ nameof(profSet.Profile),            new TransportItem() { TabNo = 0, Name = nameof(profSet.Profile), DisplayName = nameof(profSet.Profile), TypeIdentifier = WorkItem.TypeID.RsStringRo, Content = new StringTypeContent() { Value = profSet.Profile } } },


        var filteredDictinonary = !profileList.Any() ? propertiesDictionary : propertiesDictionary.Where(kvp => profileList.Contains(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return new SuitCaseProperties() { PropertiesDictionary = filteredDictinonary };
    }
    */
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
                    returnWorkItem = new WorkItem() { Name = transportItem.Name, DisplayName = transportItem.DisplayName, TabNo = transportItem.TabNo, TypeIdentifier = transportItem.TypeIdentifier, StringValue = ((StringTypeContent)transportItem.Content).Value };
                    // return new WorkItem() { Name = transportItem.Name, TypeIdentifier = transportItem.TypeIdentifier, StringValue = ((StringTypeContent)transportItem.Content).Value };
                }
                break;

            case WorkItem.TypeID.RsBoolean:
            case WorkItem.TypeID.RsBooleanRo:
            case WorkItem.TypeID.RsBooleanNo:

                {


                    returnWorkItem = new WorkItem() { Name = transportItem.Name, DisplayName = transportItem.DisplayName, TabNo = transportItem.TabNo, TypeIdentifier = transportItem.TypeIdentifier, BoolValue = ((BoolTypeContent)transportItem.Content).Value };
                    //SettingPropertyCollection.Add(new WorkItem() { Name = property.Value.Name, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });
                    break;
                }

            case WorkItem.TypeID.RsDateTime:
            case WorkItem.TypeID.RsDateTimeRo:
            case WorkItem.TypeID.RsDateTimeNo:
                {
                    returnWorkItem = new WorkItem() { Name = transportItem.Name, DisplayName = transportItem.DisplayName, TabNo = transportItem.TabNo, TypeIdentifier = transportItem.TypeIdentifier, DateValue = ((DateTimeTypeContent)transportItem.Content).Value };
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

