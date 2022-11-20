namespace Inverter.Data.Draw.Schema
{
    public class LineTimeSchema : BaseDrawable
    {
        public int Index { get; set; }
        public bool IsHidden { get; set; } = true;

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (IsHidden)
            {
                canvas.SaveState();
                canvas.Translate(dirtyRect.Left + 8, dirtyRect.Top);

                canvas.StrokeSize = 2;

                canvas.StrokeColor = Colors.Black;
                canvas.DrawLine(dirtyRect.Left + Index + 71, dirtyRect.Bottom, dirtyRect.Left + Index + 71, dirtyRect.Top); //y

                canvas.StrokeColor = Colors.White;
                canvas.DrawLine(dirtyRect.Left + Index + 70, dirtyRect.Bottom, dirtyRect.Left + Index + 70, dirtyRect.Top); //y

                canvas.StrokeColor = Colors.Red;
                canvas.DrawLine(dirtyRect.Left + Index + 69, dirtyRect.Bottom, dirtyRect.Left + Index + 69, dirtyRect.Top); //y
                canvas.ResetState();
            }
        }
    }
}
