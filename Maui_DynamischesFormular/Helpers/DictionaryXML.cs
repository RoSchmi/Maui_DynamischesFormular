using System.Xml;
using Maui_DynamischesFormular.Models;
//using ChartSluuk.Models;
using System.Text.Json;
using Maui_DynamischesFormular.ViewModels;
using System.Xml.Linq;
using static CommunityToolkit.Mvvm.ComponentModel.__Internals.__TaskExtensions.TaskAwaitableWithoutEndValidation;

namespace Maui_DynamischesFormular.Helpers;

internal class StringTransportItem
{
    public StringTransportItem() { }

    public int TabNo { get; set; }
    public string Name { get; set; }

    public string DisplayName { get; set; }
    public WorkItem.TypeID TypeIdentifier { get; set; }
    public StringTypeContent Content { get; set; }
}

internal class BoolTransportItem
{
    public BoolTransportItem() { }
    public int TabNo { get; set; }

    public string Name { get; set; }
    public string DisplayName { get; set; }
    public WorkItem.TypeID TypeIdentifier { get; set; }
    public BoolTypeContent Content { get; set; }
}

public class DateTimeTransportItem
{
    public DateTimeTransportItem() { }

    public int TabNo { get; set; }
    public string Name { get; set; }

    public string DisplayName { get; set; }
    public WorkItem.TypeID TypeIdentifier { get; set; }
    public DateTimeTypeContent Content { get; set; }
}

public class ProfilePair
{
    public ProfilePair() { }
    public string AccountAndName { get; set; }
    public List<string> Properties { get; set; }
}

public class XMLContainer
{
    public XMLContainer()
    {
        ProfileDescriptions = new List<ProfilePair>();
    }
    public List<ProfilePair> ProfileDescriptions { get; set; }
}

public class TransportItems
{
    public TransportItems()
    {
        Properties = new List<string>();
    }
    public string PropertiesName { get; set; }
    public List<string> Properties { get; set; }

}
public static class DictionaryXML
{

    #region Region method GetDictionaryStringStringFromXmlFile
    public static Dictionary<string, string> GetDictionaryStringStringFromXmlFile(string pFolderName, string pFileName)
    {
        string rootPath = FileSystem.Current.AppDataDirectory;
        string folderPath = Path.Combine(rootPath, pFolderName);
        string filePath = Path.Combine(folderPath, pFileName);

        if (!File.Exists(filePath))
        {
            return null;
        }
        

        //string backRead = File.ReadAllText(filePath);

        var file = new System.IO.StreamReader(filePath);

        var root = XElement.Load(file);

        file.Close();

        Dictionary<string, string> dict = new();
        try
        {
            foreach (XElement el in root.Elements())
            {
                dict.Add(el.Name.LocalName, el.Value);
            }

            return dict;
        }
        catch
        {
            return null;
        }
    }
    #endregion

    #region Region method WriteDictionaryStringStringToXmlFile
    public static void WriteDictionaryStringStringToXmlFile(Dictionary<string, string> pDictionary, string pRootName, string pFolderName, string pFileName)
    {
        string rootPath = FileSystem.Current.AppDataDirectory;
        string folderPath = Path.Combine(rootPath, pFolderName);
        string filePath = Path.Combine(folderPath, pFileName);

        if (File.Exists(@filePath))
        {
            File.Delete(@filePath);
        }

        //https://learn.microsoft.com/en-us/dotnet/standard/linq/work-dictionaries-linq-xml

        XElement root = new XElement(pRootName, from keyValue in pDictionary
                                                select new XElement(keyValue.Key, keyValue.Value)
                                             );

        int breakPoint_78 = 1;


        if (!File.Exists(@filePath))
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            FileStream file = File.Create(@filePath);
            file.Close();
        }

        string theXmlString = root.ToString();

        File.WriteAllText(@filePath, root.ToString());

        string backRead = File.ReadAllText(@filePath);

