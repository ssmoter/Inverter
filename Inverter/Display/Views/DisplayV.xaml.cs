using Inverter.Data;
using Inverter.Data.Draw;
using Inverter.Display.ViewsModel;
using Inverter.Models;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{
    private List<DataGraph> DataGraphs;
    private List<Graph> _graphs;
    private GraphicsView _graphicsDraw;

    public DisplayV(DisplayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        // DataGraphs = vm.DataGraphs.ToList();

        //  Initialization();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Initialization();
    }

    private void Initialization()
    {
        try
        {
            DisplayVM bc = (DisplayVM)BindingContext;

            DataGraphs = bc.DataGraphs;

            FileManager fm = new FileManager();
            ResponseModel rs = new ResponseModel();
            rs.DataGraphs = DataGraphs;

            DataGraphs = fm.OpenFile().Result.Mapping(rs).DataGraphs;


            _graphs = new();
            for (int i = 0; i < DataGraphs.Count; i++)
            {
                _graphs.Add(new Graph());
                Resources.Add(i.ToString(), _graphs[i]);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }

    private GraphicsView SetGraphicsView(GraphicsView graphicsView)
    {
        graphicsView = new();
        graphicsView.Margin = new Thickness(20, 5, 0, 0);
        graphicsView.ZIndex = 10;
        return graphicsView;
    }
    private int SetNumberCurrentGraph()
    {
        int n = 1;
        for (int i = 0; i < DataGraphs.Count; i++)
        {
            if (n <= DataGraphs[i].LocationRow)
            {
                n = (int)DataGraphs[i].LocationRow + 1;
            }
        }
        return n;
    }

    private void SetGraphRowDefinitions(Grid views, int NumberOfRows)
    {
        views.RowDefinitions.Clear();
        for (int i = 0; i < NumberOfRows; i++)
        {
            views.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Star)));
        }
    }

    private void UpdateRow_Clicked(object sender, EventArgs e)
    {
        try
        {
            gGraph.Clear();
            var numberCurrentGraph = SetNumberCurrentGraph();
            SetGraphRowDefinitions(gGraph, numberCurrentGraph);

            for (int i = 0; i < DataGraphs.Count; i++)
            {
                if (DataGraphs[i].Visible)
                {
                    _graphicsDraw = SetGraphicsView(_graphicsDraw);
                    _graphicsDraw.Drawable = _graphs[i];

                    gGraph.Add(_graphicsDraw, 0, DataGraphs[i].LocationRow);
                    gGraph.SetRowSpan(_graphicsDraw, DataGraphs[i].locationRowSpan);

                    _graphs[i].Color = DataGraphs[i].UserColor;
                    _graphs[i].point = new PathF();
                    _graphs[i].MaxY = DataGraphs[i].Y.Max(x => Math.Abs(x));
                    int n = 30;
                    int secondLoop = 0;
                    for (int j = 0; j < DataGraphs[i].Y.Count; j++)
                    {
                        _graphs[i].point.LineTo(n, DataGraphs[i].Y[j] * (float)DataGraphs[i].Multiplier);
                        n++;
                        if (j + 1 >= DataGraphs[i].Y.Count && secondLoop != 4)
                        {
                            j = 0;
                            secondLoop++;
                        }
                    }

                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}