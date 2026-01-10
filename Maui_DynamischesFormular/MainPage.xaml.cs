using Maui_DynamischesFormular;
using Maui_DynamischesFormular.Models;
using Maui_DynamischesFormular.ViewModels;
using System;
using System.Collections.ObjectModel;


namespace Maui_DynamischesFormular;

public partial class MainPage : ContentPage
{
    // RoSchmi: Important:

    // For MVVM in .xaml has to be included:            
    // xmlns:pagemodel="clr-namespace:Maui_DynamischesFormular"
    // x:DataType="pageModel:MainPageViewModel">
    // In 'MauiProgram.cs' References to MainPage and MainPageViewModel have to be added
    // In 'AppShell.xaml' the 'ShellContent' for each page has to be added 
    // In 'AppShell.xaml.cs' the Navigation routes have to be registered
    // 
    // For Windows the initial Windowsize and -position are set in an override
    // in 'App.xaml.cs'

    //private readonly MainPageViewModel vm;
    private MainPageViewModel vm;

    public MainPage()
    {
        InitializeComponent();
        vm = new MainPageViewModel();

        BindingContext = vm;
    }


    #region OnAppearing
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        vm.GraphPageOnAppearingCommand();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        vm.OnMainPageNavigatedToCommand(e);
    }
    #endregion

}
