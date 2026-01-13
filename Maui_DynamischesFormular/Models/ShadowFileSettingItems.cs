using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui_DynamischesFormular.Models;

public class ShadowFileSettingItems
{
    public ShadowFileSettingItems() { }
    public string Sender { get; set; }
    public string AccountName { get; set; }
    public string TableAccount { get; set; }
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string ColumnType { get; set; }
    public string Factor { get; set; }
    public string Instruction { get; set; }
}