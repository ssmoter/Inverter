namespace Inverter.Data.Draw
{
    public class Axis : BaseDrawable, IDrawable
    {
        public int MaxNumberY { get; set; } = 0;
        public int MaxNumberX { get; set; } = 0;

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;

            canvas.DrawLine(dirtyRect.Left + 30, dirtyRect.Bottom, dirtyRect.Left + 30, dirtyRect.Top); //y
            canvas.DrawLine(dirtyRect.Width, dirtyRect.Bottom - 30, dirtyRect.Left + 10, dirtyRect.Bottom - 30); //x


            //wartości na osi Y
            int axisYValue = -MaxNumberY;
            for (int i = 0; i < (MaxNumberY * 2); i = i + ((int)dirtyRect.Bottom / 20))
            {
                canvas.RestoreState();

                canvas.DrawString(axisYValue.ToString(), 25, dirtyRect.Bottom - i - 35, HorizontalAlignment.Right);

                axisYValue = axisYValue + ((int)dirtyRect.Bottom / 15);
                canvas.SaveState();
            }


            //wartosci na osi X
            bool verticalX = false;
            for (int i = 0; i < MaxNumberX; i = i + 30)
            {
                if (!verticalX)
                {
                    canvas.DrawString(i.ToString(), i + 30, dirtyRect.Bottom - 20, HorizontalAlignment.Left);
                    verticalX = !verticalX;
                }
                else if (verticalX)
                {
                    canvas.DrawString(i.ToString(), i + 30, dirtyRect.Bottom - 5, HorizontalAlignment.Left);
                    verticalX = !verticalX;
                }

            }




            //for (int i = -MaxNumberY; i < MaxNumberY; i = i + ((int)dirtyRect.Bottom / 10))
            //{
            //    canvas.RestoreState();
            //    canvas.DrawString(i.ToString(), 5, dirtyRect.Bottom - i, HorizontalAlignment.Left);
            //    canvas.SaveState();
            //}



        }
    }
}
