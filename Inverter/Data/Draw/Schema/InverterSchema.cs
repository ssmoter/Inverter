namespace Inverter.Data.Draw.Schema
{
    internal class InverterSchema : BaseDrawable
    {
        public float MaxYValue { get; set; } = 100;
        public float MinYValue { get; set; }
        private int skala = 2;
        public int Index { get; set; }
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 50);

            var ColorFill = ReturnColor(Index);
            canvas.StrokeColor = ColorFill;
            canvas.FillColor = ColorFill;
            canvas.FontColor = ColorFill;
            DiodeRight(canvas, dirtyRect, 100, 100);
            DiodeLeft(canvas, dirtyRect, 150, 100);
            DiodeTop(canvas, dirtyRect, 200, 100);


            canvas.ResetState();
        }

        private Color ReturnColor(float current)
        {
            Color c = new Color();

            switch (current)
            {
                case > 0:
                    c = Color.FromRgb(255, 0, 0);
                    if (current < (MaxYValue / 2))
                        c = Color.FromRgb(128, 0, 0);

                    break;
                case < 0:
                    c = Color.FromRgb(0, 0, 255);
                    if (current < (MinYValue / 2))
                        c = Color.FromRgb(0, 0, 128);

                    break;
                default:
                    c = Colors.Black;
                    break;
            }
            return c;
        }
        private void DiodeRight(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);
            PathF pathDiode = new PathF();
            pathDiode.MoveTo(0 * skala, 0 * skala);
            pathDiode.LineTo(0 * skala, 10 * skala);
            pathDiode.LineTo(5 * skala, 5 * skala);
            pathDiode.Close();

            canvas.StrokeSize = 1;
            canvas.DrawPath(pathDiode);
            canvas.FillPath(pathDiode);

            PathF pathLine = new PathF();
            pathLine.MoveTo(5 * skala, 10 * skala);
            pathLine.LineTo(5 * skala, 0 * skala);
            canvas.StrokeSize = 3;
            canvas.DrawPath(pathLine);

            canvas.RestoreState();
        }
        private void DiodeLeft(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);
            PathF pathDiode = new PathF();
            pathDiode.MoveTo(10 * skala, 10 * skala);
            pathDiode.LineTo(10 * skala, 0 * skala);
            pathDiode.LineTo(5 * skala, 5 * skala);
            pathDiode.Close();

            canvas.StrokeSize = 1;
            canvas.DrawPath(pathDiode);
            canvas.FillPath(pathDiode);

            PathF pathLine = new PathF();
            pathLine.MoveTo(5 * skala, 10 * skala);
            pathLine.LineTo(5 * skala, 0 * skala);
            canvas.StrokeSize = 3;
            canvas.DrawPath(pathLine);

            canvas.RestoreState();
        }
        private void DiodeTop(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);
            PathF pathDiode = new PathF();
            pathDiode.MoveTo(10 * skala, 10 * skala);
            pathDiode.LineTo(0 * skala, 10 * skala);
            pathDiode.LineTo(5 * skala, 5 * skala);
            pathDiode.Close();

            canvas.StrokeSize = 1;
            canvas.DrawPath(pathDiode);
            canvas.FillPath(pathDiode);

            PathF pathLine = new PathF();
            pathLine.MoveTo(0 * skala, 5 * skala);
            pathLine.LineTo(10 * skala, 5 * skala);
            canvas.StrokeSize = 3;
            canvas.DrawPath(pathLine);

            canvas.RestoreState();
        }




    }
}
