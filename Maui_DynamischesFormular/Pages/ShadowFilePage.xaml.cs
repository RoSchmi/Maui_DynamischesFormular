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

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        vm.OnNavigatingFrom(args);
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        vm.Entry_Focused(sender, e);
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        vm.Entry_Unfocused(sender, e);
    }
}