namespace Inverter.Data.Draw
{
    internal class Graph : BaseDrawable
    {
        public PathF point { get; set; }
        public Color Color { get; set; }
        public float MaxY { get; set; } = 1;
        public List<float> AxisX { get; set; }
        public bool AxisXWrite { get; set; }
        public string Name { get; set; }
        public int PositionName { get; set; } = 1;

        public Graph()
        {
            AxisX = new List<float>();
            Color = new Color();
            point = new PathF();
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            try
            {
                canvas.SaveState();

                canvas.Translate(dirtyRect.Left + 40, dirtyRect.Center.Y);

                canvas.StrokeColor = Color;
                canvas.StrokeSize = 2;
                canvas.DrawPath(point);
                canvas.RestoreState();

                #region Osie

                canvas.SaveState();
                canvas.Translate(dirtyRect.Left, dirtyRect.Top);
                //podpis wykresu
                canvas.FontColor = Color;
                canvas.DrawString(Name, 10, dirtyRect.Top + (15 * PositionName), HorizontalAlignment.Left);

                canvas.StrokeColor = Colors.Black;

                canvas.DrawLine(dirtyRect.Left + 70, dirtyRect.Bottom, dirtyRect.Left + 70, dirtyRect.Top); //y
                canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 30, dirtyRect.Left + 10, dirtyRect.Bottom - 30); //x

                //wartość na osi Y
                var maxSizeY = (MaxY / 2).ToString().Split('.', ',');

                int maxYLength = (int)(dirtyRect.Height / int.Parse(maxSizeY[0]));

                int axisYValue = (int)MaxY;

                for (int i = 0; i < maxYLength; i++)
                {
                  //  canvas.DrawString(axisYValue.ToString(), 75, dirtyRect.Top + ((dirtyRect.Bottom / maxYLength) * i), HorizontalAlignment.Right);
                    axisYValue -= (int)(MaxY / maxYLength);
                }

                canvas.FontColor = Colors.Black;

                //wartosci na osi X
                if (AxisXWrite)
                {
                    canvas.Translate(dirtyRect.Left + 70, dirtyRect.Top);
                    bool verticalX = false;
                    for (int i = 0; i < AxisX.Count; i++)
                    {
                        if (!verticalX)
                        {
                            canvas.DrawString(AxisX[i].ToString(), i * 40, dirtyRect.Bottom - 20, HorizontalAlignment.Left);
                            verticalX = !verticalX;
                        }
                        else if (verticalX)
                        {
                            canvas.DrawString(AxisX[i].ToString(), i * 40, dirtyRect.Bottom - 5, HorizontalAlignment.Left);
                            verticalX = !verticalX;
                        }
                    }
                }
                canvas.RestoreState();

                #endregion
            }
            catch (Exception)
            {

            }
        }
    }
}
