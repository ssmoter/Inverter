using Inverter.Helpers;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;

namespace Inverter.Display.ViewsModel {
    [QueryProperty(nameof(ResponseModel), nameof(ResponseModel))]
    public class DisplayVM : INotifyPropertyChanged {
        public ResponseModel ResponseModel { get; set; }
        private int _fontSize = 20;
        public int FontSize {
            get {
                if (_fontSize <= 0) {
                    _fontSize = 20;
                }
                return _fontSize;
            }
            set {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        private ObservableCollection<DataGraph> _dataGraphs;
        public ObservableCollection<DataGraph> DataGraphs {
            get {
                return _dataGraphs;
            }
            set {
                _dataGraphs = value;
                OnPropertyChanged(nameof(DataGraphs));
            }
        }

        private string _maxMinValue;
        public string MaxMinValue {
            get { return _maxMinValue; }
            set {
                _maxMinValue = value;
                OnPropertyChanged(nameof(MaxMinValue));
            }
        }

        private DataGraph _dataGraphSelectedItem;
        public DataGraph DataGraphSelectedItem {
            get => _dataGraphSelectedItem;
            set {
                _dataGraphSelectedItem = value;
                DataGraphUpdateItem = DataGraphSelectedItem;

                if (_dataGraphSelectedItem != null) {
                    try {
                        _maxMinValue = "Max = " + _dataGraphSelectedItem.Max.ToString() + " " + _dataGraphSelectedItem.DataName.ToLower().FirstOrDefault() +
                           " Min = " + _dataGraphSelectedItem.Min.ToString() + " " + _dataGraphSelectedItem.DataName.ToLower().FirstOrDefault();
                        if (_dataGraphSelectedItem.DataName.Contains(AppConst.Fourier)) {
                            _maxMinValue = $"THD={GetTHD(_dataGraphSelectedItem)}" +
                                $" THD%={GetTHD(_dataGraphSelectedItem) * 100}%";
                        }

                    }
                    catch { }
                    OnPropertyChanged(nameof(MaxMinValue));
                }
                OnPropertyChanged(nameof(DataGraphSelectedItem));
            }
        }

        private float GetTHD(DataGraph DataGraphSelectedItem) {
            float counter = 0;
            for (int i = 2; i < DataGraphSelectedItem.Y.Count; i++) {
                counter += DataGraphSelectedItem.Y[i] * DataGraphSelectedItem.Y[i];
            }

            return (float)Math.Sqrt(counter) / DataGraphSelectedItem.Y[1];
        }
        private DataGraph _dataGraphUpdateItem;
        public DataGraph DataGraphUpdateItem {
            get => _dataGraphUpdateItem;
            set {
                _dataGraphUpdateItem = value;
                OnPropertyChanged(nameof(DataGraphUpdateItem));
            }
        }

        public DisplayVM() {
            Initialization();
        }
        public DisplayVM(ResponseModel responseModel) {
            ResponseModel = responseModel;
            Initialization();
        }

        private void Initialization() {
            SetUpdateItem();

            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            NameSimulationbutton = "Uruchom";
            symulationRunning = false;

            FontSize = Config.FontSize;

        }
        #region Symulacja

        System.Timers.Timer _timer;
        public void TimerEvent(object source, ElapsedEventArgs e) {
            if (SActualCurrentIndex >= SCurrentMaxIndex) {
                SActualCurrentIndex = 0;
            }
            SActualCurrentIndex++;
        }
        private int _sActualCurrentndex = 0;
        public int SActualCurrentIndex {
            get => _sActualCurrentndex;
            set {
                _sActualCurrentndex = value;
                OnPropertyChanged(nameof(SActualCurrentIndex));
            }
        }
        private int _sActualMaxIndex = 1;
        public int SCurrentMaxIndex {
            get => _sActualMaxIndex;
            set {
                _sActualMaxIndex = value;
                OnPropertyChanged(nameof(SCurrentMaxIndex));
            }
        }
        private string _nameSimulationbutton;
        public string NameSimulationbutton {
            get => _nameSimulationbutton;
            set {
                _nameSimulationbutton = value;
                OnPropertyChanged(nameof(NameSimulationbutton));
            }
        }
        private bool symulationRunning;
        public ICommand StartSymulation => new Command(() => {
            if (!symulationRunning) {
                symulationRunning = !symulationRunning;
                // _timer.Start();
                NameSimulationbutton = "Zatrzymaj";
            }
            else if (symulationRunning) {
                symulationRunning = !symulationRunning;
                // _timer.Stop();
                NameSimulationbutton = "Uruchom";
            }
        });


        #endregion

        private void SetUpdateItem() {
            DataGraphUpdateItem = new();
            DataGraphUpdateItem.UserColor = new();
            OnPropertyChanged(nameof(DataGraphUpdateItem));
        }

        public ICommand UpdateRowCommand => new Command(() => {
            //var n = -1;
            //n = DataGraphs.IndexOf(DataGraphSelectedItem);
            //if (n < 0)
            //{
            //    return;
            //}

            //try
            //{
            //    if (DataGraphSelectedItem.locationRowSpan > 0)
            //    {
            //        DataGraphs[n].locationRowSpan = DataGraphSelectedItem.locationRowSpan;
            //    }
            //    if (DataGraphSelectedItem.LocationRow >= 0)
            //    {
            //        DataGraphs[n].LocationRow = DataGraphSelectedItem.LocationRow;
            //    }
            //    DataGraphs[n].UserDataName = DataGraphSelectedItem.UserDataName;
            //    DataGraphs[n].UserColor = DataGraphSelectedItem.UserColor;
            //    DataGraphs[n].Visible = DataGraphSelectedItem.Visible;
            //    DataGraphs[n].Multiplier = DataGraphSelectedItem.Multiplier;
            //   // DataGraphs = new ObservableCollection<DataGraph>(DataGraphs);
            //    OnPropertyChanged(nameof(DataGraphs));
            //}
            //catch
            //{ }
        });
        public ICommand RefreshListCommand => new Command(() => {
            DataGraphs = new ObservableCollection<DataGraph>(DataGraphs);
            OnPropertyChanged(nameof(DataGraphs));
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
