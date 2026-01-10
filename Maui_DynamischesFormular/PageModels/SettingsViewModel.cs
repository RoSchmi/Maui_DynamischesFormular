//using AndroidX.Navigation;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.PageModels;
using Maui_DynamischesFormular.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Maui_DynamischesFormular.PageModels
{

    public partial class SettingsViewModel : ObservableObject, IQueryAttributable
    {

      

        [ObservableProperty]
        private string navigationState;

        [ObservableProperty]
        private string injectedSender;

        [ObservableProperty]
        private string sender;

        private IDictionary<string, object> queryHandle;

        public ObservableCollection<SettingItem> Items { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            queryHandle = query;
            var injectedDictionary = query;
            int breakpoint = 1;
        }
        //public SettingsViewModel(IEnumerable<WorkItem> items)
        public SettingsViewModel()
        {
           // Items = new ObservableCollection<WorkItem>(items);
        }

        public void OnNavigatedTo(NavigatedToEventArgs e)
        {
            NavigationState = Shell.Current.CurrentState.Location.ToString();

            Sender = InjectedSender;
            InjectedSender = string.Empty;
            if (queryHandle != null)
            {
                queryHandle.Clear();
            }

           // public ObservableCollection<SettingItem> Items { get; }
        //public SettingsPageViewModel(IEnumerable<SettingItem> items) { Items = new ObservableCollection<SettingItem>(items); }
         // Items = new ObservableCollection<SettingItem>(query);

        /*
        ConnectionOKBackGround = Colors.LightGrey;
        if (AppearCounter == 0)
        {
            PopulateAccountFilesAction();  // This sets ActAccount
        }
        */

        // AppearCounter++;

        int dummy45 = 1;
        }


        public void SettingsPageOnAppearingCommand()
        {
            int breakpointAppearing = 1;
        }


    }
}
