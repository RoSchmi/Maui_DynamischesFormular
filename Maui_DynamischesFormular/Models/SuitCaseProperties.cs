//using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Maui_DynamischesFormular.Models;
/// <summary>
/// Wraps a Dictionary&lt;string, TransportItem> into a SuitCase object
/// </summary>

public class SuitCaseProperties
{
    public SuitCaseProperties() { }

    // The properties get wrapped in this 'SuitCase' Dictionary

    public Dictionary<string, TransportItem> PropertiesDictionary { get; set; }
}
