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
    /*
    BackButtonBehavior backButtonBehavior = new BackButtonBehavior();
    backButtonBehavior.
        */
    protected override async void OnNavigatedFrom(NavigatedFromEventArgs e)
    {
        base.OnNavigatedFrom(e);
        vm.OnProfileDetailPageNavigatedFromCommand();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        vm.OnProfileDetailPageNavigatedToCommand();
    }


    protected override async void OnNavigatingFrom(NavigatingFromEventArgs e)
    {
        //RoSchmi        
        //var newArgs = new NavigatingFromEventArgs(e.DestinationPage, e.NavigationType);

        base.OnNavigatingFrom(e);
        vm.OnProfileDetailPageNavigatedToCommand();
    }


    /*
    protected override async void OnBackButtonPressed()e.
    {
        base.OnBackButtonPressed();
        vm.OnProfileDetailPageBackButtonPressedCommand();
    }
    */


    /*
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        vm.GraphPageOnAppearingCommand();
    }
    */
}