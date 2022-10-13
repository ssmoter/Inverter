namespace Inverter.Data.Draw
{
    public abstract class BaseDrawable : IDrawable
    {
        public abstract void Draw(ICanvas canvas, RectF dirtyRect);
    }
}
