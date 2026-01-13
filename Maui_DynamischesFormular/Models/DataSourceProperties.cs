using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.Models;

public class DataSourceProperties
{
    // must expose a parameter-less constructor
    public DataSourceProperties() { }

    public Dictionary<string, string> Properties { get; set; }

    /*
    public string DataSoureID { get; set; }
    public string DataSoureName { get; set; }
    public string BaseTableName { get; set; }
    public string UsesYyyyTableNameEnding { get; set; }
    public string TableName1 { get; set; }
    public string TableName2 { get; set; }
    public string TableName3 { get; set; }
    public string TableName4 { get; set; }
    public string TableDataColumnName { get; set; }
    */
}



