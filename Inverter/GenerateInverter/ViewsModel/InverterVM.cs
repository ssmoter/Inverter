using Inverter.GenerateInverter.Model;
using Inverter.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inverter.GenerateInverter.ViewsModel
{
    public class InverterMV : INotifyPropertyChanged
    {

        private InverterM _InverterM;
        public InverterM InverterM
        {
            get { return _InverterM; }
            set
            {
                _InverterM = value;
                OnPropertyChanged();
            }
        }

        public InverterMV()
        {
            Initialization();
        }
        private InverterParameters _InverterParameters;


        private void Initialization()
        {
            _InverterParameters = new();

            _InverterM = new()
            {
                UzNotify = _InverterParameters.Uz,
                FoNotify = _InverterParameters.Fo,
                MaNotify = _InverterParameters.Ma,
                R_onNotify = _InverterParameters.R_on,
                R_offNotify = _InverterParameters.R_off,
                RoNotify = _InverterParameters.Ro,
                LoNotify = _InverterParameters.Lo,
                EoNotify = _InverterParameters.Eo,
                FioNotify = _InverterParameters.Fio,
                FiNotify = _InverterParameters.Fi,
                AlfaNotify = _InverterParameters.Alfa,
                T1Notify = _InverterParameters.T1,
                T2Notify = _InverterParameters.T2,
                T3Notify = _InverterParameters.T3,
                T4Notify = _InverterParameters.T4,
                T5Notify = _InverterParameters.T5,
                T6Notify = _InverterParameters.T6,
                DefaultPrintTranNotify = _InverterParameters.DefaultPrintTran.ToList(),
            };

        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
