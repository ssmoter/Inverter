using Inverter.Data;
using Inverter.Data.Draw;
using Inverter.Display.ViewsModel;
using Inverter.Models;
using System.Collections.ObjectModel;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{
    private ObservableCollection<DataGraph> DataGraphs;
    private List<Graph> _graphs;
    private GraphicsView _graphicsDraw;

    public DisplayV(DisplayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await Initialization();
    }

    private async Task Initialization()
    {
        try
        {
            DisplayVM bc = (DisplayVM)BindingContext;

            bc.DataGraphs = new ObservableCollection<DataGraph>(bc.ResponseModel.DataGraphs);
            DataGraphs = bc.DataGraphs;

            DataGraphs = new ObservableCollection<DataGraph>(bc.ResponseModel.OutPutString.Mapping(bc.ResponseModel).DataGraphs);

            _graphs = new();
            if (DataGraphs == null)
            {
                return;
            }
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
        graphicsView.Margin = new Thickness(5, 5, 0, 0);
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
                if (DataGraphs[i].Visible)
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
        ReDrawGraph();
    }

    private void ReDrawGraph()
    {
        try
        {
            List<int> axisX = new List<int>();
            gGraph.Clear();
            //Ustawienie ilości widocznych Wykresów
            var numberCurrentGraph = SetNumberCurrentGraph();
            //Ustawienie ilości wierszy
            SetGraphRowDefinitions(gGraph, numberCurrentGraph);


            for (int i = 0; i < DataGraphs.Count; i++)
            {
                if (DataGraphs[i].Visible)
                {

                    _graphicsDraw = SetGraphicsView(_graphicsDraw);
                    _graphicsDraw.Drawable = _graphs[i];

                    //Ustawienie lokacji wykresu
                    gGraph.Add(_graphicsDraw, 0, DataGraphs[i].LocationRow);
                    //Ustawienie wysokości wykresu
                    gGraph.SetRowSpan(_graphicsDraw, DataGraphs[i].locationRowSpan);
                    //Ustawienie wyświetlanych wartości osi x
                    _graphs[i].AxisX = DataGraphs[i].X;
                    //sprawdzenie czy tylko raz zostanie opisana oś X
                    _graphs[i].AxisXWrite = !axisX.Any(x => x == DataGraphs[i].LocationRow);
                    axisX.Add(DataGraphs[i].LocationRow);
                    //ustawienie koloru wykresu
                    _graphs[i].Color = DataGraphs[i].UserColor.Color;
                    _graphs[i].point = new PathF();
                    //Nazwa wykresy
                    _graphs[i].Name = DataGraphs[i].DataName;
                    //pozycja wykresów
                    int PositionName = 1;
                    PositionName = axisX.FindAll(x => x == DataGraphs[i].LocationRow).Count;
                    _graphs[i].PositionName = PositionName;

                    //os Y
                    _graphs[i].MaxYValue = DataGraphs[i].Y.Max();
                    _graphs[i].MinYValue = DataGraphs[i].Y.Min();
                    _graphs[i].MaxYPosition = DataGraphs[i].Y.Max() * DataGraphs[i].Multiplier;
                    _graphs[i].MinYPositions = DataGraphs[i].Y.Min() * DataGraphs[i].Multiplier;

                    //Czy siatka jest widoczna
                    _graphs[i].GridIsVisible = gridVisible;

                    int n = 30;
                    int secondLoop = 0;
                    for (int j = 0; j < DataGraphs[i].Y.Count; j++)
                    {
                        _graphs[i].point.LineTo(n, -(DataGraphs[i].Y[j] * DataGraphs[i].Multiplier));
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

    bool gridVisible = true;
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        gridVisible = !gridVisible;
        if(DataGraphs!=null)
            ReDrawGraph();

    }
}