using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Common.Models;


namespace Maui_DynamischesFormular.Models;

public class TransportItem
{
    public int TabNo { get; set; }
    public string Name { get; set; }

    public string DisplayName { get; set; }
    public WorkItem.TypeID TypeIdentifier { get; set; }
    public object Content { get; set; }
}
