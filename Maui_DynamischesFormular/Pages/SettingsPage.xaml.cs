namespace Maui_DynamischesFormular.Pages;

using Maui_DynamischesFormular.ViewModels;

public partial class SettingsPage : ContentPage
{
    //SettingsViewModel vm = new();
    SettingsViewModel vm;

    public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        vm = viewModel;


      //  BindingContext = vm;
    }

    #region OnNavigatedTo
    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        vm.OnNavigatedTo(e);
    }
    #endregion

    #region OnAppearing
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        vm.SettingsPageOnAppearingCommand();
    }
    #endregion
    //protected override async void On

}