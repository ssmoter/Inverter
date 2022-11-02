using Inverter.Data.Draw;
using Inverter.Display.ViewsModel;
using Inverter.Models;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{
    private List<DataGraph> _dataGraph;
    private List<Graph> _graphs;
    private GraphicsView _graphicsDraw;

    public DisplayV(DisplayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _dataGraph = vm.DataGraphs.ToList();

        Initialization();
    }


    private void Initialization()
    {
        _graphs = new();
        for (int i = 0; i < _dataGraph.Count; i++)
        {
            _graphs.Add(new Graph());
            Resources.Add(i.ToString(), _graphs[i]);
        }
    }

    private GraphicsView SetGraphicsView(GraphicsView graphicsView)
    {
        graphicsView = new();
        graphicsView.Margin = new Thickness(20, 0, 0, 0);
        graphicsView.ZIndex = 10;
        return graphicsView;
    }
    private int SetNumberCurrentGraph()
    {
        int n = 1;
        for (int i = 0; i < _dataGraph.Count; i++)
        {
            if (n < _dataGraph[i].LocationRow)
            {
                n = (int)_dataGraph[i].LocationRow + 1;
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

            for (int i = 0; i < _dataGraph.Count; i++)
            {
                if (_dataGraph[i].Visible)
                {
                    _graphicsDraw = SetGraphicsView(_graphicsDraw);
                    _graphicsDraw.Drawable = _graphs[i];

                    gGraph.Add(_graphicsDraw, 0, (int)_dataGraph[i].LocationRow);
                    gGraph.SetRowSpan(_graphicsDraw, (int)_dataGraph[i].locationRowSpan);

                    _graphs[i].Color = _dataGraph[i].UserColor;
                    _graphs[i].point = new PathF();
                    _graphs[i].point.Move(200, (_dataGraph[i].Y.FirstOrDefault() * (float)_dataGraph[i].Multiplier));

                    for (int j = 0; j < _dataGraph[i].Y.Count; j++)
                    {
                        _graphs[i].point.LineTo(j, _dataGraph[i].Y[j] * (float)_dataGraph[i].Multiplier);
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