using Inverter.Display.Model;
using Inverter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Inverter.Display.ViewsModel {
    public class PopUpListVM : INotifyPropertyChanged, IDisposable {

        public PopUpListVM() {
            DataGraphs = new ObservableCollection<DataGraph>();
            NewEntryGraph = new ObservableCollection<string>();
            NewEntryGraph.Add(string.Empty);
        }
        public PopUpListVM(ObservableCollection<DataGraph> dataGraphs) {
            DataGraphs = dataGraphs;
            NewEntryGraph = new ObservableCollection<string>();
            NewEntryGraph.Add(string.Empty);
        }

        #region Parameter

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

        private ObservableCollection<string> _newEntryGraph;
        public ObservableCollection<string> NewEntryGraph {
            get {
                return _newEntryGraph;
            }
            set {
                if (value != null)
                    _newEntryGraph = value;
                OnPropertyChanged(nameof(NewEntryGraph));
            }
        }

        private DataGraph _dataGraphSelectedItem;
        public DataGraph DataGraphSelectedItem {
            get => _dataGraphSelectedItem;
            set {
                _dataGraphSelectedItem = value;
                OnPropertyChanged(nameof(DataGraphSelectedItem));
            }
        }
        private string _eNewParameter;
        public string ENewParameter {
            get => _eNewParameter;
            set {
                _eNewParameter = value;
                OnPropertyChanged(nameof(ENewParameter));
            }
        }
        #endregion

        #region Command

        public ICommand DeleteCommand => new Command((parameter) => {
            if (parameter is DataGraph)
                DataGraphSelectedItem = (DataGraph)parameter;
            if (DataGraphSelectedItem.CanDelete) {
                DataGraphs.Remove(DataGraphSelectedItem);
            }
        });
        public ICommand DoubleTapCommand => new Command((parameter) => {
            if (parameter is DataGraph)
                DataGraphSelectedItem = (DataGraph)parameter;

            AddGraphItem(DataGraphSelectedItem.DataName);

        });
        public ICommand DragCommand => new Command((parameter) => {
            if (parameter is DataGraph)
                DataGraphSelectedItem = (DataGraph)parameter;
        });
        public ICommand DropCommand => new Command(() => {
            AddGraphItem(DataGraphSelectedItem.DataName);
        });

        public ICommand ButtonCommand => new Command((parameter) => {
            if (parameter is not string)
                return;

            AddGraphItem((string)parameter);
        });

        public ICommand AddCommand => new Command(() => {

            if (NewEntryGraph == null) {
                return;
            }
            else if (NewEntryGraph.Count == 0) {
                return;
            }
            else if (!DataGraphs.Any(x => x.DataName == NewEntryGraph[0])) {
                return;
            }

            DataGraphs.Insert(0, AddDataGraph());
            NewEntryGraph.Clear();
            NewEntryGraph.Add(string.Empty);
        });
        public ICommand RemoveNewCommand => new Command((parameter) => {
            if (parameter is not string)
                return;
            NewEntryGraph.Remove((string)parameter);
            if (NewEntryGraph.Count == 0) {
                NewEntryGraph.Add(string.Empty);
            }
        });
        public ICommand EntryTextChangedCommand => new Command((parameter) => {
            if (parameter is not string)
                return;
            AddGraphItem((string)parameter);
            ENewParameter = string.Empty;
        });
        #endregion

        #region Methods
        private void AddGraphItem(string parameter) {
            if (NewEntryGraph.Count < 1) {
                return;
            }
            if (NewEntryGraph[0] == string.Empty) {
                NewEntryGraph.Clear();
            }
            NewEntryGraph.Add(parameter);
        }
        private DataGraph AddDataGraph() {
            try {
                var g = DataGraphs.FirstOrDefault(x => x.DataName == NewEntryGraph.FirstOrDefault());
                var graph = new DataGraph();
                for (int i = 0; i < g.Y.Count; i++) {
                    graph.X.Add(g.X[i]);
                    graph.Y.Add(g.Y[i]);
                }

                List<int> signList = new List<int>();
                for (int i = 0; i < NewEntryGraph.Count; i++) {
                    if (PopUpList.SignList.Any(x => x == NewEntryGraph[i].ToCharArray()[0])) {
                        signList.Add(i);
                    }
                }


                graph.UserColor = NamedColor.All.ToArray()[new Random().NextInt64(0, NamedColor.All.Count())];
                graph.CanDelete = true;
                graph.DataName = GetName(NewEntryGraph);

                float y2 = 0;
                int n = -1;
                for (int i = 1; i < NewEntryGraph.Count - 1; i++) {
                    if (signList.Any(x => x == i)) {
                        n++;
                        continue;
                    }
                    for (int j = 0; j < DataGraphs.Last().Y.Count; j++) {

                        if (DataGraphs.Any(x => x.DataName == NewEntryGraph[i])) {
                            y2 = DataGraphs.FirstOrDefault(x => x.DataName == NewEntryGraph[i]).Y[j];
                        }
                        else {
                            if (!float.TryParse(NewEntryGraph[i], out y2)) {
                                graph.DataName = "Error";
                                graph.UserDataName = "Usuń zmienną";
                                return graph;
                            }
                        }
                        graph.Y[j] = AddNewY(NewEntryGraph[signList[n]]
                            , graph.Y[j]
                            , y2);
                    }
                }
                graph.SetMaxMin();
                return graph;
            }
            catch (Exception) {
                throw;
            }
        }

        private string GetName(ObservableCollection<string> names) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < names.Count; i++) {
                sb.Append(names[i]);
            }
            return sb.ToString();
        }
        #region NewGraphData

        private float AddNewY(string name, float y, float y2) {
            if (name.ToCharArray()[0] == PopUpList.Add) {
                y = Add(y, y2);
            }
            else if (name.ToCharArray()[0] == PopUpList.Sub) {
                y = Sub(y, y2);
            }
            else if (name.ToCharArray()[0] == PopUpList.Multi) {
                y = Multi(y, y2);
            }
            else if (name.ToCharArray()[0] == PopUpList.Div) {
                y = Div(y, y2);
            }
            return y;
        }
        private float Add(params float[] numbers) {

            float result = numbers[0];
            for (int i = 1; i < numbers.Length; i++) {
                result += numbers[i];
            }
            return result;
        }
        private float Sub(params float[] numbers) {

            float result = numbers[0];
            for (int i = 1; i < numbers.Length; i++) {
                result -= numbers[i];
            }
            return result;
        }
        private float Multi(params float[] numbers) {

            float result = numbers[0];
            if (result == 0)
                result = 1;

            for (int i = 1; i < numbers.Length; i++) {
                result *= numbers[i];
            }
            return result;
        }
        private float Div(params float[] numbers) {

            float result = numbers[0];
            if (result == 0)
                result = 1;

            for (int i = 1; i < numbers.Length; i++) {
                result /= numbers[i];
            }
            return result;
        }
        #endregion

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose() {
            _dataGraphs = null;
            _dataGraphSelectedItem = null;
            _eNewParameter = null;
            _newEntryGraph = null;
        }
    }
}
