using Inverter.Data;
using Inverter.Data.Draw;
using Inverter.Data.Draw.Schema;
using Inverter.Display.ViewsModel;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.Timers;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{
    private DisplayVM bc; //context

    //wykresy
    private ObservableCollection<DataGraph> DataGraphs;
    private List<Graph> _graphs;
    private GraphicsView _gvGraphs;
    //schemat
    private GraphicsView _gvSchema;
    private InverterSchema _inverterSchema;
    private int SActualCurrentIndex;
    private int SCurrentMaxIndex;
    System.Timers.Timer _timer;
    //linia do schematu
    private LineTimeSchema _lineTimeSchema;
    public DisplayV(DisplayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        symulationRunning = false;
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
            #region Wykresy

            bc = (DisplayVM)BindingContext;

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
            #endregion

            #region schemat
            //schemat
            _inverterSchema = new();
            _gvSchema = new();
            _gvSchema.Drawable = _inverterSchema;
            Resources.Add(nameof(InverterSchema), _gvSchema);
            gSchema.Add(_gvSchema);
            SCurrentMaxIndex = DataGraphs.FirstOrDefault().X.Count;
            bc.SCurrentMaxIndex = SCurrentMaxIndex;
            SActualCurrentIndex = bc.SActualCurrentIndex;
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            //linia
            _lineTimeSchema = new();
            gvLineTimeSchematV.Drawable = _lineTimeSchema;
            _lineTimeIsHidden = true;
            #endregion
        }
        catch (Exception)
        {
            throw;
        }

    }
    #region Wykres


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
    private async void UpdateRow_Clicked(object sender, EventArgs e)
    {
        lvDataGraphs.BeginRefresh();
        await ReDrawGraph();
    }
    private async Task ReDrawGraph()
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

                    _gvGraphs = SetGraphicsView(_gvGraphs);
                    _gvGraphs.Drawable = _graphs[i];

                    //Ustawienie lokacji wykresu
                    gGraph.Add(_gvGraphs, 0, DataGraphs[i].LocationRow);
                    //Ustawienie wysokości wykresu
                    gGraph.SetRowSpan(_gvGraphs, DataGraphs[i].locationRowSpan);
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
                    _graphs[i].MultipledGraph = multipledGraph;
                    int n = 30;
                    int secondLoop = 0;
                    for (int j = 0; j < DataGraphs[i].Y.Count; j++)
                    {
                         _graphs[i].point.LineTo(n, -(DataGraphs[i].Y[j] * DataGraphs[i].Multiplier));

                        n++;
                        if (!multipledGraph)
                        {
                            if (j + 1 >= DataGraphs[i].Y.Count && secondLoop < 5)
                            {
                                j = 0;
                                secondLoop++;
                            }
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
    bool multipledGraph = true;
    private async void ckMultipledGraph_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        multipledGraph = !multipledGraph;
        if (DataGraphs != null)
            await ReDrawGraph();
    }
    private async void ckGridVisible_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        gridVisible = !gridVisible;
        if (DataGraphs != null)
            await ReDrawGraph();
    }

    #endregion

    #region Symulacja
    public void TimerEvent(object source, ElapsedEventArgs e)
    {
        SActualCurrentIndex++;
        if (SActualCurrentIndex >= SCurrentMaxIndex)
        {
            SActualCurrentIndex = 0;
        }
        changeTime();
    }
    private bool symulationRunning;
    private void Symulation_Clicked(object sender, EventArgs e)
    {
        SActualCurrentIndex = bc.SActualCurrentIndex;

        if (!symulationRunning)
        {
            symulationRunning = !symulationRunning;
            _timer.Start();
        }
        else if (symulationRunning)
        {
            symulationRunning = !symulationRunning;
            _timer.Stop();
        }

    }
    private void sSymulationTimer_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        SActualCurrentIndex = bc.SActualCurrentIndex;
        changeTime();
    }
    private void changeTime()
    {
        _inverterSchema.Index = SActualCurrentIndex;
        _gvSchema.Invalidate();

        _lineTimeSchema.Index = SActualCurrentIndex;
        gvLineTimeSchematV.Invalidate();
    }
    private bool _lineTimeIsHidden = true;
    private void ckLineTime_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_lineTimeSchema != null)
        {
            _lineTimeIsHidden = !_lineTimeIsHidden;
            _lineTimeSchema.IsHidden = _lineTimeIsHidden;
            gvLineTimeSchematV.Invalidate();
        }
    }

    #endregion


}