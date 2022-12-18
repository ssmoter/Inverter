using Inverter.Data;
using Inverter.Data.Draw;
using Inverter.Data.Draw.Schema;
using Inverter.Display.ViewsModel;
using Inverter.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{
    FileManager _fm;
    private string _name;

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
        eStrokeSize.Text = strokeSize.ToString();
        _fm = new FileManager();
        try
        {
            Initialization();
        }
        catch
        { }

    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Task t = Initialization();
        await t;
        //await Initialization();
    }

    private async Task Initialization()
    {
        try
        {
            if (BindingContext is DisplayVM)
            {
                bc = (DisplayVM)BindingContext;
                bc.DataGraphs = new ObservableCollection<DataGraph>(bc.ResponseModel.DataGraphs);
                DataGraphs = bc.DataGraphs;
                _name = bc.ResponseModel.FileDataPath;
                if (!bc.ResponseModel.IsReady)
                {
                    DataGraphs = new ObservableCollection<DataGraph>(bc.ResponseModel.OutPutString.Mapping(bc.ResponseModel).DataGraphs);
                }


                #region Wykresy




                _graphs = new();
                if (DataGraphs == null)
                {
                    return;
                }
                for (int i = 0; i < DataGraphs.Count; i++)
                {
                    DataGraphs[i].SetMaxMin();
                    _graphs.Add(new Graph());
                    Resources.Add(i.ToString(), _graphs[i]);
                }
                #endregion

                #region schemat
                //schemat
                _inverterSchema = new(DataGraphs.ToList());
                _inverterSchema.MaxYValue = DataGraphs.Where(x => x.DataName == "I(VoA)").FirstOrDefault().Max;
                _inverterSchema.MinYValue = DataGraphs.Where(x => x.DataName == "I(VoA)").FirstOrDefault().Min;

                _gvSchema = new();
                _gvSchema.Drawable = _inverterSchema;
                Resources.Add(nameof(InverterSchema), _gvSchema);
                gSchema.Add(_gvSchema);
                SCurrentMaxIndex = DataGraphs.FirstOrDefault().X.Count - 1;
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
        await ReDrawGraph();
    }
    private async Task ReDrawGraph()
    {
        try
        {

            var ser = JsonConvert.SerializeObject(DataGraphs);
            var der = JsonConvert.DeserializeObject<List<DataGraph>>(ser);

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
                    //grubosc lini
                    try
                    {
                        strokeSize = float.Parse(eStrokeSize.Text);
                    }
                    catch
                    {
                        strokeSize = 1.5f;
                    }
                    _graphs[i].StrokeSize = strokeSize;
                    //Ustawienie lokacji wykresu
                    gGraph.Add(_gvGraphs, 0, DataGraphs[i].LocationRow);
                    //Ustawienie wysokości wykresu
                    gGraph.SetRowSpan(_gvGraphs, DataGraphs[i].locationRowSpan);
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
                    //tylko wybrana wartosc
                    startIndex = 0;
                    endIndex = DataGraphs[i].Y.Count;
                    #region OsX =user
                    if (scopeGraph)
                    {
                        float valueStart = 0;
                        float valueEnd = DataGraphs[i].X.LastOrDefault();
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(eStartScope.Text))
                            {
                                valueStart = float.Parse(eStartScope.Text);
                            }
                            if (!string.IsNullOrWhiteSpace(eEndScope.Text))
                            {
                                valueEnd = float.Parse(eEndScope.Text);
                                if (valueEnd <= 0)
                                {
                                    valueEnd = DataGraphs[i].X.LastOrDefault();
                                }
                            }
                        }
                        catch
                        {
                        }
                        startIndex = DataGraphs[i].Y.Count;
                        endIndex = 0;
                        for (int j = 1; j < DataGraphs[i].Y.Count - 1; j++)
                        {
                            if (valueStart >= DataGraphs[i].X[j])
                            {
                                if (valueStart <= DataGraphs[i].X[j + 1])
                                {
                                    startIndex = j;
                                }
                            }
                            if (valueEnd <= DataGraphs[i].X[j])
                            {
                                if (valueEnd >= DataGraphs[i].X[j - 1])
                                {
                                    endIndex = j;
                                }
                            }
                        }
                        if (valueStart <= DataGraphs[i].X.FirstOrDefault())
                        {
                            startIndex = 0;
                        }
                        if (valueEnd >= DataGraphs[i].X.LastOrDefault())
                        {
                            endIndex = DataGraphs[i].X.Count;
                        }
                    }
                    data.X = new List<float>();
                    data.Y = new List<float>();
                    data.DataName = DataGraphs[i].DataName;
                    data.UserDataName = DataGraphs[i].UserDataName;
                    data.UserColor = DataGraphs[i].UserColor;
                    data.Multiplier = DataGraphs[i].Multiplier;
                    data.Visible = DataGraphs[i].Visible;
                    data.LocationRow = DataGraphs[i].LocationRow;
                    data.locationRowSpan = DataGraphs[i].locationRowSpan;
                    if (startIndex != DataGraphs.FirstOrDefault().Y.Count || endIndex != 0)
                    {
                        for (int k = startIndex; k < endIndex; k++)
                        {
                            data.X.Add(DataGraphs[i].X[k]);
                            data.Y.Add(DataGraphs[i].Y[k]);
                        }
                    }
                    else
                    {
                        data.X = DataGraphs[i].X;
                        data.Y = DataGraphs[i].Y;
                    }
                    #endregion
                    //os Y
                    _graphs[i].MaxYValue = DataGraphs.Max(x => x.Max);
                    _graphs[i].MinYValue = DataGraphs.Min(x => x.Min);
                    _graphs[i].MaxYPosition = data.Max * data.Multiplier;
                    _graphs[i].MinYPositions = data.Min * data.Multiplier;
                    //font size
                    _graphs[i].FontSize = bc.FontSize;
                    //Czy siatka jest widoczna
                    _graphs[i].GridIsVisible = gridVisible;
                    _graphs[i].MultipledGraph = multipledGraph;

                    //Ustawienie wyświetlanych wartości osi x
                    _graphs[i].AxisX = data.X;
                    int n = 1;
                    int secondLoop = 0;
                    //Ustawienie wyświetlanych wartości osi x
                    _graphs[i].AxisX = data.X;
                    //zakres wyświetlanych wartości na X
                    _graphs[i].StartScopeIndex = startIndex;
                    _graphs[i].EndScopeIndex = endIndex;
                    //skalowanie osi X
                    _graphs[i].AutoScaleX = autoScaleX;

                    for (int j = 0; j < (endIndex - startIndex); j++)
                    {
                        if (data.Multiplier != 0)
                        {
                            _graphs[i].AutoScaleY = false;
                            _graphs[i].point.LineTo(n, -(data.Y[j] * data.Multiplier));
                        }
                        else
                        {
                            _graphs[i].AutoScaleY = true;
                            _graphs[i].point.LineTo(n, -data.Y[j]);
                        }

                        n++;
                        if (!multipledGraph)
                        {
                            if (j + 1 >= data.Y.Count && secondLoop < 10)
                            {
                                j = 0;
                                secondLoop++;
                            }
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(Environment.NewLine);
            Debug.WriteLine(ex);
            // throw;
        }
    }
    DataGraph data = new DataGraph();

    int startIndex;
    int endIndex;
    float strokeSize = 0f;
    bool gridVisible = true;
    bool multipledGraph = true;
    bool scopeGraph = false;
    bool autoScaleX = false;
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
    private async void ckGridScope_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        scopeGraph = !scopeGraph;
        if (DataGraphs != null)
            await ReDrawGraph();
    }
    private async void ckExtendAxisX_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        autoScaleX = !autoScaleX;
        if (DataGraphs != null)
            await ReDrawGraph();
        if (_lineTimeSchema != null)
        {
            _lineTimeSchema.StartScopeIndex = startIndex;
            _lineTimeSchema.EndScopeIndex = endIndex;
            _lineTimeSchema.AutoScaleX = autoScaleX;
            gvLineTimeSchematV.Invalidate();
        }

    }
    private async void eStrokeSize_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataGraphs != null)
            await ReDrawGraph();
    }
    private async void eScope_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataGraphs != null)
            await ReDrawGraph();
    }
    private async void bClearVisible_Clicked(object sender, EventArgs e)
    {
        if (DataGraphs != null)
        {
            for (int i = 0; i < DataGraphs.Count; i++)
            {
                DataGraphs[i].Visible = false;
            }
            bc.DataGraphs = DataGraphs;
            await ReDrawGraph();
        }
    }

    #endregion

    #region Symulacja
    public void TimerEvent(object source, ElapsedEventArgs e)
    {
        if (SActualCurrentIndex >= SCurrentMaxIndex)
        {
            SActualCurrentIndex = 0;
        }
        SActualCurrentIndex++;
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
        _lineTimeSchema.StartScopeIndex = startIndex;
        _lineTimeSchema.EndScopeIndex = endIndex;
        _lineTimeSchema.AutoScaleX = autoScaleX;
        changeTime();
    }
    private void changeTime()
    {
        _inverterSchema.Index = SActualCurrentIndex;
        _gvSchema.Invalidate();

        _lineTimeSchema.Index = SActualCurrentIndex;
        if (autoScaleX)
        {
            _lineTimeSchema.StartScopeIndex = startIndex;
            _lineTimeSchema.EndScopeIndex = endIndex;
            _lineTimeSchema.AutoScaleX = autoScaleX;
        }
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

    private void svGraph_SizeChanged(object sender, EventArgs e)
    {
        svGraph.MaximumWidthRequest = this.Width;
    }

    private void svMain_SizeChanged(object sender, EventArgs e)
    {
        svMain.MaximumWidthRequest = this.Width;
        svMain.MaximumHeightRequest = this.Height;
    }

    #region Zapisywanie

    private async void bSave_Clicked(object sender, EventArgs e)
    {
        var json = JsonConvert.SerializeObject(DataGraphs);
        bool complite = false;
        string result = await DisplayPromptAsync("Zapisywanie", "Podaj nazwę pliku", "Zapisz", "Anuluj", _name, -1, null, _name);
        if (!string.IsNullOrWhiteSpace(result))
        {
            bool exist = _fm.ExistFile(result);

            if (exist)
            {
                complite = await _fm.SaveNewData(result, json);
            }
            else
            {
                bool overwrite = await DisplayAlert("Zapisywanie", "Istnieje już plik o takiej nazwię. Czy chcesz go nadpisać?", "Tak", "Nie");
                if (overwrite)
                {
                    complite = await _fm.SaveNewData(result, json);
                }
            }
        }
        if (complite)
        {
            await DisplayAlert("Zapisywanie", "Zapisono nowy plik", "OK");
        }
        else
        {
            await DisplayAlert("Zapisywanie", "Nie udało się zapisać pliku", "OK");
        }
    }
    #endregion

}