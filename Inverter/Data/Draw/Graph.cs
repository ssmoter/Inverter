
using Inverter.Helpers;

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
        public List<float> AxisY { get; set; }
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
        private bool fourier = false;
        private FileManager _fm;
        public Graph(FileManager fileManager)
        {
            AxisX = new List<float>();
            AxisY = new List<float>();
            Color = new Color();
            point = new PathF();
            _fm = fileManager;
        }
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            try
            {
                if (Name.Contains(AppConst.Fourier))
                {
                    fourier = true;
                }

                int farFromUp = 20;

                #region Wykresy
                canvas.SaveState();
                if (!fourier)
                {
                    canvas.StrokeSize = StrokeSize;
                }


                canvas.StrokeColor = Color;

                //skala w osi X
                float scaleX = 1;
                scaleX = (dirtyRect.Width - 100) / (EndScopeIndex - StartScopeIndex);

                //skalowanie w osi Y
                float scaleY = 1;
                if (AutoScaleY && !fourier)
                {
                    float denominator = 1;
                    canvas.Translate(dirtyRect.Left, dirtyRect.Top);
                    canvas.Translate(dirtyRect.Left + 70, dirtyRect.Center.Y - farFromUp);

                    denominator = Math.Abs(MinYValue * 2f) + MaxYValue * 2f;

                    scaleY = (dirtyRect.Height - farFromUp - 80) / denominator;
                    canvas.Scale(1, scaleY);
                }
                else if (fourier)
                {
                    canvas.Translate(dirtyRect.Left + 70, dirtyRect.Bottom - 50);
                }
                else
                {
                    canvas.Translate(dirtyRect.Left + 70, dirtyRect.Center.Y - farFromUp);
                    //canvas.Translate(dirtyRect.Left + 70, dirtyRect.Top + MaxYPosition + farFromUp);
                }
                if (AutoScaleX)
                {
                    canvas.Scale(scaleX, 1);
                }

                if (StrokeSize == 0 && !fourier)
                {
                    canvas.StrokeSize = (scaleX / scaleY);
                    if (1 / (scaleX / scaleY) <= 0.5f)
                    {
                        //canvas.StrokeSize = 0.5f;
                    }
                }

                canvas.DrawPath(point);
                canvas.RestoreState();
                #endregion               
                canvas.Scale(1, 1);


                #region Osie
                canvas.Translate(dirtyRect.Left, dirtyRect.Top);

                canvas.SaveState();
                //podpis wykresu
                canvas.FontColor = Color;
                canvas.FontSize = FontSize;
                if (PositionName == 1)
                    canvas.DrawString(Name, 0, dirtyRect.Bottom - 3, HorizontalAlignment.Left);
                else
                    canvas.DrawString(Name, PositionName, dirtyRect.Bottom - 3, HorizontalAlignment.Left);


                //wartości na osi Y}
                var yValue = MaxYValue.ToString().Split(',');
                string visibleYValue = string.Empty;

                if (AutoScaleY && !fourier)
                {
                    if (yValue.Length > 1)
                        visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 2);
                    else
                        visibleYValue = yValue.FirstOrDefault();

                    canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Center.Y - (scaleY * MaxYValue) - farFromUp, HorizontalAlignment.Right);
                    if (Math.Abs(MinYValue) > 0.001)
                    {
                        yValue = MinYValue.ToString().Split(',');
                        if (yValue.Length > 1)
                        {
                            visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 2);
                        }
                        else
                        {
                            visibleYValue = yValue.FirstOrDefault();
                        }
                        canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Center.Y - (MinYValue * scaleY) - farFromUp, HorizontalAlignment.Right);
                    }
                    if (MaxYValue > 0)
                    {
                        canvas.FontColor = Color;
                        canvas.DrawString("0-", 70, dirtyRect.Center.Y - farFromUp, HorizontalAlignment.Right);
                        canvas.FontColor = Colors.Black;
                    }
                }
                else if (!fourier)
                {
                    if (yValue.Length > 1 && yValue.LastOrDefault().Length > 2)
                        visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 2);
                    else
                        visibleYValue = yValue.FirstOrDefault();

                    if (yValue.LastOrDefault().Length < 2)
                        visibleYValue += "," + yValue.LastOrDefault();

                    //  canvas.Translate(dirtyRect.Left + 70, dirtyRect.Center.Y - farFromUp);
                    // canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Top + farFromUp, HorizontalAlignment.Right);

                    canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Center.Y - farFromUp - MaxYPosition, HorizontalAlignment.Right);
                    if (Math.Abs(MinYValue) > 0.001)
                    {
                        yValue = MinYValue.ToString().Split(',');
                        if (yValue.Length > 1 && yValue.LastOrDefault().Length > 2)
                            visibleYValue = yValue.FirstOrDefault() + "," + yValue.LastOrDefault().Substring(0, 2);
                        else
                            visibleYValue = yValue.FirstOrDefault();

                        if (yValue.LastOrDefault().Length < 2)
                            visibleYValue += "," + yValue.LastOrDefault();
                        canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Center.Y - farFromUp - MinYPositions, HorizontalAlignment.Right);
                        // canvas.DrawString(visibleYValue + "-", 70, dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + farFromUp, HorizontalAlignment.Right);
                    }
                    if (MaxYValue > 0)
                    {
                        canvas.FontColor = Color;
                        canvas.DrawString("0-", 70, dirtyRect.Center.Y - farFromUp, HorizontalAlignment.Right);
                        // canvas.DrawString("0-", 70, (dirtyRect.Top + MaxYPosition + Math.Abs(MinYPositions) + farFromUp) - Math.Abs(MinYPositions), HorizontalAlignment.Right);
                        canvas.FontColor = Colors.Black;
                    }
                }
                else if (fourier)
                {
                    int secondLineI = 0;
                    bool secondLineB = true;
                    for (int i = 0; i < AxisY.Count; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }

                        if (AutoScaleX)
                        {
                            canvas.DrawString(AxisY[i].ToString(), (i + 0f) * scaleX + 75, dirtyRect.Top + farFromUp + secondLineI, HorizontalAlignment.Center);
                        }
                        else
                        {
                            canvas.DrawString(AxisY[i].ToString(), (i + 0f) + 75, dirtyRect.Top + farFromUp + secondLineI, HorizontalAlignment.Center);
                        }
                        if (secondLineB)
                        {
                            secondLineB = !secondLineB;
                            secondLineI = 20;
                        }
                        else
                        {
                            secondLineB = !secondLineB;
                            secondLineI = 0;
                        }
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
                    int lenghtN = 20;
                    if (AxisX.Count > 0)
                    {
                        lenghtN = AxisX.Count / 20;
                    }
                    for (int j = 0; ; j++)
                    {
                        if (lenghtN < 50)
                            lenghtN *= 2;

                        if (lenghtN > 100)
                            lenghtN /= 2;

                        if (lenghtN >= 50)
                            break;

                        if (j > 20)
                        {
                            break;
                        }
                    }
                    if (AutoScaleX)
                    {
                        if (scaleX < 1)
                            lenghtN += 0;
                        else
                            lenghtN /= (int)scaleX;
                    }

                    canvas.DrawString("|", nPosition - 2, dirtyRect.Bottom - 48, HorizontalAlignment.Right);
                    canvas.DrawString(AxisX[nIndicator].ToString(), nPosition, dirtyRect.Bottom - 35, HorizontalAlignment.Center);

                    int secondLineI = 0;
                    bool secondLineB = true;
                    if (fourier)
                    {
                        for (int i = 0; i < AxisX.Count; i++)
                        {
                            if (i == 0)
                            {
                                continue;
                            }

                            if (AutoScaleX)
                            {
                                canvas.DrawString("|", (i + 0f) * scaleX, dirtyRect.Bottom - 48, HorizontalAlignment.Center);
                                canvas.DrawString(AxisX[i].ToString(), (i + 0f) * scaleX, dirtyRect.Bottom - 25, HorizontalAlignment.Center);
                                canvas.DrawString(i.ToString(), (i + 0f) * scaleX, dirtyRect.Bottom - 5, HorizontalAlignment.Center);

                            }
                            else
                            {
                                canvas.DrawString("|", (i + 0f), dirtyRect.Bottom - 48, HorizontalAlignment.Center);
                                canvas.DrawString(AxisX[i].ToString(), i, dirtyRect.Bottom - 25, HorizontalAlignment.Center);
                                canvas.DrawString(i.ToString(), (i + 0f), dirtyRect.Bottom - 5, HorizontalAlignment.Center);
                            }

                        }
                    }
                    else
                    {
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
                                canvas.DrawString("|", (nPosition) * scaleX, dirtyRect.Bottom - 48 + secondLineI, HorizontalAlignment.Center);
                                canvas.DrawString(AxisX[nIndicator].ToString(), nPosition * scaleX, dirtyRect.Bottom - 25 + secondLineI, HorizontalAlignment.Center);
                            }
                            else
                            {
                                canvas.DrawString("|", nPosition, dirtyRect.Bottom - 48 + secondLineI, HorizontalAlignment.Center);
                                canvas.DrawString(AxisX[nIndicator].ToString(), nPosition, dirtyRect.Bottom - 25 + secondLineI, HorizontalAlignment.Center);
                            }
                            if (secondLineB && nIndicator > 2 * lenghtN)
                            {
                                secondLineB = !secondLineB;
                                secondLineI = 20;
                            }
                            else
                            {
                                secondLineB = !secondLineB;
                                secondLineI = 0;
                            }
                        }
                    }



                    canvas.RestoreState();

                }
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
                            pc.StrokeColor = Colors.Silver;
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
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                _fm.SaveLog(ex.ToString());
            }
        }
    }
}
