using System.Reflection;
using System.Text;

namespace Inverter.Models
{
    public class ResponseModel
    {
        public string FileDataPath { get; set; }
        public string OutPutString { get; set; }
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
        public NamedColor UserColor { get; set; }
        public float Multiplier { get; set; } = 0.00f;
        public List<float> X { get; set; }
        public List<float> Y { get; set; }
        public bool Visible { get; set; } = false;
        public int LocationRow { get; set; } = 0;
        public int locationRowSpan { get; set; } = 1;
        public DataGraph()
        {
            UserColor = new NamedColor();
            X = new();
            Y = new();
        }
    }


    public class NamedColor
    {
        public string Name { get; private set; }
        public string FriendlyName { get; private set; }
        public Color Color { get; private set; }
        // Expose the Color fields as properties
        public float Red => Color.Red;
        public float Green => Color.Green;
        public float Blue => Color.Blue;
        public NamedColor()
        {
            Color = new Color();
        }
        public static IEnumerable<NamedColor> All { get; private set; }


        static NamedColor()
        {
            List<NamedColor> all = new List<NamedColor>();
            StringBuilder stringBuilder = new StringBuilder();

            // Loop through the public static fields of the Color structure.
            foreach (FieldInfo fieldInfo in typeof(Colors).GetRuntimeFields())
            {
                if (fieldInfo.IsPublic &&
                    fieldInfo.IsStatic &&
                    fieldInfo.FieldType == typeof(Color))
                {
                    // Convert the name to a friendly name.
                    string name = fieldInfo.Name;
                    stringBuilder.Clear();
                    int index = 0;

                    foreach (char ch in name)
                    {
                        if (index != 0 && Char.IsUpper(ch))
                        {
                            stringBuilder.Append(' ');
                        }
                        stringBuilder.Append(ch);
                        index++;
                    }

                    // Instantiate a NamedColor object.
                    NamedColor namedColor = new NamedColor
                    {
                        Name = name,
                        FriendlyName = stringBuilder.ToString(),
                        Color = (Color)fieldInfo.GetValue(null)
                    };

                    // Add it to the collection.
                    all.Add(namedColor);
                }
            }
            all.TrimExcess();
            All = all;
        }
    }
}
