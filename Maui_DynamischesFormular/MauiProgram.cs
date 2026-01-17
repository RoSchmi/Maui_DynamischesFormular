using CommunityToolkit.Maui;
using Maui_DynamischesFormular.ViewModels;
using Microsoft.Extensions.Logging;

namespace Maui_DynamischesFormular
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ViewModels.MainPageViewModel>();
            builder.Services.AddSingleton<Pages.PersonEditPage>();
            builder.Services.AddSingleton<Pages.SettingsPage>();
            builder.Services.AddSingleton<ViewModels.SettingsViewModel>();
            builder.Services.AddSingleton<Pages.ProfileDetailPage>();
            builder.Services.AddSingleton<ViewModels.ProfileDetailViewModel>();
            builder.Services.AddSingleton<Pages.ShadowFilePage>();
            builder.Services.AddSingleton<ViewModels.ShadowFileViewModel>();


            
            //builder.Services.AddSingleton<Pages.TestPage>();
            //builder.Services.AddSingleton<Pages.ProfilesPage>();

           

            return builder.Build();
        }
    }
}
