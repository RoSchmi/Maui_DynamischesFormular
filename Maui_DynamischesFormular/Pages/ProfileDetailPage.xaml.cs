using Maui_DynamischesFormular.ViewModels;
using Maui_DynamischesFormular.Models;


namespace Maui_DynamischesFormular.Pages;

public partial class ProfileDetailPage : ContentPage
{

    //ProfileDetailViewModel vm = new();
    ProfileDetailViewModel vm;

    public ProfileDetailPage(ProfileDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        vm = viewModel;
    }
   

    

    protected override async void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        vm.OnProfileDetailPageNavigatedToCommand();
    }

    #region Region not used and outcommented events 

    /*
    protected override bool OnBackButtonPressed()
    {
        vm.BackCommand.Execute(null);
        return true; // verhindert Shell-Standardnavigation
    }
    */


    /*
    protected override async void OnNavigatingFrom(NavigatingFromEventArgs e)
    {
        base.OnNavigatingFrom(e);
    }
    */

    /*
    protected override async void OnNavigatedFrom(NavigatedFromEventArgs e)
    {
        base.OnNavigatedFrom(e)
    }
    */

    /*
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        vm.GraphPageOnAppearingCommand();
    }
    */
    #endregion

    private void Picker_Opened(object sender, PickerOpenedEventArgs e)
    {
        /*
        if (sender is not Picker picker) 
            return;
        
        // Das WorkItem, zu dem dieser Picker gehört
        if (picker.BindingContext is not WorkItem actItem) 
            return; // Dein Page-ViewModel

        if (BindingContext is not ProfileDetailViewModel vm) 
            return;
        
        // Jetzt hast du ALLES, was du brauchst:
        // Beispiel: kontextabhängig AllowedPickerItems setzen

        // Wenn TabNo nicht 0 ist, nach dem um eine Stelle gekürzten String suchen (letzte Stelle repräsentiert Tabellenzugehörigkeit)  
        var baseName = actItem.TabNo == 0 ? actItem.Name : string.IsNullOrEmpty(actItem.Name) ? string.Empty : actItem.Name[..^1];
        
        vm.AllowedPickerItems.Clear(); 
        switch (baseName) 
        { 
            case "TableProvider":          
                vm.AllowedPickerItems.Add("Azure Storage"); 
                vm.AllowedPickerItems.Add("AWS-Datbase (not impl.)");
                vm.AllowedPickerItems.Add("Google-Datbase (not impl.)");
                
                break; 
            case "TableType": 
                vm.AllowedPickerItems.Add("String"); 
                vm.AllowedPickerItems.Add("DateTime");
                vm.AllowedPickerItems.Add("Float");
                break; 
            default: 
                vm.AllowedPickerItems.Add(""); 
                break; }
        picker.SelectedItem = actItem.StringValue;
        System.Diagnostics.Debugger.Break();

        int breakpoint63 = 1;
        */
    }

    private void Picker_BindingContextChanged(object sender, EventArgs e)
    {
        
    }

    private void Picker_HandlerChanged(object sender, EventArgs e)
    {
      
    }

    private void Picker_Loaded(object sender, EventArgs e)
    {
        if (sender is not Picker picker)
            return;

        // Das WorkItem, zu dem dieser Picker gehört
        if (picker.BindingContext is not WorkItem actItem)
            return; // Dein Page-ViewModel

        if (BindingContext is not ProfileDetailViewModel vm)
            return;

        // Jetzt hast du ALLES, was du brauchst:
        // Beispiel: kontextabhängig AllowedPickerItems setzen

        // Wenn TabNo nicht 0 ist, nach dem um eine Stelle gekürzten String suchen (letzte Stelle repräsentiert Tabellenzugehörigkeit)  
        var baseName = actItem.TabNo == 0 ? actItem.Name : string.IsNullOrEmpty(actItem.Name) ? string.Empty : actItem.Name[..^1];

        switch (baseName)
        {
            case "TableProvider":         
                actItem.AllowedPickerItems.Clear();

                actItem.AllowedPickerItems.Add("Azure Storage");
                actItem.AllowedPickerItems.Add("AWS-Datbase (not impl.)");
                actItem.AllowedPickerItems.Add("Google-Datbase (not impl.)");

                actItem.SelectedPickerItem = actItem.StringValue;
                break;
            case "TableType":              
                actItem.AllowedPickerItems.Clear();

                actItem.AllowedPickerItems.Add("String");
                actItem.AllowedPickerItems.Add("DateTime");
                actItem.AllowedPickerItems.Add("Float");

                actItem.SelectedPickerItem = actItem.StringValue;
                break;

            default:
                actItem.AllowedPickerItems.Clear();
                actItem.AllowedPickerItems.Add("");

                actItem.SelectedPickerItem = actItem.StringValue;
                break;
        }      
    }
}