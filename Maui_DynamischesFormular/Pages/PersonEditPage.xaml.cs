using Maui_DynamischesFormular.ViewModels;

namespace Maui_DynamischesFormular.Pages
{

	public partial class PersonEditPage : ContentPage
	{
		public PersonEditPage(MainPageViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
		}
	}
}