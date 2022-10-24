namespace Inverter.Models
{
    public class ResponseModel
    {
        public string FileDataPath { get; set; }
        public List<Data> Data { get; set; }
        public ResponseModel(string fileDataPath)
        {
            Data = new();
            FileDataPath = fileDataPath;
        }
    }

    public class Data
    {
        public string DataName { get; set; }
        public string UserDataName { get; set; }
        public Color UserColor { get; set; }
        public int Multiplier { get; set; } = 1;
        public List<float> X { get; set; }
        public List<float> Y { get; set; }
        public bool Default { get; set; } = false;

        public Data()
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
