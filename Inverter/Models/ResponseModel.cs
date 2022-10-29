namespace Inverter.Models
{
    public class ResponseModel
    {
        public string FileDataPath { get; set; }
        public List<DataGraph> DataGraphs { get; set; }
        public ResponseModel()
        {
            DataGraphs = new();
        }
        public ResponseModel(string fileDataPath)
        {
            DataGraphs = new();
            FileDataPath = fileDataPath;
        }
    }

    public class DataGraph
    {
        public string DataName { get; set; }
        public string UserDataName { get; set; }
        public Color UserColor { get; set; }
        public float? Multiplier { get; set; } = 1.00f;
        public List<float> X { get; set; }
        public List<float> Y { get; set; }
        public bool Visible { get; set; } = false;

        public DataGraph()
        {
            SetColor();
            X = new();
            Y = new();
        }

        public void SetColor()
        {
            Random rnd = new Random();
            UserColor = Color.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
        }

    }
}
