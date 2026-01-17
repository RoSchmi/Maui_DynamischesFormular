using Maui_DynamischesFormular.ViewModels;

namespace Maui_DynamischesFormular.Pages;

public partial class ShadowFilePage : ContentPage
{
    // ShadowFileViewModel vm = new();
    ShadowFileViewModel vm;


    public ShadowFilePage(ShadowFileViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        vm = viewModel;
        //vm.InjectedSender = sender;
        //vm.TransmissionObject = transmissionObject;
    }


    /*
    public ShadowFilePage(string sender, object transmissionObject)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.InjectedSender = sender;
        vm.TransmissionObject = transmissionObject;
    }
    */

    


    #region OnNavigatedTo
    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        vm.OnNavigatedTo(e);
    }
    #endregion

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
        vm.OnNavigatingFrom(e);
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

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        vm.Entry_Focused(sender, e);
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        vm.Entry_Unfocused(sender, e);
    }
}