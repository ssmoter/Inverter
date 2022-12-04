using Font = Microsoft.Maui.Graphics.Font;

namespace Inverter.Data.Draw.Schema
{
    internal class InverterSchema : BaseDrawable
    {
        public float MaxYValue { get; set; } = 400;
        public float MinYValue { get; set; } = 200;

        private int Scala = 2;
        private int StrokeSize = 5;
        public int Index { get; set; }
        float stillOpen = 0.01f;
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            float scaleX = 1, scaleY = 1;

             scaleX = dirtyRect.Width / 650;
             scaleY = dirtyRect.Height / 350;

            canvas.Scale(scaleX, scaleY);
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 50);
            
            canvas.StrokeSize = StrokeSize;
            ReturnColor(Index, canvas);

            SourceLine(canvas, dirtyRect);

            FirstLine(canvas, dirtyRect);

            canvas.Translate(170, 0);
            SecondtLine(canvas, dirtyRect);
            canvas.Translate(170, 0);
            ThirdLine(canvas, dirtyRect);

            canvas.Translate(-180, 260);
            Odbline(canvas, dirtyRect);
            canvas.RestoreState();
        }

        private Color ReturnColor(float current, ICanvas canvas)
        {
            Color c = new Color();

            switch (current)
            {
                case > 200:
                    c = Color.FromRgb(255, 0, 0);
                    if (current < (MaxYValue / 2))
                        c = Color.FromRgb(128, 0, 0);

                    break;
                case < 200:
                    c = Color.FromRgb(0, 0, 255);
                    if (current < (MinYValue / 2))
                        c = Color.FromRgb(0, 0, 128);

                    break;
                default:
                    c = Colors.Black;
                    break;
            }
            canvas.StrokeColor = c;
            canvas.FillColor = c;
            canvas.FontColor = c;
            return c;
        }
        private void DiodeRight(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);
            PathF pathDiode = new PathF();
            pathDiode.MoveTo(0 * Scala, 0 * Scala);
            pathDiode.LineTo(0 * Scala, 10 * Scala);
            pathDiode.LineTo(5 * Scala, 5 * Scala);
            pathDiode.Close();

            canvas.StrokeSize = 1;
            canvas.DrawPath(pathDiode);
            canvas.FillPath(pathDiode);

            PathF pathLine = new PathF();
            pathLine.MoveTo(5 * Scala, 10 * Scala);
            pathLine.LineTo(5 * Scala, 0 * Scala);
            canvas.StrokeSize = 3;
            canvas.DrawPath(pathLine);
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 65);

            drawName(canvas, dirtyRect, name);

            canvas.RestoreState();
        }
        private void DiodeLeft(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);
            PathF pathDiode = new PathF();
            pathDiode.MoveTo(10 * Scala, 10 * Scala);
            pathDiode.LineTo(10 * Scala, 0 * Scala);
            pathDiode.LineTo(5 * Scala, 5 * Scala);
            pathDiode.Close();

            canvas.StrokeSize = 1;
            canvas.DrawPath(pathDiode);
            canvas.FillPath(pathDiode);

            PathF pathLine = new PathF();
            pathLine.MoveTo(5 * Scala, 10 * Scala);
            pathLine.LineTo(5 * Scala, 0 * Scala);
            canvas.StrokeSize = 3;
            canvas.DrawPath(pathLine);
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 65);

            drawName(canvas, dirtyRect, name);

            canvas.RestoreState();
        }
        private void DiodeTop(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);
            PathF pathDiode = new PathF();
            pathDiode.MoveTo(10 * Scala, 10 * Scala);
            pathDiode.LineTo(0 * Scala, 10 * Scala);
            pathDiode.LineTo(5 * Scala, 5 * Scala);
            pathDiode.Close();

            canvas.StrokeSize = 1;
            canvas.DrawPath(pathDiode);
            canvas.FillPath(pathDiode);

            PathF pathLine = new PathF();
            pathLine.MoveTo(0 * Scala, 5 * Scala);
            pathLine.LineTo(10 * Scala, 5 * Scala);
            canvas.StrokeSize = 3;
            canvas.DrawPath(pathLine);
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 65);

            drawName(canvas, dirtyRect, name, 30, 25);

            canvas.RestoreState();
        }

        private void Transistor(ICanvas canvas, RectF dirtyRect, float current, int moveX = 0, int moveY = 0, string name = null)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

            if (Math.Abs(current) > stillOpen)
            {
                canvas.DrawLine(dirtyRect.Left + 150, 0, 150, 40);
            }
            else
            {
                canvas.DrawLine(dirtyRect.Left + 150, 0, 140, 25);
                canvas.DrawLine(150, 40, 150, 30);
            }
            drawName(canvas, dirtyRect, name, 160, 85);
            canvas.RestoreState();
        }

        private void Source(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

            canvas.DrawCircle(10, 10, 10 * Scala);
            canvas.DrawLine(10, 0, 10, 10 * Scala);//pionowa
            canvas.DrawLine(10, 0, 20, 4 * Scala);//prawo
            canvas.DrawLine(0, 4 * Scala, 10, 0);//lewo
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 65);

            canvas.RestoreState();
        }
        private void SourceLine(ICanvas canvas, RectF dirtyRect)
        {
            Source(canvas, dirtyRect, -10, 0); //A
            canvas.DrawLine(dirtyRect.Left, -10, dirtyRect.Left, -20);
            canvas.DrawLine(dirtyRect.Left, -20, dirtyRect.Left + 110, -20);


            canvas.DrawLine(dirtyRect.Left, 30, dirtyRect.Left, 190);
            canvas.DrawLine(dirtyRect.Left, 110, 40, 110);
            canvas.Font = Font.DefaultBold;
            canvas.DrawString("M", -10, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;
            canvas.DrawLine(30, 110, 30, -40);

            canvas.DrawLine(30, -40, 210, -40); //druga linia
            canvas.DrawLine(210, -40, 210, 40);

            canvas.DrawLine(210, -40, 380, -40); //trzecia linia
            canvas.DrawLine(380, -40, 380, 40);

            Source(canvas, dirtyRect, -10, 200); //B
            canvas.DrawLine(dirtyRect.Left, 230, dirtyRect.Left, 240);
            canvas.DrawLine(dirtyRect.Left, 240, dirtyRect.Left + 110, 240);

        }
        private void FirstLine(ICanvas canvas, RectF dirtyRect)
        {

            canvas.Font = Font.DefaultBold;
            canvas.DrawString("A", 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;
            canvas.DrawLine(110, 110, 190, 110);
            canvas.DrawLine(190, 110, 190, 250);


            //gora
            canvas.DrawLine(40, 40, 40, 110);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, "Dmpa");
            canvas.DrawLine(70, 40, 110, 40);

            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, "Dpa");
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            Transistor(canvas, dirtyRect, this.Index, -40, 0, "Spa");
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);    //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            DiodeTop(canvas, dirtyRect, 120, 60, "Dpma");
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);
            Transistor(canvas, dirtyRect, this.Index, -40, 60, "Spma");
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);


            //dol
            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, "Dmna");
            canvas.DrawLine(60, 180, 110, 180);

            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, "Dnma");
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);
            Transistor(canvas, dirtyRect, this.Index, -40, 130, "Snma");
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 190);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, "Dna");
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);
            Transistor(canvas, dirtyRect, this.Index, -40, 190, "Sna");
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);


        }
        private void SecondtLine(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Font = Font.DefaultBold;
            canvas.DrawString("B", 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;
            canvas.DrawLine(110, 110, 190, 110);
            canvas.DrawLine(190, 110, 190, 250);


            canvas.DrawLine(dirtyRect.Left - 60, -20, dirtyRect.Left + 110, -20);

            //gora
            canvas.DrawLine(40, 40, 40, 110);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, "Dmpb");
            canvas.DrawLine(70, 40, 110, 40);

            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, "Dpb");
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            Transistor(canvas, dirtyRect, 0, -40, 0, "Spb");
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);    //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            DiodeTop(canvas, dirtyRect, 120, 60, "Dpmb");
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);
            Transistor(canvas, dirtyRect, 0, -40, 60, "Spmb");
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);


            //dol
            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, "Dmnb");
            canvas.DrawLine(60, 180, 110, 180);

            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, "Dnmb");
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);
            Transistor(canvas, dirtyRect, 0, -40, 130, "Snmb");
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 190);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, "Dnb");
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);
            Transistor(canvas, dirtyRect, 0, -40, 190, "Snb");
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);

            canvas.DrawLine(dirtyRect.Left - 60, 240, dirtyRect.Left + 110, 240);

        }
        private void ThirdLine(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Font = Font.DefaultBold;
            canvas.DrawString("C", 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;
            canvas.DrawLine(110, 110, 190, 110);
            canvas.DrawLine(190, 110, 190, 250);


            canvas.DrawLine(dirtyRect.Left - 60, -20, dirtyRect.Left + 110, -20);

            //gora
            canvas.DrawLine(40, 40, 40, 110);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, "Dmpc");
            canvas.DrawLine(70, 40, 110, 40);

            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, "Dpc");
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            Transistor(canvas, dirtyRect, 0, -40, 0, "Dpc");
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);    //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            DiodeTop(canvas, dirtyRect, 120, 60, "Dmpc");
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);
            Transistor(canvas, dirtyRect, 0, -40, 60, "Snmc");
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);


            //dol
            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, "Dmnc");
            canvas.DrawLine(60, 180, 110, 180);

            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, "Dnmc");
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);
            Transistor(canvas, dirtyRect, 0, -40, 130, "Snmc");
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 190);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, "Dnc");
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);
            Transistor(canvas, dirtyRect, 0, -40, 190, "Snc");
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);

            canvas.DrawLine(dirtyRect.Left - 60, 240, dirtyRect.Left + 110, 240);

        }

        private void Odbline(ICanvas canvas, RectF dirtyRect)
        {
            canvas.DrawRectangle(0, -10, 400, 35);
            canvas.FontSize = 28;
            canvas.DrawString("Odbiornik 3-fazy", 200, 18, HorizontalAlignment.Center);
        }

        private void drawName(ICanvas canvas, RectF dirtyRect, string name, int moveX = 0, int moveY = 0)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string down = "", up = "";
                var letters = name.ToArray();

                up = letters.FirstOrDefault().ToString();

                for (int i = 1; i < letters.Length; i++)
                {
                    down += letters[i].ToString();
                }

                canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

                canvas.DrawString(up.ToUpper(), -52, -75, HorizontalAlignment.Center);
                canvas.DrawString(down.ToUpper(), -48, -70, HorizontalAlignment.Left);
            }
        }

    }
}
