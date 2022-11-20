using Inverter.GenerateInverter.ViewsModel;

namespace Inverter.GenerateInverter.Views;

public partial class InverterV : ContentPage
{
    public InverterV(GenerateMV vm)
    {
        InitializeComponent();
        BindingContext = vm;        
    }
}