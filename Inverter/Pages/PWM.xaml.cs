using Inverter.Data.Draw;
using Inverter.Models;
using System.Timers;

namespace Inverter.Pages;

public partial class PWM : ContentPage
{
    private System.Timers.Timer timer = new System.Timers.Timer(1);
    private GraphicsView graphicsDraw;
    private Graph graph;
    private Graph graph2;

    private Axis axis;
    public uint multiTime = 10;


    public PWM()
    {
        InitializeComponent();
        graphicsDraw = this.DrawAxis;
        axis = (Axis)graphicsDraw.Drawable;
        graphicsDraw = this.Draw;
        timer.Elapsed += new ElapsedEventHandler(RedrawLine);
        timer.Stop();
        graph = (Graph)graphicsDraw.Drawable;
        graph2 = (Graph)graphicsDraw.Drawable;
        DrawLine();
        InverterParameters inventerParameters = new InverterParameters();
    }
    int n = 0;

    public void DrawLine()
    {
        for (int i = 0; i < 4_000; i++)
        {
            float x = i + 10;
            float y = (float)(100 * Math.Sin(50 * 3.14 * i) + 50);
            pp.Add(new PointF(x, y));
            p2.Add(new PointF(x + 10, y));
        }

        graph.PositionY = (float)pp.Max(e => e.Y);
        axis.MaxNumberY = (int)graph.PositionY;
        axis.MaxNumberX = pp.Count;

        // Draw.WidthRequest = pp.Count;


        graph.point = new PathF();

        for (int i = 0; i < pp.Count; i++)
        {
            graph.point.LineTo(pp[i]);
        }
        graph.point.MoveTo(pp[0]);
        //for (int i = 0; i < pp.Count; i++)
        //{
        //    graph.point.LineTo(p2[i]);
        //}
        //graphicsDraw.Invalidate();
    }

    List<Point> pp = new List<Point>();
    List<Point> p2 = new List<Point>();


    public void RedrawLine(object source, ElapsedEventArgs e)
    {
        //graph.point = new PathF();
        // Move();
        n++;
        for (int i = 0; i < pp.Count; i++)
        {
            ////graph.point.LineTo(5, 70 * (float)Math.Sin(i)+100);
            //float x = (float)i + 10;
            //float y = 50 * (float)Math.Cos(50 * 3.14 * i) + 50;
            //float y2 = ((float)(50 * Math.Sin(50 * 3.14 * i) + 50));
            //// graph.point.LineTo(x, y);
            //graph.point.LineTo(x, y2);


            //  graph.point.LineTo(pp[i]);
        }
        // graphicsDraw.Invalidate();
    }

    bool isTimer = true;
    private void OnTapGestureRecognizerTapped(object sender, EventArgs args)
    {
        this.Draw.ScaleY = double.Parse(graph.ScaleY);

        //if (isTimer)
        //{
        //    timer.Start();
        //    isTimer = !isTimer;
        //}
        //else
        //{
        //    timer.Stop();
        //    isTimer = !isTimer;
        //}
    }

    private void Reset(object sender, EventArgs args)
    {
        Move();
    }

    private void Move()
    {

        if (Draw.AnimationIsRunning("rot"))
        {
            Draw.AbortAnimation("rot");
            return;
        }

        uint time = 0;
        double max = pp.Max(t => t.Y);
        for (int i = 0; i < pp.Count; i++)
        {
            time++;
            if (i >= pp.Count * 0.5)
            {
                if (Math.Abs(pp[i].Y) < 0.1)
                {
                    break;
                }

            }
        }
        var anim = new Animation((e) =>
        {
            Draw.TranslationX = e;
        }, 0, -time / 10);


        Draw.Animate("rot", anim, rate: 1, length: time, Easing.Linear, null, () => true);
    }
}