namespace Maui_DynamischesFormular.Pages;

using Maui_DynamischesFormular.PageModels;

public partial class SettingsPage : ContentPage
{
    SettingsViewModel vm = new();
    public SettingsPage()
	{
		InitializeComponent();
        BindingContext = vm;
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