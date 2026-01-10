using System;
using System.Collections.Generic;
using System.Text;

namespace Maui_DynamischesFormular.Models
{
    internal class ProfileSet
    {
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChartSluuk.Models
{
    public class ProfileSet
    {

        // must expose a parameter-less constructor
        public ProfileSet() { }

        public string Account { get; set; }
        public string Profile { get; set; }

        public string DataSourceTable1 { get; set; }
        public string DataSourceTable2 { get; set; }
        public string DataSourceTable3 { get; set; }
        public string DataSourceTable4 { get; set; }

        public string Table1Property { get; set; }
        public string Table2Property { get; set; }
        public string Table3Property { get; set; }
        public string Table4Property { get; set; }


        public bool Table1AutomaticYear { get; set; }
        public string Table1Provider { get; set; }
        public string Table1Type { get; set; }
        public string Table1Displacement { get; set; }
        public string Table1Unit { get; set; }
        public string Table1Factor { get; set; }
        public DateTime Table1StartDate { get; set; }
        public DateTime Table1EndDate { get; set; }
        public string Table1Account { get; set; }

        //public string T1Ph2 { get; set; }
        public string Table1SortField { get; set; }
        public string T1Ph3 { get; set; }

        public bool Table2AutomaticYear { get; set; }
        public string Table2Provider { get; set; }
        public string Table2Type { get; set; }
        public string Table2Displacement { get; set; }
        public string Table2Unit { get; set; }
        public string Table2Factor { get; set; }
        public DateTime Table2StartDate { get; set; }
        public DateTime Table2EndDate { get; set; }
        //public string T2Ph1 { get; set; }
        public string Table2Account { get; set; }

        //public string T2Ph2 { get; set; }
        public string Table2SortField { get; set; }
        public string T2Ph3 { get; set; }

        public bool Table3AutomaticYear { get; set; }
        public string Table3Provider { get; set; }
        public string Table3Type { get; set; }
        public string Table3Displacement { get; set; }
        public string Table3Unit { get; set; }
        public string Table3Factor { get; set; }
        public DateTime Table3StartDate { get; set; }
        public DateTime Table3EndDate { get; set; }
        //public string T3Ph1 { get; set; }
        public string Table3Account { get; set; }
        //public string T3Ph2 { get; set; }
        public string Table3SortField { get; set; }
        public string T3Ph3 { get; set; }


        public bool Table4AutomaticYear { get; set; }
        public string Table4Provider { get; set; }
        public string Table4Type { get; set; }
        public string Table4Displacement { get; set; }
        public string Table4Unit { get; set; }
        public string Table4Factor { get; set; }
        public DateTime Table4StartDate { get; set; }
        public DateTime Table4EndDate { get; set; }
        public string Table4Account { get; set; }

        //public string T4Ph2 { get; set; }
        public string Table4SortField { get; set; }
        public string T4Ph3 { get; set; }


        public bool SettingsState { get; set; }
        public DateTime SettingsDate { get; set; }
        public string SettingsID { get; set; }
        public string Index { get; set; }
        public string Selected { get; set; }


        /*
        public string SettingsID { get; set; }    //  0  Name of the variable 'SettingsID' may not be changed
        public string Account { get; set; }       //  1  Name of the variable 'Account' may not be changed
        public string Index { get; set; }         //  2  Name of the variable 'Index' may not be changed
        public string Selected { get; set; }      //  3  Name of the variable 'Selected' may not be changed
        public string Profile { get; set; }       //  4  Name of the variable 'Profile' may not be changed but Program should be changed that the name can be changed
        public bool SettingsState { get; set; }
        public DateTime SettingsDate { get; set; }
        public string Cph1 { get; set; }        // 7 Common Placeholder 1
        public string Cph2 { get; set; }        // 8 Common Placeholder 2
        public string Cph3 { get; set; }        // 9 Common Placeholder 3

        public string DataSourceTable1 { get; set; }    // 10
        public string Table1Property { get; set; }      //  11
        public bool Table1AutomaticYear { get; set; }   // 12
        public string Table1Provider { get; set; }      // 13
        public string Table1Type { get; set; }          // 14
        public string Table1Displacement { get; set; }
        public string Table1Unit { get; set; }
        public string Table1Factor { get; set; }
        public DateTime Table1StartDate { get; set; }
        public DateTime Table1EndDate { get; set; }
        public string T1Ph1 { get; set; }
        public string T1Ph2 { get; set; }
        public string T1Ph3 { get; set; }

        public string DataSourceTable2 { get; set; }   // 23
        public string Table2Property { get; set; }
        public bool Table2AutomaticYear { get; set; }
        public string Table2Provider { get; set; }
        public string Table2Type { get; set; }         // 27
        public string Table2Displacement { get; set; }
        public string Table2Unit { get; set; }
        public string Table2Factor { get; set; }
        public DateTime Table2StartDate { get; set; }
        public DateTime Table2EndDate { get; set; }
        public string T2Ph1 { get; set; }
        public string T2Ph2 { get; set; }
        public string T2Ph3 { get; set; }

        public string DataSourceTable3 { get; set; }   // 36
        public string Table3Property { get; set; }
        public bool Table3AutomaticYear { get; set; }
        public string Table3Provider { get; set; }
        public string Table3Type { get; set; }
        public string Table3Displacement { get; set; }
        public string Table3Unit { get; set; }
        public string Table3Factor { get; set; }
        public DateTime Table3StartDate { get; set; }
        public DateTime Table3EndDate { get; set; }
        public string T3Ph1 { get; set; }
        public string T3Ph2 { get; set; }
        public string T3Ph3 { get; set; }


        public string DataSourceTable4 { get; set; }  // 49
        public string Table4Property { get; set; }
        public bool Table4AutomaticYear { get; set; }
        public string Table4Provider { get; set; }
        public string Table4Type { get; set; }
        public string Table4Displacement { get; set; }
        public string Table4Unit { get; set; }
        public string Table4Factor { get; set; }
        public DateTime Table4StartDate { get; set; }
        public DateTime Table4EndDate { get; set; }
        public string T4Ph1 { get; set; }
        public string T4Ph2 { get; set; }
        public string T4Ph3 { get; set; }
        */
    }
}

