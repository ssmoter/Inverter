using Inverter.Display.Views;
using Inverter.GenerateInverter.Views;
using Inverter.Sets;
using Inverter.Options;

namespace Inverter;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(InverterV), typeof(InverterV));
        Routing.RegisterRoute(nameof(DisplayV), typeof(DisplayV));
        Routing.RegisterRoute(nameof(ReadySetsV), typeof(ReadySetsV));
        Routing.RegisterRoute(nameof(GetOptions), typeof(GetOptions));

    }
}
