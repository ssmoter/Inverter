namespace Inverter.Data.Draw
{
    internal class Graph : BaseDrawable
    {
        public PathF point { get; set; } = new();
        public Color Color { get; set; } = new();

        public float MaxY { get; set; } = 1;

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.Translate(dirtyRect.Left, dirtyRect.Center.Y);

            canvas.StrokeColor = Color;
            canvas.StrokeSize = 2;
            canvas.DrawPath(point);

            canvas.StrokeColor = Colors.Black;

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(dirtyRect.Left, dirtyRect.Top);


            canvas.DrawLine(dirtyRect.Left + 30, dirtyRect.Bottom, dirtyRect.Left + 30, dirtyRect.Top); //y
            canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 30, dirtyRect.Left + 10, dirtyRect.Bottom - 30); //x

            var maxSizeY = (MaxY/2).ToString().Split('.', ',');



            int maxYLength = (int)(dirtyRect.Height / int.Parse(maxSizeY[0]));

            int axisYValue = (int)MaxY;

            for (int i = 0; i < maxYLength; i++)
            {
                canvas.DrawString(axisYValue.ToString(), 25, dirtyRect.Top + ((dirtyRect.Bottom / maxYLength) * i), HorizontalAlignment.Right);
                axisYValue -= (int)(MaxY / maxYLength);
            }
            canvas.RestoreState();

        }
    }
}
