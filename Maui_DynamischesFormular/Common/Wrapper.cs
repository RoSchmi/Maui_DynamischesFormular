
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
                       
                        workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier,  StringValue = ((StringTypeContent)property.Value.Content).Value });
                        
                        break;
                    }    

                case WorkItem.TypeID.RsBoolean:
                case WorkItem.TypeID.RsBooleanRo:
                case WorkItem.TypeID.RsBooleanNo:

                    {     
                        workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier, BoolValue = ((BoolTypeContent)property.Value.Content).Value });
                          break;
                    }

                case WorkItem.TypeID.RsDateTime:
                case WorkItem.TypeID.RsDateTimeRo:
                case WorkItem.TypeID.RsDateTimeNo:
                    {
                        workItemsList.Add(new WorkItem() { TabNo = property.Value.TabNo, Name = property.Value.Name, DisplayName = property.Value.DisplayName, TypeIdentifier = property.Value.TypeIdentifier, DateValue = ((DateTimeTypeContent)property.Value.Content).Value });

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

        return workItemsList.Count > 0 ? workItemsList.ToObservableCollection<WorkItem>() : new ObservableCollection<WorkItem>();
    }
    #endregion

    #region Region ProfileToSuitCaseProperties
    // Is now in Folder LayoutEntryCollections
    
    #endregion

    #region Region TransportItemToWorkItem
    public static WorkItem TransportItemToWorkItem(TransportItem transportItem)
    {
        WorkItem returnWorkItem = null;

        switch (transportItem.TypeIdentifier)
        {
            case WorkItem.TypeID.RsString:
            case WorkItem.TypeID.RsStringRo:  
            case WorkItem.TypeID.RsStringNo:
            case WorkItem.TypeID.RsStringSw:
            case WorkItem.TypeID.RsStringPi:
                {
                    returnWorkItem = new WorkItem() { Name = transportItem.Name, DisplayName = transportItem.DisplayName, TabNo = transportItem.TabNo, TypeIdentifier = transportItem.TypeIdentifier, StringValue = ((StringTypeContent)transportItem.Content).Value };
                }
                break;

            case WorkItem.TypeID.RsBoolean:
            case WorkItem.TypeID.RsBooleanRo:
            case WorkItem.TypeID.RsBooleanNo:

                {


                    returnWorkItem = new WorkItem() { Name = transportItem.Name, DisplayName = transportItem.DisplayName, TabNo = transportItem.TabNo, TypeIdentifier = transportItem.TypeIdentifier, BoolValue = ((BoolTypeContent)transportItem.Content).Value };
                    break;
                }

            case WorkItem.TypeID.RsDateTime:
            case WorkItem.TypeID.RsDateTimeRo:
            case WorkItem.TypeID.RsDateTimeNo:
                {
                    returnWorkItem = new WorkItem() { Name = transportItem.Name, DisplayName = transportItem.DisplayName, TabNo = transportItem.TabNo, TypeIdentifier = transportItem.TypeIdentifier, DateValue = ((DateTimeTypeContent)transportItem.Content).Value };
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