        int breakPoint_79 = 1;

    }
    #endregion

    #region Region method WriteProfilesDictionaryToXmlFile
    public static void WriteProfilesDictionaryToXmlFile(Dictionary<string, SuitCaseProperties> pDictionary, string pFolderName, string pFileName)
    {
        string rootPath = FileSystem.Current.AppDataDirectory;
        string folderPath = Path.Combine(rootPath, pFolderName);
        string filePath = Path.Combine(folderPath, pFileName);

        if (File.Exists(@filePath))
        {
            File.Delete(@filePath);
        }

        var xmLContainer = new XMLContainer();

        foreach (string profilesKey in pDictionary.Keys)
        {
            if (profilesKey.Length > 12)
            {
                var transportItems = new TransportItems();
                foreach (string propertiesKey in pDictionary[profilesKey].PropertiesDictionary.Keys)
                {
                    transportItems.PropertiesName = pDictionary[profilesKey].PropertiesDictionary[propertiesKey].Name;
                    transportItems.Properties.Add(JsonSerializer.Serialize(pDictionary[profilesKey].PropertiesDictionary[propertiesKey]));
                }

                xmLContainer.ProfileDescriptions.Add(new ProfilePair() { AccountAndName = profilesKey, Properties = transportItems.Properties });
            }
        }

        System.Xml.Serialization.XmlSerializer writer = null;
        try
        {
            writer = new System.Xml.Serialization.XmlSerializer(typeof(XMLContainer));
        }
        catch (Exception ex)
        {
            throw new XmlException(filePath, ex);
        }

        if (!File.Exists(@filePath))
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            FileStream file = File.Create(@filePath);
            file.Close();
        }

        var wfile = new System.IO.StreamWriter(@filePath);
        writer.Serialize(wfile, xmLContainer);
        wfile.Close();

        // RoSchmi: only for tests, can be deleted
        // We  read the serialized Profiles

        var reader = new System.Xml.Serialization.XmlSerializer(typeof(XMLContainer));
        var file2 = new System.IO.StreamReader(filePath);

        XMLContainer backRead = (XMLContainer)reader.Deserialize(file2);

        file2.Close();



    }
    #endregion


    #region Region method GetProfilesDictionaryFromXmlFile
    public static Dictionary<string, SuitCaseProperties> GetProfilesDictionaryFromXmlFile(string pFolderName, string pFileName)
    {
        string rootPath = FileSystem.Current.AppDataDirectory;
        string folderPath = Path.Combine(rootPath, pFolderName);
        string filePath = Path.Combine(folderPath, pFileName);




        if (!File.Exists(filePath))
        {

            return null;
        }
        else
        {
           //File.Delete(filePath);
           //return null;
        }
        

        // We  read the serialized Profiles 
        var reader = new System.Xml.Serialization.XmlSerializer(typeof(XMLContainer));
        var file = new System.IO.StreamReader(filePath);

        XMLContainer backRead = (XMLContainer)reader.Deserialize(file);

        file.Close();

        var actDictionary = new Dictionary<string, SuitCaseProperties>();

        foreach (ProfilePair item in backRead.ProfileDescriptions)
        {
            var PropertiesDictionary = new Dictionary<string, TransportItem>();
            foreach (string jsonProperty in item.Properties)
            {
                TransportItem transportItem = JsonSerializer.Deserialize<TransportItem>(jsonProperty);

                switch (transportItem.TypeIdentifier)
                {
                    case WorkItem.TypeID.RsString:
                    case WorkItem.TypeID.RsStringRo:
                    case WorkItem.TypeID.RsStringNo:
                    case WorkItem.TypeID.RsStringSw:
                    case WorkItem.TypeID.RsStringPi:
                        {
                            StringTransportItem stringTransportItem = JsonSerializer.Deserialize<StringTransportItem>(jsonProperty);
                            transportItem = new TransportItem() { Name = stringTransportItem.Name, DisplayName = stringTransportItem.DisplayName, TabNo = stringTransportItem.TabNo, TypeIdentifier = stringTransportItem.TypeIdentifier, Content = stringTransportItem.Content };
                            break;
                        }
                    case WorkItem.TypeID.RsBoolean:
                    case WorkItem.TypeID.RsBooleanRo:
                    case WorkItem.TypeID.RsBooleanNo:
                        {
                            BoolTransportItem boolTransportItem = JsonSerializer.Deserialize<BoolTransportItem>(jsonProperty);
                            transportItem = new TransportItem() { Name = boolTransportItem.Name, DisplayName = boolTransportItem.DisplayName, TabNo = boolTransportItem.TabNo, TypeIdentifier = boolTransportItem.TypeIdentifier, Content = boolTransportItem.Content };
                            break;

                        }
                    case WorkItem.TypeID.RsDateTime:
                    case WorkItem.TypeID.RsDateTimeRo:
                    case WorkItem.TypeID.RsDateTimeNo:
                        {
                            DateTimeTransportItem dateTimeTransportItem = JsonSerializer.Deserialize<DateTimeTransportItem>(jsonProperty);
                            transportItem = new TransportItem() { Name = dateTimeTransportItem.Name, DisplayName = dateTimeTransportItem.DisplayName, TabNo = dateTimeTransportItem.TabNo, TypeIdentifier = dateTimeTransportItem.TypeIdentifier, Content = dateTimeTransportItem.Content };
                            break;

                        }

                    default:
                        {
                            StringTransportItem dateTimeTransportItem = JsonSerializer.Deserialize<StringTransportItem>(jsonProperty);
                            transportItem = new TransportItem() { Name = dateTimeTransportItem.Name, TypeIdentifier = dateTimeTransportItem.TypeIdentifier, Content = dateTimeTransportItem.Content };
                            throw new ArgumentOutOfRangeException("Not expected TypeID: " + transportItem.TypeIdentifier);
                            break;
                        }

                }

                PropertiesDictionary.Add(transportItem.Name, transportItem);

            }

            actDictionary.Add(item.AccountAndName, new SuitCaseProperties() { PropertiesDictionary = PropertiesDictionary });
        }

        return actDictionary;
    }
    #endregion
}

