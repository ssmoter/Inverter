namespace Inverter.Data.Draw
{
    internal class Graph : BaseDrawable
    {
        public PathF point { get; set; } = new();
        public bool ResetCanvas = false;
        public Color Color { get; set; } = new();

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.Translate(dirtyRect.Left, dirtyRect.Top);

            canvas.StrokeColor = Color;
            canvas.StrokeSize = 2;
            canvas.DrawPath(point);

            canvas.StrokeColor = Colors.Black;

            canvas.DrawLine(dirtyRect.Left + 30, dirtyRect.Bottom, dirtyRect.Left + 30, dirtyRect.Top); //y
            canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 30, dirtyRect.Left + 10, dirtyRect.Bottom - 30); //x

        }
    }
}
