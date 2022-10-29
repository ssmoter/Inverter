using Inverter.Data;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Inverter.Display.ViewsModel
{
    public class DisplayVM : INotifyPropertyChanged
    {
        private FileManager _FileManager;

        private ObservableCollection<DataGraph> _dataGraphs;
        public ObservableCollection<DataGraph> DataGraphs
        {
            get => _dataGraphs;
            set
            {
                _dataGraphs = value;
                OnPropertyChanged("DataGraphs");
            }
        }

        private DataGraph _dataGraphSelectedItem;
        public DataGraph DataGraphSelectedItem
        {
            get => _dataGraphSelectedItem;
            set
            {
                _dataGraphSelectedItem = value;
                OnPropertyChanged("DataGraphSelectedItem");
            }
        }

        private DataGraph _dataGraphUpdateItem;
        public DataGraph DataGraphUpdateItem
        {
            get => _dataGraphUpdateItem;
            set
            {
                _dataGraphUpdateItem = value;
                OnPropertyChanged("DataGraphSelectedItem");
            }
        }




        public DisplayVM()
        {
            Initialization();
        }

        private void Initialization()
        {
            _FileManager = new();
            ResponseModel responseModel = new();
            InverterParameters ip = new();
            responseModel.DataGraphs = ip.DefaultPrintTran;
            responseModel = _FileManager.OpenFile().Result.Mapping(responseModel);
            DataGraphs = new();
            foreach (var item in responseModel.DataGraphs)
            {
                DataGraphs.Add(item);
            }

            DataGraphUpdateItem = new();
            DataGraphUpdateItem.UserColor = null;
            DataGraphUpdateItem.Multiplier = null;
        }

        public ICommand UpdateRowCommand => new Command(() =>
        {
            var update = DataGraphUpdateItem;
            int n = DataGraphs.IndexOf(DataGraphSelectedItem);
            if (n == -1)
            {
                return;
            }
            if (!string.IsNullOrEmpty(update.DataName))
            {
                return;
            }

            var data = DataGraphs[n];

            if (update.UserDataName != DataGraphs[n].UserDataName && !string.IsNullOrEmpty(update.UserDataName))
                data.UserDataName = update.UserDataName;

            if (update.Multiplier != DataGraphs[n].Multiplier && update.Multiplier != null)
                data.Multiplier = update.Multiplier;

            if (update.UserColor != DataGraphs[n].UserColor && update.UserColor != null)
                data.UserColor = update.UserColor;

            if (update.Visible != DataGraphs[n].Visible)
                data.Visible = update.Visible;

            DataGraphs.RemoveAt(n);
            DataGraphs.Insert(n, data);


            DataGraphUpdateItem = new();
            DataGraphUpdateItem.Multiplier = null;
            DataGraphUpdateItem.UserColor = null;
            OnPropertyChanged("DataGraphUpdateItem");
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
