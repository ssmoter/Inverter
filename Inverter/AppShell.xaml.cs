using Inverter.Display.Views;
using Inverter.GenerateInverter.Views;

namespace Inverter;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(InverterV), typeof(InverterV));
        Routing.RegisterRoute(nameof(DisplayV), typeof(DisplayV));

    }
}
