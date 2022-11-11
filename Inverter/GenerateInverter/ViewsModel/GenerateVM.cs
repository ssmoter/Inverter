using Inverter.Data;
using Inverter.Display.Views;
using Inverter.GenerateInverter.Model;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Inverter.GenerateInverter.ViewsModel
{
    public class GenerateMV : INotifyPropertyChanged
    {
        private InverterParameters _InverterParameters;
        private FileManager _FileManager;

        private GenerateM _InverterM;
        public GenerateM InverterM
        {
            get { return _InverterM; }
            set
            {
                _InverterM = value;
                OnPropertyChanged(nameof(InverterM));
            }
        }

        public GenerateMV()
        {
            _InverterParameters = new();
            _InverterM = new();
            _InverterM.DefaultDataNotify = new();
            _InverterM.DataNotify = new();
            _FileManager = new();
            Message = new ObservableCollection<string>();
            Message.Clear();
            Initialization();
        }
        public ICommand InitialValues => new Command(() =>
        {
            AddMessage("Wczytanie ustawień początkowych");
            Initialization();
        });

        private void Initialization()
        {
            try
            {
                _InverterM.UzNotify = _InverterParameters.Uz;
                _InverterM.FoNotify = _InverterParameters.Fo;
                _InverterM.MaNotify = _InverterParameters.Ma;
                _InverterM.R_onNotify = _InverterParameters.R_on;
                _InverterM.R_offNotify = _InverterParameters.R_off;
                _InverterM.RoNotify = _InverterParameters.Ro;
                _InverterM.LoNotify = _InverterParameters.Lo;
                _InverterM.EoNotify = _InverterParameters.Eo;
                _InverterM.FioNotify = _InverterParameters.Fio;
                _InverterM.FiNotify = _InverterParameters.Fi;
                _InverterM.AlfaNotify = _InverterParameters.Alfa;
                _InverterM.T1Notify = _InverterParameters.T1;
                _InverterM.T2Notify = _InverterParameters.T2;
                _InverterM.T3Notify = _InverterParameters.T3;
                _InverterM.T4Notify = _InverterParameters.T4;
                _InverterM.T5Notify = _InverterParameters.T5;
                _InverterM.T6Notify = _InverterParameters.T6;
                _InverterM.DataNotify = new();
                foreach (var item in _InverterParameters.DefaultPrintTran)
                {
                    _InverterM.DefaultDataNotify.Add(item);
                }

                _InverterM.StringModelNotify = _InverterParameters.StringModel;
            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }

        }


        #region Nowy plik
        private ObservableCollection<string> _Message;
        public ObservableCollection<string> Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        private void AddMessage(string message)
        {
            Message.Insert(0, message);
            // Message.Add(message);
        }

        public ICommand CreatedNewFile => new Command(async () =>
        {
            try
            {
                bool isCreatedFile = await _FileManager.NewFile(InverterM.StringModelNotify);
                AddMessage("Tworzenie Pliku");
                if (isCreatedFile)
                {
                    AddMessage("Plik Został utworzony");

                    AddMessage("Uruchamianie Aplikacji");
                    Process.Start(@"F:\pspice\instal\PSpice\pspice.exe", _FileManager.FilePathData);
                    AddMessage("Aplikacja została uruchomiona");
                }
                LoadDataColor = Colors.Green;
            }
            catch (Exception ex)
            {
                Message.Add(ex.Message);
            }
        });
        #endregion

        #region WczytajDane

        private Color _loadDataColor = Colors.Grey;
        public Color LoadDataColor
        {
            get => _loadDataColor;
            set
            {
                _loadDataColor = value;
                OnPropertyChanged(nameof(LoadDataColor));
            }

        }
        public ICommand LoadData => new Command(async () =>
        {
            try
            {
                if (await Shell.Current.DisplayAlert("", "Czy chcesz kontynuować", "Tak", "Nie"))
                {
                    AddMessage("Wczytywanie nowego modelu");
                    ResponseModel response = new(_FileManager.FilePathData);

                    response.DataGraphs = InverterM.DefaultDataNotify.ToList();
                    if (InverterM.DataNotify.Any(x => !string.IsNullOrEmpty(x.DataName)))
                        response.DataGraphs.AddRange(InverterM.DataNotify);

                    Message = new ObservableCollection<string>();
                    await Shell.Current.GoToAsync($"../{nameof(DisplayV)}?",
                          new Dictionary<string, object>
                          {
                              [nameof(DataGraph)] = response.DataGraphs,
                          });
                }
            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }

        });

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}