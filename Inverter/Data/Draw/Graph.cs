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
        public bool AutoScaleY { get; set; }
        public bool AutoScaleX { get; set; }
        public float FontSize { get; set; }
        public float StrokeSize { get; set; } = 1f;
        public int StartScopeIndex { get; set; } = 0;
        public int EndScopeIndex { get; set; } = 0;

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
                canvas.StrokeSize = StrokeSize;


                canvas.StrokeColor = Color;

                //skala w osi X
                float scaleX = 1;
                scaleX = (dirtyRect.Width - 100) / (EndScopeIndex - StartScopeIndex);


                //skalowanie w osi Y
                float scaleY = 1;
                if (AutoScaleY)
                {
                    float denominator = 1;
                    if (Math.Abs(MinYValue) < Math.Abs(MaxYValue) * 0.1)
                    {
                        denominator = Math.Abs(MaxYValue) * 2;
                        canvas.Translate(dirtyRect.Left + 70, dirtyRect.Center.Y - farFromUp);
                    }
                    else if (Math.Abs(MaxYValue) < Math.Abs(MinYValue) * 0.1)
                    {
                        denominator = Math.Abs(MinYValue) * 2;
                        canvas.Translate(dirtyRect.Left + 70, dirtyRect.Top + farFromUp);
                    }
                    else
                    {
                        denominator = Math.Abs(MinYValue) + Math.Abs(MaxYValue);
                        canvas.Translate(dirtyRect.Left + 70, dirtyRect.Center.Y - farFromUp);
                    }

                    scaleY = (dirtyRect.Height - farFromUp - 80) / denominator;
                    canvas.Scale(1, scaleY);
                }
                else
                {
                    canvas.Translate(dirtyRect.Left + 70, dirtyRect.Top + MaxYPosition + farFromUp);
                }
                if (AutoScaleX)
                {
                    canvas.Scale(scaleX, 1);
                }

                if (StrokeSize == 0)
                {
                    canvas.StrokeSize = 1 / scaleX;
                    if (1 / scaleX <= 0.1f)
                    {
                        canvas.StrokeSize = 0.1f;
                    }
                }

                canvas.DrawPath(point);
                canvas.RestoreState();
                #endregion               
                canvas.Scale(1, 1);

                #region Osie

                canvas.SaveState();
                canvas.Translate(dirtyRect.Left, dirtyRect.Top);
                //podpis wykresu
                canvas.FontColor = Color;
                canvas.FontSize = FontSize;
                canvas.DrawString(Name, 50 + (PositionName * (Name.Length + FontSize + 25)), dirtyRect.Bottom - 10, HorizontalAlignment.Left);

                //wartości na osi Y
                //poprawic

                var yValue = MaxYValue.ToString().Split(',');
                string visibleYValue = string.Empty;

                if (AutoScaleY)
                {
                    if (yValue.Length > 1)
                        visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 3);
                    else
                        visibleYValue = yValue.FirstOrDefault();

                    canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Center.Y - (scaleY * MaxYValue) - farFromUp, HorizontalAlignment.Right);
                    if (Math.Abs(MinYValue) > 0.001)
                    {
                        yValue = MinYValue.ToString().Split(',');
                        visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 3);
                        canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Center.Y - (MinYValue * scaleY) - farFromUp, HorizontalAlignment.Right);
                    }
                    if (MaxYValue > 0)
                    {
                        canvas.FontColor = Color;
                        canvas.DrawString("0-", 70, dirtyRect.Center.Y - farFromUp, HorizontalAlignment.Right);
                        canvas.FontColor = Colors.Black;
                    }
                }
                else
                {
                    if (yValue.Length > 1)
                        visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 3);
                    else
                        visibleYValue = yValue.FirstOrDefault();

                    canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Top + farFromUp, HorizontalAlignment.Right);
                    if (Math.Abs(MinYValue) > 0.001)
                    {
                        yValue = MinYValue.ToString().Split(',');
                        visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 3);
                        canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + farFromUp, HorizontalAlignment.Right);
                    }
                    if (MaxYValue > 0)
                    {
                        canvas.FontColor = Color;
                        canvas.DrawString("0-", 70, (dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + farFromUp) - Math.Abs(MinYPositions), HorizontalAlignment.Right);
                        canvas.FontColor = Colors.Black;
                    }
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

                //wartosci na osi X
                if (AxisXWrite)
                {
                    canvas.FontColor = Color;
                    canvas.Translate(dirtyRect.Left + 75, dirtyRect.Top);

                    int nIndicator = 0;
                    int nPosition = 0;
                    int lenghtN = AxisX.Count / 20;

                    for (int j = 0; ; j++)
                    {
                        if (lenghtN < 50)
                            lenghtN *= 2;

                        if (lenghtN > 100)
                            lenghtN /= 2;

                        if (lenghtN > 50)
                            break;
                    }
                    if (AutoScaleX)
                        lenghtN /= (int)scaleX;

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

                        if (AutoScaleX)
                        {
                            canvas.DrawString("|", (nPosition - 2) * scaleX, dirtyRect.Bottom - 48, HorizontalAlignment.Right);
                            canvas.DrawString(AxisX[nIndicator].ToString(), nPosition * scaleX, dirtyRect.Bottom - 25, HorizontalAlignment.Center);
                        }
                        else if (false)
                        {

                        }
                        else
                        {
                            canvas.DrawString("|", nPosition - 2, dirtyRect.Bottom - 48, HorizontalAlignment.Right);
                            canvas.DrawString(AxisX[nIndicator].ToString(), nPosition, dirtyRect.Bottom - 25, HorizontalAlignment.Center);
                        }
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
