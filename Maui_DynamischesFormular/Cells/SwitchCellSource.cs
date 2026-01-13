using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Maui.Controls;


namespace Maui_DynamischesFormular.Cells
{
    public class SwitchCellSource
    {
        private List<string> names;
        private bool[] namesStates;

        private TableSection section;

        public enum CellAction
        {
            select,
            delete,
            leave
        }

        public SwitchCellSource(List<string> pNames, TableSection pSection)
        {
            // values of the calling class are changed through operations in the called class
            names = pNames;
            section = pSection;
        }

        public void Populate(List<string> pNames)
        {
            names = pNames;
            namesStates = new bool[names.Count];
            for (int i1 = 0; i1 < namesStates.Length; i1++)
            {
                namesStates[i1] = false;
            }

            // clears also the section in the calling class
            section.Clear();

            if (names.Count > 0)
            {
                // clears also the section in the calling class
                //section.Clear();

                foreach (string itemName in names)
                {
                    var switchCell = new SwitchCell() { Text = itemName, On = false };

                    section.Add(switchCell);
                    if (itemName != "")
                    {
                        var toggledEventArgs = new ToggledEventArgs(switchCell.On);
                        switchCell.OnChanged += (object sender2, ToggledEventArgs e2) => OnSwitchCell_toggled(this, toggledEventArgs, itemName);
                        switchCell.Tapped += OnSwitchCell_Tapped;
                    }
                }
            }
        }

        /*
        private void SwitchCell_OnChanged(object sender, ToggledEventArgs e)
        {
            int index = names.FindIndex(x => x == ((SwitchCell)sender).Text);
            if ((index == -1) || (index >= names.Count))
            {
                throw new Exception("ItemName not found, this should not occur");
            }

            SwitchCellSourceEventArgs eventArgs = new SwitchCellSourceEventArgs(
            namesStates[index] == true ? CellAction.delete : CellAction.select, ((SwitchCell)sender).Text);

            OnSwitchCellSourceSend(this, eventArgs);
            
        }
        */

        /*
        private void Section_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
            int index = 0;
            try
            {
                index = names.FindIndex(x => x == ((SwitchCell)e.NewItems[0]).Text);
            }
            catch
            { }


           // int index = names.FindIndex(x => x == ((SwitchCell)e.NewItems[0]).Text);
            if ((index == -1) || (index >= names.Count))
            {
                throw new Exception("Account not found, this should not occur");
            }

            SwitchCellSourceEventArgs eventArgs = new SwitchCellSourceEventArgs(
            namesStates[index] == true ? CellAction.delete : CellAction.select, ((SwitchCell)e.NewItems[0]).Text);

            OnSwitchCellSourceSend(this, eventArgs);
            }

        }
        */


        private void OnSwitchCell_Tapped(object sender, EventArgs e)
        {
            int index = names.FindIndex(x => x == ((SwitchCell)sender).Text);
            if ((index == -1) || (index >= names.Count))
            {
                throw new Exception("ItemName not found, this should not occur");
            }
            var eventArgs = new SwitchCellSourceEventArgs(
            namesStates[index] == true ? CellAction.delete : CellAction.select, ((SwitchCell)sender).Text);

            OnSwitchCellSourceSend(this, eventArgs);
        }

        public void OnSwitchCell_toggled(object sender, ToggledEventArgs toggledEventArgs, string itemName)
        {

            int index = -1;

            index = names.FindIndex(x => x == itemName);

            if (index == -1)
            {
                index = names.FindIndex(x => x == itemName + " (DEL)");
            }

            if ((index == -1) || (index >= names.Count))
            {
                throw new Exception("itemName not found, this should not occur");
            }
            if (namesStates[index] == false)
            {
                names[index] += " (DEL)";
                ((SwitchCell)section[index]).Text += " (DEL)";
            }
            else
            {
                if (names[index].Length == itemName.Length + 6)
                {
                    names[index] = names[index].Substring(0, names[index].Length - 6);
                    ((SwitchCell)section[index]).Text = names[index];
                }
            }

            var eventArgs = new SwitchCellSourceEventArgs(
            CellAction.leave, itemName);
            namesStates[index] = !namesStates[index];

            OnSwitchCellSourceSend(this, eventArgs);
        }

        public delegate void switchCellSourceEventhandler(SwitchCellSource sender, SwitchCellSourceEventArgs e);

        /// <summary>
        /// Raised when a message from the SwitchCellSource is received
        /// </summary>
        public event switchCellSourceEventhandler SwitchCellSourceSend;

        private switchCellSourceEventhandler onSwitchCellSourceSend;

        private void OnSwitchCellSourceSend(SwitchCellSource sender, SwitchCellSourceEventArgs e)
        {
            if (this.onSwitchCellSourceSend == null)

            {
                this.onSwitchCellSourceSend = this.OnSwitchCellSourceSend;
            }

            SwitchCellSourceSend(sender, e);
        }

        public class SwitchCellSourceEventArgs : EventArgs
        {
            public SwitchCellSource.CellAction Action
            { get; private set; }

            public string ItemName
            { get; private set; }

            internal SwitchCellSourceEventArgs(CellAction pAction, string pItemName)
            {
                ItemName = pItemName;
                Action = pAction;
            }
        }
    }
}
