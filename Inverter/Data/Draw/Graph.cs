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
        public bool MultipledGraph { get; set; }
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
                int farFromUp = 10;
                #region Wykresy
                canvas.SaveState();

                canvas.Translate(dirtyRect.Left + 40, dirtyRect.Top + MaxYPosition + farFromUp);

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

                var yValue = MaxYValue.ToString().Split(',');
                string visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 3);
                canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Top + farFromUp, HorizontalAlignment.Right);
                if (Math.Abs(MinYValue) > 0.001)
                {
                    yValue = MinYValue.ToString().Split(',');
                    visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 3);
                    canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + farFromUp, HorizontalAlignment.Right);
                }

                canvas.StrokeColor = Colors.Black;
                canvas.FontColor = Colors.Black;

                if (AxisXWrite)
                {
                    //linie czarne
                    canvas.DrawLine(dirtyRect.Left + 70, dirtyRect.Bottom - 50, dirtyRect.Left + 70, dirtyRect.Top); //y
                    canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 50, dirtyRect.Left + 10, dirtyRect.Bottom - 50); //x

                    //linie biale
                    canvas.StrokeColor = Colors.White;
                    canvas.DrawLine(dirtyRect.Left + 70, dirtyRect.Bottom - 50, dirtyRect.Left + 70, dirtyRect.Top); //y
                    canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 50, dirtyRect.Left + 10, dirtyRect.Bottom - 50); //x
                }

                canvas.StrokeColor = Colors.Black;

                if (MaxYValue > 0)
                {
                    canvas.FontColor = Color;
                    canvas.DrawString("0-", 70, (dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + farFromUp) - Math.Abs(MinYPositions), HorizontalAlignment.Right);
                    canvas.FontColor = Colors.Black;
                }

                //wartosci na osi X
                if (AxisXWrite)
                {
                    canvas.FontColor = Color;
                    canvas.Translate(dirtyRect.Left + 75, dirtyRect.Top);

                    int nIndicator = 0;
                    int nPosition = 0;
                    int lenghtN = AxisX.Count / 20;
                    if (lenghtN < 30)
                        lenghtN += lenghtN;
                    if (lenghtN > 100)
                        lenghtN = 50;
                    canvas.DrawString("|", nPosition - 2, dirtyRect.Bottom - 48, HorizontalAlignment.Right);
                    canvas.DrawString(AxisX[nIndicator].ToString(), nPosition, dirtyRect.Bottom - 35, HorizontalAlignment.Center);

                    for (int i = 0; i < AxisX.Count; i++, nIndicator += lenghtN, nPosition += lenghtN)
                    {
                        if (!MultipledGraph)
                        {
                            if (nIndicator >= AxisX.Count)
                            {
                                nIndicator = 0;
                            }
                        }
                        if (MultipledGraph)
                        {
                            if (nIndicator >= AxisX.Count)
                            {
                                break;
                            }
                        }
                        if (i == 0)
                        {
                            continue;
                        }

                        canvas.DrawString("|", nPosition - 2, dirtyRect.Bottom - 48, HorizontalAlignment.Right);
                        canvas.DrawString(AxisX[nIndicator].ToString(), nPosition, dirtyRect.Bottom - 35, HorizontalAlignment.Center);
                    }
                }
                canvas.RestoreState();

                #endregion

                #region Siatka
                if (!GridIsVisible)
                {
                    if (AxisXWrite)
                    {


                        IPattern pattern;
                        //Tworzenie paternu 10x10
                        using (PictureCanvas pc = new PictureCanvas(0, 0, 10, 10))
                        {
                            pc.StrokeColor = Colors.White;
                            pc.StrokeSize = 0.5f;
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
                        //siatka w czarnym kolorze wyzej bialy
                        using (PictureCanvas pc = new PictureCanvas(0, 0, 10, 10))
                        {
                            pc.StrokeColor = Colors.Black;
                            pc.StrokeSize = 0.5f;
                            pc.DrawLine(0, 0, 0, 10);
                            pc.DrawLine(10, 0, 0, 0);
                            pattern = new PicturePattern(pc.Picture, 10, 10);
                        }
                        pp.Pattern = pattern; ;
                        canvas.SetFillPaint(pp, RectF.Zero);
                        canvas.FillRectangle(70, 0, dirtyRect.Width, dirtyRect.Height - 50);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception($"Błąd przy rysowaniu Wykresów {Environment.NewLine}{ex.Message}");
            }
        }
    }
}
