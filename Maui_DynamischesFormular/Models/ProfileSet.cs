using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Maui_DynamischesFormular.Models
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

        //RoSchmi
        //public StringTypeContent Table1Property { get; set; }


        public string TableProperty1 { get; set; }
        public string TableProperty2 { get; set; }
        public string TableProperty3 { get; set; }
        public string TableProperty4 { get; set; }




        public bool TableAutomaticYear1 { get; set; }
        public string TableProvider1 { get; set; }
        public string TableType1 { get; set; }
        public string TableDisplacement1 { get; set; }
        public string TableUnit1 { get; set; }
        public string TableFactor1 { get; set; }
        public string TableOffset1 { get; set; }
        public DateTime TableStartDate1 { get; set; }
        public DateTime TableEndDate1 { get; set; }
        public string TableAccount1 { get; set; }
        public string TableCloudTable1 { get; set; }
        public string TableSortField1 { get; set; }
        public string TablePh31 { get; set; }
        public bool TableAutomaticYear2 { get; set; }
        public string TableProvider2 { get; set; }
        public string TableType2 { get; set; }
        public string TableDisplacement2 { get; set; }
        public string TableUnit2 { get; set; }
        public string TableFactor2 { get; set; }
        public string TableOffset2 { get; set; }
        public DateTime TableStartDate2 { get; set; }
        public DateTime TableEndDate2 { get; set; }

        public string TableAccount2 { get; set; }

        public string TableCloudTable2 { get; set; }     
        public string TableSortField2 { get; set; }
        public string TablePh32 { get; set; }

        public bool TableAutomaticYear3 { get; set; }
        public string TableProvider3 { get; set; }
        public string TableType3 { get; set; }
        public string TableDisplacement3 { get; set; }
        public string TableUnit3 { get; set; }
        public string TableFactor3 { get; set; }
        public string TableOffset3 { get; set; }
        public DateTime TableStartDate3 { get; set; }
        public DateTime TableEndDate3 { get; set; }

        public string TableAccount3 { get; set; }

        public string TableCloudTable3 { get; set; }

        public string TableSortField3 { get; set; }
        public string TablePh33 { get; set; }


        public bool TableAutomaticYear4 { get; set; }
        public string TableProvider4 { get; set; }
        public string TableType4 { get; set; }
        public string TableDisplacement4 { get; set; }
        public string TableUnit4 { get; set; }
        public string TableFactor4 { get; set; }
        public string TableOffset4 { get; set; }
        public DateTime TableStartDate4 { get; set; }
        public DateTime TableEndDate4 { get; set; }
        public string TableAccount4 { get; set; }
        public string TableCloudTable4 { get; set; }    
        public string TableSortField4 { get; set; }
        public string TablePh34 { get; set; }


        public bool SettingsState { get; set; }
        public DateTime SettingsDate { get; set; }
        public string SettingsID { get; set; }
        public string Index { get; set; }
        public string Selected { get; set; }

        /*
            public string Account { get; set; }
            public string Profile { get; set; }

            public string DataSourceTable1 { get; set; }
            public string DataSourceTable2 { get; set; }
            public string DataSourceTable3 { get; set; }
            public string DataSourceTable4 { get; set; }

            //RoSchmi
            //public StringTypeContent Table1Property { get; set; }


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
            public string Table1Offset { get; set; }
            public DateTime Table1StartDate { get; set; }
            public DateTime Table1EndDate { get; set; }
            public string Table1Account { get; set; }
            public string Table1CloudTable { get; set; }         
            public string Table1SortField { get; set; }
            public string T1Ph3 { get; set; }
            public bool Table2AutomaticYear { get; set; }
            public string Table2Provider { get; set; }
            public string Table2Type { get; set; }
            public string Table2Displacement { get; set; }
            public string Table2Unit { get; set; }
            public string Table2Factor { get; set; }
            public string Table2Offset { get; set; }
            public DateTime Table2StartDate { get; set; }
            public DateTime Table2EndDate { get; set; }
            
            public string Table2Account { get; set; }

            public string Table2CloudTable { get; set; }

            //public string T2Ph2 { get; set; }
            public string Table2SortField { get; set; }
            public string T2Ph3 { get; set; }

            public bool Table3AutomaticYear { get; set; }
            public string Table3Provider { get; set; }
            public string Table3Type { get; set; }
            public string Table3Displacement { get; set; }
            public string Table3Unit { get; set; }
            public string Table3Factor { get; set; }
            public string Table3Offset { get; set; }
            public DateTime Table3StartDate { get; set; }
            public DateTime Table3EndDate { get; set; }
            
            public string Table3Account { get; set; }

            public string Table3CloudTable { get; set; }
        
            public string Table3SortField { get; set; }
            public string T3Ph3 { get; set; }


            public bool Table4AutomaticYear { get; set; }
            public string Table4Provider { get; set; }
            public string Table4Type { get; set; }
            public string Table4Displacement { get; set; }
            public string Table4Unit { get; set; }
            public string Table4Factor { get; set; }
            public string Table4Offset { get; set; }
            public DateTime Table4StartDate { get; set; }
            public DateTime Table4EndDate { get; set; }
            public string Table4Account { get; set; }
            public string Table4CloudTable { get; set; }

            //public string T4Ph2 { get; set; }
            public string Table4SortField { get; set; }
            public string T4Ph3 { get; set; }


            public bool SettingsState { get; set; }
            public DateTime SettingsDate { get; set; }
            public string SettingsID { get; set; }
            public string Index { get; set; }
            public string Selected { get; set; }
        */

    }
   
}

