using Maui_DynamischesFormular.ViewModels;

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

}