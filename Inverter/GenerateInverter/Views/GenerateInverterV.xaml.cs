using Inverter.GenerateInverter.ViewsModel;

namespace Inverter.GenerateInverter.Views;

public partial class InverterV : ContentPage
{
    public InverterV(GenerateMV vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void rvMain_SizeChanged(object sender, EventArgs e)
    {
        rvMain.MaximumHeightRequest = this.Height;
        rvMain.MaximumWidthRequest = this.Width;
    }
}