using Inverter.Helpers;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;

namespace Inverter.Display.ViewsModel
{
    //[QueryProperty(nameof(DataGraphs), nameof(DataGraph))]
    [QueryProperty(nameof(ResponseModel), nameof(ResponseModel))]
    public class DisplayVM : INotifyPropertyChanged
    {
        public ResponseModel ResponseModel { get; set; }
        private int _fontSize = 14;
        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
        private ObservableCollection<DataGraph> _dataGraphs;
        public ObservableCollection<DataGraph> DataGraphs
        {
            get
            {
                return _dataGraphs;
            }
            set
            {
                _dataGraphs = value;
                OnPropertyChanged(nameof(DataGraphs));
            }
        }

        private string _maxMinValue;
        public string MaxMinValue
        {
            get { return _maxMinValue; }
            set
            {
                _maxMinValue = value;
                OnPropertyChanged(nameof(MaxMinValue));
            }
        }

        private DataGraph _dataGraphSelectedItem;
        public DataGraph DataGraphSelectedItem
        {
            get => _dataGraphSelectedItem;
            set
            {
                _dataGraphSelectedItem = value;
                DataGraphUpdateItem = DataGraphSelectedItem;

                if (_dataGraphSelectedItem != null)
                {
                    try
                    {
                        _maxMinValue = "Maksymalna = " + _dataGraphSelectedItem.Max.ToString() + " " + _dataGraphSelectedItem.DataName.ToLower().FirstOrDefault() +
                           Environment.NewLine + "Minimalna = " + _dataGraphSelectedItem.Min.ToString() + " " + _dataGraphSelectedItem.DataName.ToLower().FirstOrDefault();
                    }
                    catch
                    { }
                    OnPropertyChanged(nameof(MaxMinValue));
                }
                OnPropertyChanged(nameof(DataGraphSelectedItem));
            }
        }
        private DataGraph _dataGraphUpdateItem;
        public DataGraph DataGraphUpdateItem
        {
            get => _dataGraphUpdateItem;
            set
            {
                _dataGraphUpdateItem = value;
                OnPropertyChanged(nameof(DataGraphUpdateItem));
            }
        }

        public DisplayVM()
        {
            Initialization();
        }
        public DisplayVM(ResponseModel responseModel)
        {
            ResponseModel = responseModel;
            Initialization();
        }

        private void Initialization()
        {
            SetUpdateItem();

            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            NameSimulationbutton = "Uruchom";
            symulationRunning = false;

            FontSize = Config.FontSize;

        }
        #region Symulacja

        System.Timers.Timer _timer;
        public void TimerEvent(object source, ElapsedEventArgs e)
        {
            if (SActualCurrentIndex >= SCurrentMaxIndex)
            {
                SActualCurrentIndex = 0;
            }
            SActualCurrentIndex++;
        }
        private int _sActualCurrentndex = 0;
        public int SActualCurrentIndex
        {
            get => _sActualCurrentndex;
            set
            {
                _sActualCurrentndex = value;
                OnPropertyChanged(nameof(SActualCurrentIndex));
            }
        }
        private int _sActualMaxIndex = 1;
        public int SCurrentMaxIndex
        {
            get => _sActualMaxIndex;
            set
            {
                _sActualMaxIndex = value;
                OnPropertyChanged(nameof(SCurrentMaxIndex));
            }
        }
        private string _nameSimulationbutton;
        public string NameSimulationbutton
        {
            get => _nameSimulationbutton;
            set
            {
                _nameSimulationbutton = value;
                OnPropertyChanged(nameof(NameSimulationbutton));
            }
        }
        private bool symulationRunning;
        public ICommand StartSymulation => new Command(() =>
        {
            if (!symulationRunning)
            {
                symulationRunning = !symulationRunning;
                _timer.Start();
                NameSimulationbutton = "Zatrzymaj";
            }
            else if (symulationRunning)
            {
                symulationRunning = !symulationRunning;
                _timer.Stop();
                NameSimulationbutton = "Uruchom";
            }
        });


        #endregion

        private void SetUpdateItem()
        {
            DataGraphUpdateItem = new();
            DataGraphUpdateItem.UserColor = new();
            OnPropertyChanged("DataGraphUpdateItem");
        }

        public ICommand UpdateRowCommand => new Command(() =>
        {
            var n = -1;
            n = DataGraphs.IndexOf(DataGraphSelectedItem);
            if (n < 0)
            {
                return;
            }
            List<DataGraph> newData = DataGraphs.ToList();
            try
            {
                if (DataGraphSelectedItem.locationRowSpan < 0)
                {
                    DataGraphSelectedItem.locationRowSpan = 1;
                }
                if (DataGraphSelectedItem.LocationRow <= 0)
                {
                    DataGraphSelectedItem.LocationRow = 0;
                }
                newData.Insert(n, DataGraphSelectedItem);
                newData.RemoveAt(n + 1);
            }
            catch
            {}
            DataGraphs = new ObservableCollection<DataGraph>(newData);
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
