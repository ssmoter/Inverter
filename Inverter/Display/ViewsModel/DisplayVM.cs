using Inverter.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Inverter.Display.ViewsModel
{
    [QueryProperty(nameof(DataGraphs), nameof(DataGraph))]
    public class DisplayVM : INotifyPropertyChanged
    {
        private List<DataGraph> _dataGraphs;
        public List<DataGraph> DataGraphs
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

        private string _maxValue;
        public string MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
            }
        }
        private string _minValue;
        public string MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                OnPropertyChanged(nameof(MinValue));
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
                    _maxValue = _dataGraphSelectedItem.Y.Max().ToString() + " " + _dataGraphSelectedItem.DataName.ToLower().FirstOrDefault();
                    _minValue = _dataGraphSelectedItem.Y.Min().ToString() + " " + _dataGraphSelectedItem.DataName.ToLower().FirstOrDefault();
                    OnPropertyChanged(nameof(MaxValue));
                    OnPropertyChanged(nameof(MinValue));
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

        private void Initialization()
        {
            SetUpdateItem();
        }

        private void SetUpdateItem()
        {
            DataGraphUpdateItem = new();
            DataGraphUpdateItem.UserColor = null;
            DataGraphUpdateItem.Multiplier = 1;
            DataGraphUpdateItem.LocationRow = 0;
            DataGraphUpdateItem.locationRowSpan = 1;
            OnPropertyChanged("DataGraphUpdateItem");
        }

        public ICommand UpdateRowCommand => new Command(() =>
        {
            var update = DataGraphUpdateItem;
            int n = DataGraphs.IndexOf(DataGraphSelectedItem);
            if (n == -1)
            {
                return;
            }

            var data = DataGraphs[n];

            if (update.UserDataName != DataGraphs[n].UserDataName && !string.IsNullOrEmpty(update.UserDataName))
                data.UserDataName = update.UserDataName;

            if (update.Multiplier != DataGraphs[n].Multiplier)
                data.Multiplier = update.Multiplier;

            if (update.UserColor != DataGraphs[n].UserColor && update.UserColor != null)
                data.UserColor = update.UserColor;

            if (update.Visible != DataGraphs[n].Visible)
                data.Visible = update.Visible;


            if (update.LocationRow != DataGraphs[n].LocationRow && update.LocationRow >= 0)
                data.LocationRow = update.LocationRow;

            if (update.locationRowSpan != DataGraphs[n].locationRowSpan && update.locationRowSpan >= 0)
                data.locationRowSpan = update.locationRowSpan;


            DataGraphs.RemoveAt(n);
            DataGraphs.Insert(n, data);
            OnPropertyChanged(nameof(DataGraphs));
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
