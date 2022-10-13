using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Inverter.GenerateInverter.Model
{
    public class InverterM : INotifyPropertyChanged
    {
        #region Variables

        private double _UzNotify;
        public double UzNotify
        {
            get => _UzNotify;
            set
            {
                if (_UzNotify != value)
                {
                    _UzNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _FoNotify;
        public double FoNotify
        {
            get => _FoNotify;
            set
            {
                if (_FoNotify != value)
                {
                    _FoNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _MaNotify;
        public double MaNotify
        {
            get => _MaNotify;
            set
            {
                if (_MaNotify != value)
                {
                    _MaNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _R_onNotify;
        public double R_onNotify
        {
            get => _R_onNotify;
            set
            {
                if (_R_onNotify != value)
                {
                    _R_onNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _R_offNotify;
        public double R_offNotify
        {
            get => _R_offNotify;
            set
            {
                if (_R_offNotify != value)
                {
                    _R_offNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _RoNotify;
        public double RoNotify
        {
            get => _RoNotify;
            set
            {
                if (_RoNotify != value)
                {
                    _RoNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _LoNotify;
        public double LoNotify
        {
            get => _LoNotify;
            set
            {
                if (_LoNotify != value)
                {
                    _LoNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _EoNotify;
        public double EoNotify
        {
            get => _EoNotify;
            set
            {
                if (_EoNotify != value)
                {
                    _EoNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _FioNotify;
        public double FioNotify
        {
            get => _FioNotify;
            set
            {
                if (_FioNotify != value)
                {
                    _FioNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _FiNotify;
        public double FiNotify
        {
            get => _FiNotify;
            set
            {
                if (_FiNotify != value)
                {
                    _FiNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _AlfaNotify;
        public double AlfaNotify
        {
            get => _AlfaNotify;
            set
            {
                if (_AlfaNotify != value)
                {
                    _AlfaNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private Dictionary<string, int> _KNotify;

        public Dictionary<string, int> KNotify
        {
            get => _KNotify;
            set
            {
                if (_KNotify != value)
                {
                    _KNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _T1Notify;
        public string T1Notify
        {
            get => _T1Notify;
            set
            {
                if (_T1Notify != value)
                {
                    _T1Notify = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _T2Notify;
        public string T2Notify
        {
            get => _T2Notify;
            set
            {
                if (_T2Notify != value)
                {
                    _T2Notify = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _T3Notify;
        public string T3Notify
        {
            get => _T3Notify;
            set
            {
                if (_T3Notify != value)
                {
                    _T3Notify = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _T4Notify;
        public string T4Notify
        {
            get => _T4Notify;
            set
            {
                if (_T4Notify != value)
                {
                    _T4Notify = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _T5Notify;
        public string T5Notify
        {
            get => _T5Notify;
            set
            {
                if (_T5Notify != value)
                {
                    _T5Notify = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _T6Notify;
        public string T6Notify
        {
            get => _T6Notify;
            set
            {
                if (_T6Notify != value)
                {
                    _T6Notify = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _DefaultPrintTranNotify;

        public List<string> DefaultPrintTranNotify
        {
            get => _DefaultPrintTranNotify;
            set
            {
                if (_DefaultPrintTranNotify != value)
                {
                    _DefaultPrintTranNotify = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public InverterM()
        {

            _KNotify = new Dictionary<string, int>()
            {
                {"Fala Prostokątna",0},
                {"PWM",1 }
            };
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

