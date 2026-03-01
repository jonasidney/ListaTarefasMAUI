using Ex07.ViewModels;

namespace Ex07.Views;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = new Ex07.ViewModels.MainViewModel();
	}

}
