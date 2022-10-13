using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inverter.Data.Draw
{
    internal class Graph : BaseDrawable, INotifyPropertyChanged
    {
        public PathF point { get; set; }
        public float PositionY { get; set; } = 0;
        private int positionX;
        public int PositionX
        {
            get => positionX;
            set => positionX = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged( string name )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(name)));
        }
        private string scaleY;
        public string ScaleY
        {
            get => scaleY;
            set
            {
                scaleY = value;
                OnPropertyChanged(ScaleY);
            }
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            scaleY = (dirtyRect.Bottom / PositionY).ToString();
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left - 10, dirtyRect.Bottom - (PositionY + 35));


            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            canvas.DrawPath(point);

            canvas.RestoreState();

        }
    }
}
