using Maui_DynamischesFormular;
using Maui_DynamischesFormular.Pages;

namespace Maui_DynamischesFormular
{ 

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(PersonEditPage), typeof(PersonEditPage));
            //Routing.RegisterRoute(nameof(ProfileDetailPage), typeof(ProfileDetailPage));
            //Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
            //Routing.RegisterRoute(nameof(ShadowFilePage), typeof(ShadowFilePage));

            //Routing.RegisterRoute(nameof(ProfilesPage), typeof(ProfilesPage));
        }

    }
}



