namespace Maui_DynamischesFormular.Pages;

using Maui_DynamischesFormular.ViewModels;

public partial class SettingsPage : ContentPage
{
    SettingsViewModel vm;

    public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        vm = viewModel;
        TableViewAccounts.Root = vm.AccountsTableRoot;
    }

    #region OnNavigatedTo
    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        vm.OnNavigatedTo(e);
    }
    #endregion


    #region Region nicht verwendete Events
        //protected override void OnAppearing()
        //protected override void OnDisappearing()
        //protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    #endregion



    }