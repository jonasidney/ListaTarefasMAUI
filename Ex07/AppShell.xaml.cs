using Ex07.Views;

namespace Ex07;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Registro da rota para MainPage em Views
		Routing.RegisterRoute(nameof(Views.MainPage), typeof(Views.MainPage));
	}
}
