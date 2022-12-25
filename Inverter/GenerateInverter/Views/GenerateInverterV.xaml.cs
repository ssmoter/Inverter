using Inverter.Data.Draw.Schema;
using Inverter.GenerateInverter.ViewsModel;
using Inverter.Models;

namespace Inverter.GenerateInverter.Views;

public partial class InverterV : ContentPage
{
    public InverterV(GenerateMV vm)
    {
        InitializeComponent();
        BindingContext = vm;

        GraphicsView graphics= new GraphicsView();
        List<DataGraph> dataGraphs = new List<DataGraph>();
        dataGraphs = new InverterParameters().DefaultPrintTran;
        for (int i = 0; i < dataGraphs.Count; i++)
        {
            dataGraphs[i].Y.Add(0);
        }

        InverterSchema _inverterSchema = new InverterSchema(dataGraphs);


        _inverterSchema.BlackWhite = true;
        _inverterSchema.MinYValue= 0;
        _inverterSchema.MaxYValue= 0;
        _inverterSchema.Index = 0;
        graphics.Drawable = _inverterSchema;
        Resources.Add(nameof(InverterSchema), graphics);
        gPreview.Add(graphics,0,2);
    }

    private void rvMain_SizeChanged(object sender, EventArgs e)
    {
        rvMain.MaximumHeightRequest = this.Height;
        rvMain.MaximumWidthRequest = this.Width;
    }
}