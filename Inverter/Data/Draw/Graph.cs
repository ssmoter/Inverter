namespace Inverter.Data.Draw
{
    internal class Graph : BaseDrawable
    {
        public PathF point { get; set; }
        public Color Color { get; set; }
        public float MaxYValue { get; set; } = 1;
        public float MinYValue { get; set; } = 1;
        public float MaxYPosition { get; set; } = 1;
        public float MinYPositions { get; set; } = 1;
        public List<float> AxisX { get; set; }
        public bool AxisXWrite { get; set; }
        public string Name { get; set; }
        public int PositionName { get; set; } = 1;
        public bool GridIsVisible { get; set; }

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
                #region Wykresy
                canvas.SaveState();

                canvas.Translate(dirtyRect.Left + 40, dirtyRect.Top + MaxYPosition + 20);

                canvas.StrokeColor = Color;
                canvas.StrokeSize = 1.5f;
                canvas.DrawPath(point);
                canvas.RestoreState();
                #endregion

                #region Osie

                canvas.SaveState();
                canvas.Translate(dirtyRect.Left, dirtyRect.Top);
                //podpis wykresu
                canvas.FontColor = Color;
                canvas.DrawString(Name, 50 + (PositionName * 50), dirtyRect.Bottom - 10, HorizontalAlignment.Left);

                //wartości na osi Y
                canvas.DrawString(MaxYValue.ToString() + "-", 70, dirtyRect.Top + 20, HorizontalAlignment.Right);
                if (Math.Abs(MinYValue) > 0.001)
                    canvas.DrawString(MinYValue.ToString() + "-", 70, dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + 20, HorizontalAlignment.Right);


                canvas.StrokeColor = Colors.Black;
                canvas.FontColor = Colors.Black;

                canvas.DrawLine(dirtyRect.Left + 70, dirtyRect.Bottom, dirtyRect.Left + 70, dirtyRect.Top); //y
                canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 50, dirtyRect.Left + 10, dirtyRect.Bottom - 50); //x

                //wartosci na osi X
                if (AxisXWrite)
                {
                    if (MaxYValue > 0)
                    {
                        canvas.FontColor = Color;
                        canvas.DrawString("0-", 70, (dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + 20) - Math.Abs(MinYPositions), HorizontalAlignment.Right);
                        canvas.FontColor = Colors.Black;
                    }
                    canvas.FontColor = Color;
                    canvas.Translate(dirtyRect.Left + 70, dirtyRect.Top);
                    bool verticalX = false;
                    for (int i = 0; i < AxisX.Count; i++)
                    {
                        if (!verticalX)
                        {
                            canvas.DrawString(AxisX[i].ToString(), i * 40, dirtyRect.Bottom - 40, HorizontalAlignment.Left);
                            verticalX = !verticalX;
                        }
                        else if (verticalX)
                        {
                            canvas.DrawString(AxisX[i].ToString(), i * 40, dirtyRect.Bottom - 25, HorizontalAlignment.Left);
                            verticalX = !verticalX;
                        }
                    }
                }
                canvas.RestoreState();

                #endregion

                #region Siatka
                if (!GridIsVisible)
                {
                    IPattern pattern;
                    //Tworzenie paternu 10x10
                    using (PictureCanvas pc = new PictureCanvas(0, 0, 10, 10))
                    {
                        pc.StrokeColor = Color.FromRgb(230, 225, 225);
                        pc.DrawLine(0, 0, 0, 10);
                        pc.DrawLine(10, 0, 0, 0);
                        pattern = new PicturePattern(pc.Picture, 10, 10);
                    }

                    //wypełnienie obiektu danym paternem
                    PatternPaint pp = new PatternPaint
                    {
                        Pattern = pattern
                    };
                    canvas.SetFillPaint(pp, RectF.Zero);
                    canvas.FillRectangle(70, 0, dirtyRect.Width, dirtyRect.Height - 50);
                }
                #endregion
            }
            catch (Exception)
            {
                throw new Exception("Błąd przy rysowaniu Wykresów");
            }
        }
    }
}
