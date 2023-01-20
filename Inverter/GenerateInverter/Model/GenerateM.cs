using Inverter.Helpers;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Inverter.GenerateInverter.Model
{
    public class GenerateM : INotifyPropertyChanged
    {
        #region Variables

        private string _UzNotify;
        public string UzNotify
        {
            get => _UzNotify;
            set
            {
                if (_UzNotify != value)
                {
                    _UzNotify = value;
                    OnPropertyChanged(nameof(UzNotify));
                }
            }
        }

        private string _FoNotify;
        public string FoNotify
        {
            get => _FoNotify;
            set
            {
                if (_FoNotify != value)
                {
                    _FoNotify = value;
                    OnPropertyChanged(nameof(FoNotify));
                }
            }
        }

        private string _MaNotify;
        public string MaNotify
        {
            get => _MaNotify;
            set
            {
                if (_MaNotify != value)
                {
                    _MaNotify = value;
                    OnPropertyChanged(nameof(MaNotify));
                }
            }
        }

        private string _R_onNotify;
        public string R_onNotify
        {
            get => _R_onNotify;
            set
            {
                if (_R_onNotify != value)
                {
                    _R_onNotify = value;
                    OnPropertyChanged(nameof(R_onNotify));
                }
            }
        }

        private string _R_offNotify;
        public string R_offNotify
        {
            get => _R_offNotify;
            set
            {
                if (_R_offNotify != value)
                {
                    _R_offNotify = value;
                    OnPropertyChanged(nameof(R_offNotify));
                }
            }
        }

        private string _RoNotify;
        public string RoNotify
        {
            get => _RoNotify;
            set
            {
                if (_RoNotify != value)
                {
                    _RoNotify = value;
                    OnPropertyChanged(nameof(RoNotify));
                }
            }
        }

        private string _LoNotify;
        public string LoNotify
        {
            get => _LoNotify;
            set
            {
                if (_LoNotify != value)
                {
                    _LoNotify = value;
                    OnPropertyChanged(nameof(LoNotify));
                }
            }
        }

        private string _EoNotify;
        public string EoNotify
        {
            get => _EoNotify;
            set
            {
                if (_EoNotify != value)
                {
                    _EoNotify = value;
                    OnPropertyChanged(nameof(EoNotify));
                }
            }
        }

        private string _FioNotify;
        public string FioNotify
        {
            get => _FioNotify;
            set
            {
                if (_FioNotify != value)
                {
                    _FioNotify = value;
                    OnPropertyChanged(nameof(FioNotify));
                }
            }
        }

        private string _FiNotify;
        public string FiNotify
        {
            get => _FiNotify;
            set
            {
                if (_FiNotify != value)
                {
                    _FiNotify = value;
                    OnPropertyChanged(nameof(FiNotify));
                }
            }
        }

        private int _AlfaNotify;
        public int AlfaNotify
        {
            get => _AlfaNotify;
            set
            {
                if (_AlfaNotify != value)
                {
                    _AlfaNotify = value;
                    OnPropertyChanged(nameof(AlfaNotify));
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
                    OnPropertyChanged(nameof(KNotify));
                }
            }
        }

        private KeyValuePair<string, int> _SelectedKNotify;
        public KeyValuePair<string, int> SelectedKNotify
        {
            get => _SelectedKNotify;
            set
            {
                _SelectedKNotify = value;
                OnPropertyChanged(nameof(SelectedKNotify));
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
                    OnPropertyChanged(nameof(T1Notify));
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
                    OnPropertyChanged(nameof(T2Notify));
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
                    OnPropertyChanged(nameof(T3Notify));
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
                    OnPropertyChanged(nameof(T4Notify));
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
                    OnPropertyChanged(nameof(T5Notify));
                }
            }
        }

        private int _NumberOfFftNotify;
        public int NumberOfFftNotify
        {
            get => _NumberOfFftNotify;
            set
            {
                if (_NumberOfFftNotify != value)
                {
                    _NumberOfFftNotify = value;
                    OnPropertyChanged(nameof(NumberOfFftNotify));
                }
            }
        }

        private string _StringModelNotify;
        public string StringModelNotify
        {
            get => _StringModelNotify;
            set
            {
                if (_StringModelNotify != value)
                {
                    _StringModelNotify = value;
                    OnPropertyChanged(nameof(StringModelNotify));
                }
            }
        }
        private ObservableCollection<Inverter.Models.DataGraph> _DefaultDataNotify;
        public ObservableCollection<Inverter.Models.DataGraph> DefaultDataNotify
        {
            get => _DefaultDataNotify;
            set
            {
                _DefaultDataNotify = value;
                OnPropertyChanged(nameof(DefaultDataNotify));
            }
        }

        private ObservableCollection<Inverter.Models.DataGraph> _DataNotify;
        public ObservableCollection<Inverter.Models.DataGraph> DataNotify
        {
            get => _DataNotify;
            set
            {
                _DataNotify = value;
                OnPropertyChanged(nameof(DataNotify));
            }
        }

        private Inverter.Models.DataGraph _SingleDataNotify;
        public Inverter.Models.DataGraph SingleDataNotify
        {
            get => _SingleDataNotify;
            set
            {
                this._SingleDataNotify = value;
                OnPropertyChanged(nameof(SingleDataNotify), false);
            }
        }
        private Inverter.Models.DataGraph _SelectedDataNotify;
        public Inverter.Models.DataGraph SelectedDataNotify
        {
            get => _SelectedDataNotify;
            set
            {
                this._SelectedDataNotify = value;
                OnPropertyChanged(nameof(SelectedDataNotify));
            }
        }

        #endregion

        public GenerateM()
        {
            _KNotify = new Dictionary<string, int>()
            {
                {"Fala Prostokątna",0},
                {"PWM",1 }
            };
            DefaultDataNotify = new();
            DataNotify = new();
            SingleDataNotify = new();
            _IsSelectedDataNotify = false;
        }


        private void RefreshStringModel()
        {
            InverterParameters _InverterParameters = new InverterParameters()
            {
                Uz = UzNotify,
                Fo = FoNotify,
                Ma = MaNotify,
                R_on = R_onNotify,
                R_off = R_offNotify,
                Ro = RoNotify,
                Lo = LoNotify,
                Eo = EoNotify,
                Fio = FioNotify,
                Fi = FiNotify,
                Alfa = AlfaNotify,
                K = _SelectedKNotify.Value,

                T1 = T1Notify,
                T2 = T2Notify,
                T3 = T3Notify,
                T4 = T4Notify,
                T5 = T5Notify,
                NumberOfFft= NumberOfFftNotify,
            };
            if (DataNotify != null)
            {
                for (int i = 0; i < DataNotify.Count; i++)
                {
                    if (DataNotify[i].DataName.Contains(AppConst.Fourier))
                    {
                        _InverterParameters.Four.Add(DataNotify[i].DataName.Replace(AppConst.Fourier, ""));
                    }
                    else
                    {
                        _InverterParameters.ExtraPrintTran = DataNotify.ToList();
                    }
                }

            }

            if (SelectedDataNotify != null)
            {
                IsSelectedDataNotify = true;
            }
            else
            {
                IsSelectedDataNotify = false;
            }

            _InverterParameters.CreateNewModel();
            StringModelNotify = _InverterParameters.StringModel;
        }

        public ICommand AddNewTranPrint => new Command(() =>
        {
            if (string.IsNullOrWhiteSpace(SingleDataNotify.DataName))
            {
                return;
            }

            DataNotify.Add(SingleDataNotify);
            SingleDataNotify = null;
            SingleDataNotify = new();
            RefreshStringModel();
        });
        public ICommand RemoveTranPrint => new Command(() =>
        {
            DataNotify.Remove(SelectedDataNotify);
            SelectedDataNotify = null;
            SelectedDataNotify = new();
            RefreshStringModel();
        });

        private bool _IsSelectedDataNotify;
        public bool IsSelectedDataNotify
        {
            get => _IsSelectedDataNotify;
            set
            {
                this._IsSelectedDataNotify = value;
                OnPropertyChanged(nameof(IsSelectedDataNotify), false);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null, bool refresh = true)
        {
            if (refresh)
            {
                RefreshStringModel();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }

}

