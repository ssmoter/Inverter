using Inverter.Data;
using Inverter.Display.Views;
using Inverter.GenerateInverter.Model;
using Inverter.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
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
                OnPropertyChanged("InverterM");
            }
        }

        public GenerateMV()
        {
            _InverterParameters = new();
            _InverterM = new();
            _InverterM.DefaultDataNotify = new();
            _InverterM.DataNotify = new();
            _FileManager = new();
            Message = new List<string>();

            Initialization();
        }
        public ICommand InitialValues => new Command(() =>
        {
            Initialization();
        });

        private void Initialization()
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


        #region Nowy plik

        private int _ShowMessageInt = 0;
        private System.Timers.Timer _Timer;
        private void _CountTheTime(object source, ElapsedEventArgs e)
        {
            _ShowMessageInt++;

            if (_ShowMessageInt == 20)
            {
                _ShowMessageInt = 0;
                _Timer.Stop();
                _Timer.Dispose();
                _Timer = null;

                Message.RemoveRange(0, Message.Count);
                ShowMessage = false;
            }
        }

        private List<string> _Message;
        public List<string> Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                OnPropertyChanged("Message");
            }
        }
        private bool _ShowMessage = false;
        public bool ShowMessage
        {
            get => _ShowMessage;
            set
            {
                _ShowMessage = value;
                OnPropertyChanged("ShowMessage");
            }
        }


        public ICommand CreatedNewFile => new Command(async () =>
        {
            try
            {
                _ShowMessageInt = 0;
                ShowMessage = true;


                bool isCreatedFile = await _FileManager.NewFile(InverterM.StringModelNotify);

                if (isCreatedFile)
                {
                    Message.Add("Plik Został utworzony");
                    Process.Start(@"F:\pspice\instal\PSpice\pspice.exe", _FileManager.FilePathData);

                    Message.Add("Aplikacja została otwarta");
                }
                LoadDataColor=Colors.Green;
            }
            catch (Exception ex)
            {
                Message.Add(ex.Message);
                throw;
            }

            if (_Timer == null)
            {
                _Timer = new System.Timers.Timer(1000);
            }

            _Timer.Start();
            _Timer.Elapsed += new ElapsedEventHandler(_CountTheTime);
            _Timer.AutoReset = true;
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
                OnPropertyChanged("LoadDataColor");
            }

        }

        public ICommand LoadData => new Command(() =>
        {
            ResponseModel response = new(_FileManager.FilePathData);

            response.DataGraphs = InverterM.DefaultDataNotify.ToList();
            if (InverterM.DataNotify.Any(x => !string.IsNullOrEmpty(x.DataName)))
                response.DataGraphs.AddRange(InverterM.DataNotify);

            Shell.Current.GoToAsync($"../{nameof(DisplayV)}?",
                new Dictionary<string, object>
                {
                    [nameof(DataGraph)] = response.DataGraphs,
                });
        });

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}