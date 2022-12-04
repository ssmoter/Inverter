namespace Inverter.Data.Draw.Schema
{
    public class LineTimeSchema : BaseDrawable
    {
        public int Index { get; set; }
        public bool IsHidden { get; set; } = true;
        public bool AutoScaleX { get; set; }
        public int StartScopeIndex { get; set; } = 0;
        public int EndScopeIndex { get; set; } = 0;
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            //skala w osi X
            float scaleX = 1;

            scaleX = (dirtyRect.Width - 100) / (EndScopeIndex - StartScopeIndex);
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + 75, dirtyRect.Top);

            if (AutoScaleX)
            {
                canvas.Scale(scaleX, 1);
            }
            if (IsHidden)
            {
                canvas.StrokeSize = 1;

                canvas.StrokeColor = Colors.White;
                canvas.DrawLine(dirtyRect.Left + Index, dirtyRect.Bottom, dirtyRect.Left + Index, dirtyRect.Top); //y

                canvas.StrokeColor = Colors.Black;
                canvas.DrawLine(dirtyRect.Left + Index, dirtyRect.Bottom - 1, dirtyRect.Left + Index, dirtyRect.Top); //y

                canvas.StrokeColor = Colors.Red;
                canvas.DrawLine(dirtyRect.Left + Index, dirtyRect.Bottom + 1, dirtyRect.Left + Index, dirtyRect.Top); //y
                canvas.ResetState();
            }
        }
    }
}
