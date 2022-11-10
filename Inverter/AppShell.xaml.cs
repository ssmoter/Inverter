using Inverter.Display.Views;

namespace Inverter;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DisplayV), typeof(DisplayV));
    }
}
