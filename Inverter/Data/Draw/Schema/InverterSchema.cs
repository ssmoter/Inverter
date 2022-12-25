using Inverter.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace Inverter.Data.Draw.Schema
{
    internal class InverterSchema : BaseDrawable
    {
        public float MaxYValue { get; set; } = 1;
        public float MinYValue { get; set; } = -1;
        public List<DataGraph> Graphs { get; set; }
        public bool BlackWhite { get; set; }

        private readonly int Scala = 2;
        private int StrokeSize = 5;
        public int Index { get; set; }
        private float stillOpen = 0.1f;

        public InverterSchema(List<DataGraph> graphs)
        {
            Graphs = graphs;
            stillOpen = 0;
            for (int i = 0; i < Graphs.Count; i++)
            {
                if (Graphs[i].UserDataName == "Tyrystor")
                {
                    stillOpen += Math.Abs(Graphs[i].Min);
                }
            }
            if (stillOpen < 0.1)
            {
                stillOpen = 0.1f;
            }
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            if (BlackWhite)
            {
                canvas.StrokeColor = Colors.White;
                canvas.FillColor = Colors.White;
                canvas.FontColor = Colors.White;
            }
            else
            {
                canvas.StrokeColor = Colors.Black;
                canvas.FillColor = Colors.Black;
                canvas.FontColor = Colors.Black;
            }

            float scaleX = 1, scaleY = 1;

            scaleX = dirtyRect.Width / 650;
            scaleY = dirtyRect.Height / 350;

            canvas.Scale(scaleX, scaleY);
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 50);

            canvas.StrokeSize = StrokeSize;

            SourceLine(canvas, dirtyRect);

            canvas.Translate(10, 0);
            FirstLine(canvas, dirtyRect);

            canvas.Translate(180, 0);
            SecondtLine(canvas, dirtyRect);
            canvas.Translate(190, 0);
            ThirdLine(canvas, dirtyRect);

            canvas.Translate(-195, 260);
            OdbLine(canvas, dirtyRect);
            canvas.RestoreState();
        }

        private void SourceLine(ICanvas canvas, RectF dirtyRect)
        {

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(VzP)").FirstOrDefault().Y[Index]);
            Source(canvas, dirtyRect, -10, 0, "UD 1/2"); //A
            canvas.DrawLine(dirtyRect.Left, 30, dirtyRect.Left, 110);//190


            float sum = Graphs.Where(x => x.DataName == "I(S_PA)").FirstOrDefault().Y[Index] +
                     Graphs.Where(x => x.DataName == "I(D_PAz)").FirstOrDefault().Y[Index] +
                     Graphs.Where(x => x.DataName == "I(S_PB)").FirstOrDefault().Y[Index] +
                     Graphs.Where(x => x.DataName == "I(D_PBz)").FirstOrDefault().Y[Index] +
                     Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index] +
                     Graphs.Where(x => x.DataName == "I(D_PCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);

            canvas.DrawLine(dirtyRect.Left, -10, dirtyRect.Left, -20);
            canvas.DrawLine(dirtyRect.Left, -20, dirtyRect.Left + 120, -20);
            DrawDot(canvas, dirtyRect, 110, -30);



            sum = Graphs.Where(x => x.DataName == "I(DPA)").FirstOrDefault().Y[Index] +
                        Graphs.Where(x => x.DataName == "I(DNA)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 30, 110, 50, 110);

            sum = Graphs.Where(x => x.DataName == "I(DPA)").FirstOrDefault().Y[Index] +
                         Graphs.Where(x => x.DataName == "I(DNA)").FirstOrDefault().Y[Index] +
                         Graphs.Where(x => x.DataName == "I(DPB)").FirstOrDefault().Y[Index] +
                         Graphs.Where(x => x.DataName == "I(DNB)").FirstOrDefault().Y[Index] +
                         Graphs.Where(x => x.DataName == "I(DPC)").FirstOrDefault().Y[Index] +
                         Graphs.Where(x => x.DataName == "I(DNC)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);

          
            DrawDot(canvas, dirtyRect, 20, 100);
            canvas.DrawLine(dirtyRect.Left, 110, 30, 110);

            sum += Graphs.Where(x => x.DataName == "I(VzP)").FirstOrDefault().Y[Index] +
            Graphs.Where(x => x.DataName == "I(VzN)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, -10, 100);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString("M", -10, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;

            sum = Graphs.Where(x => x.DataName == "I(DPB)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(DNB)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(DPC)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(DNC)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);

            canvas.DrawLine(30, 110, 30, -40);
            canvas.DrawLine(30, -40, 230, -40); //druga linia
            DrawDot(canvas, dirtyRect, 220, -50);

            sum = Graphs.Where(x => x.DataName == "I(DPB)").FirstOrDefault().Y[Index] +
                    Graphs.Where(x => x.DataName == "I(DNB)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(230, -40, 230, 40);
            DrawDot(canvas, dirtyRect, 220, 30);


            sum = Graphs.Where(x => x.DataName == "I(DPC)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(DNC)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(230, -40, 420, -40); //trzecia linia
            canvas.DrawLine(420, -40, 420, 40);
            DrawDot(canvas, dirtyRect, 410, 30);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(VzN)").FirstOrDefault().Y[Index]);
            Source(canvas, dirtyRect, -10, 200, "UD 1/2"); //B
            canvas.DrawLine(dirtyRect.Left, 110, dirtyRect.Left, 190);//190

            sum = Graphs.Where(x => x.DataName == "I(S_NC)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 300, 240, dirtyRect.Left + 490, 240);

            sum += Graphs.Where(x => x.DataName == "I(S_NB)").FirstOrDefault().Y[Index] +
                   Graphs.Where(x => x.DataName == "I(D_NBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 120, 240, dirtyRect.Left + 300, 240);
            DrawDot(canvas, dirtyRect, 290, 230);

            sum += Graphs.Where(x => x.DataName == "I(S_NA)").FirstOrDefault().Y[Index] +
                   Graphs.Where(x => x.DataName == "I(D_NAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 110, 230);
            canvas.DrawLine(dirtyRect.Left, 230, dirtyRect.Left, 240);
            canvas.DrawLine(dirtyRect.Left, 240, dirtyRect.Left + 120, 240);

        }
        private void FirstLine(ICanvas canvas, RectF dirtyRect)
        {

            canvas.Font = Font.DefaultBold;
            canvas.DrawString("A", 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;


            //gora
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(DPA)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(40, 40, 40, 110);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, "Dmpa");
            canvas.DrawLine(70, 40, 110, 40);
            float sum = Graphs.Where(x => x.DataName == "I(DPA)").FirstOrDefault().Y[Index] +
              Graphs.Where(x => x.DataName == "I(S_PA)").FirstOrDefault().Y[Index] +
              Graphs.Where(x => x.DataName == "I(D_PAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 30);

            sum = Graphs.Where(x => x.DataName == "I(DPA)").FirstOrDefault().Y[Index] +
                          Graphs.Where(x => x.DataName == "I(DNA)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 30, 100);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_PAz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, "Dpa");
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);

            sum = Graphs.Where(x => x.DataName == "I(S_PA)").FirstOrDefault().Y[Index] +
               Graphs.Where(x => x.DataName == "I(D_PAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            DrawDot(canvas, dirtyRect, 100, -20);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_PA)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_PA)").FirstOrDefault().Y[Index], -40, 0, "Spa");

            sum = Graphs.Where(x => x.DataName == "I(D_PMAz)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(S_PMA)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            DrawDot(canvas, dirtyRect, 100, 40);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_PMAz)").FirstOrDefault().Y[Index]); //odnoga dla diody 
            DiodeTop(canvas, dirtyRect, 120, 60, "Dpma");
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_PMA)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_PMA)").FirstOrDefault().Y[Index], -40, 60, "Spma");
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);
            sum = Graphs.Where(x => x.DataName == "I(S_PMA)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_PMAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 90);


            //dol
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(DNA)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, "Dmna");
            canvas.DrawLine(60, 180, 110, 180);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_NMAz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, "Dnma");
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);

            sum = Graphs.Where(x => x.DataName == "I(D_NMAz)").FirstOrDefault().Y[Index] + Graphs.Where(x => x.DataName == "I(S_NMA)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);

            sum = Graphs.Where(x => x.DataName == "I(S_NMA)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NMAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 110);
            DrawDot(canvas, dirtyRect, 100, 160);
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_NMA)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_NMA)").FirstOrDefault().Y[Index], -40, 130, "Snma");
            sum = Graphs.Where(x => x.DataName == "I(S_NMA)").FirstOrDefault().Y[Index] + Graphs.Where(x => x.DataName == "I(D_NAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 180);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_NAz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, "Dna");
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_NA)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 110, 190);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_NA)").FirstOrDefault().Y[Index], -40, 190, "Sna");
            sum = Graphs.Where(x => x.DataName == "I(S_NA)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NAz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 220);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(VoA)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(110, 110, 190, 110);
            canvas.DrawLine(190, 110, 190, 250);

            DrawDot(canvas, dirtyRect, 100, 100);
        }
        private void SecondtLine(ICanvas canvas, RectF dirtyRect)
        {

            float sum = Graphs.Where(x => x.DataName == "I(S_PB)").FirstOrDefault().Y[Index] +
                        Graphs.Where(x => x.DataName == "I(D_PBz)").FirstOrDefault().Y[Index] +
                        Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index] +
                        Graphs.Where(x => x.DataName == "I(D_PCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, -30);
            canvas.DrawLine(dirtyRect.Left - 70, -20, dirtyRect.Left + 110, -20);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString("B", 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;


            //gora
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(DPB)").FirstOrDefault().Y[Index]);

            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, "Dmpb");
            canvas.DrawLine(70, 40, 110, 40);

            sum = Graphs.Where(x => x.DataName == "I(DPB)").FirstOrDefault().Y[Index] +
                    Graphs.Where(x => x.DataName == "I(S_PB)").FirstOrDefault().Y[Index] +
                    Graphs.Where(x => x.DataName == "I(D_PBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 30);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_PBz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, "Dpb");
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);

            sum = Graphs.Where(x => x.DataName == "I(S_PB)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(D_PBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            DrawDot(canvas, dirtyRect, 100, -20);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_PB)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_PB)").FirstOrDefault().Y[Index], -40, 0, "Spb");

            sum = Graphs.Where(x => x.DataName == "I(D_PMBz)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(S_PMB)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            DrawDot(canvas, dirtyRect, 100, 40);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_PMBz)").FirstOrDefault().Y[Index]); //odnoga dla diody 
            DiodeTop(canvas, dirtyRect, 120, 60, "Dpmb");
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_PMB)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_PMB)").FirstOrDefault().Y[Index], -40, 60, "Spmb");
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);
            sum = Graphs.Where(x => x.DataName == "I(S_PMB)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_PMBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 90);

            //dol
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(DNB)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(40, 40, 40, 110);

            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, "DmnB");
            canvas.DrawLine(60, 180, 110, 180);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_NMBz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, "Dnmb");
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);

            sum = Graphs.Where(x => x.DataName == "I(D_NMBz)").FirstOrDefault().Y[Index] + Graphs.Where(x => x.DataName == "I(S_NMB)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);

            sum = Graphs.Where(x => x.DataName == "I(S_NMB)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NMBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 110);
            DrawDot(canvas, dirtyRect, 100, 160);
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_NMB)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_NMB)").FirstOrDefault().Y[Index], -40, 130, "Snmb");
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 180);

            sum = Graphs.Where(x => x.DataName == "I(S_NB)").FirstOrDefault().Y[Index] + Graphs.Where(x => x.DataName == "I(D_NBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 110, 190);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_NBz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, "Dnb");
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_NB)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_NB)").FirstOrDefault().Y[Index], -40, 190, "Snb");

            sum = Graphs.Where(x => x.DataName == "I(S_NB)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NBz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);
            DrawDot(canvas, dirtyRect, 100, 220);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(VoB)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(110, 110, 190, 110);
            canvas.DrawLine(190, 110, 190, 250);

            DrawDot(canvas, dirtyRect, 100, 100);

        }
        private void ThirdLine(ICanvas canvas, RectF dirtyRect)
        {
            float sum = Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index] +
                        Graphs.Where(x => x.DataName == "I(D_PCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left - 80, -20, dirtyRect.Left + 110, -20);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString("C", 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;


            //gora
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(DPC)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, "Dmpc");
            canvas.DrawLine(70, 40, 110, 40);

            sum = Graphs.Where(x => x.DataName == "I(DPC)").FirstOrDefault().Y[Index] +
                   Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index] +
                   Graphs.Where(x => x.DataName == "I(D_PCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 30);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_PCz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, "Dpc");
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);

            sum = Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(D_PCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            DrawDot(canvas, dirtyRect, 100, -20);


            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_PC)").FirstOrDefault().Y[Index], -40, 0, "Spc");

            sum = Graphs.Where(x => x.DataName == "I(D_PMCz)").FirstOrDefault().Y[Index] +
                Graphs.Where(x => x.DataName == "I(S_PMC)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            DrawDot(canvas, dirtyRect, 100, 40);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_PMCz)").FirstOrDefault().Y[Index]); //odnoga dla diody 
            DiodeTop(canvas, dirtyRect, 120, 60, "Dpmc");
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_PMC)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_PMC)").FirstOrDefault().Y[Index], -40, 60, "Spmc");
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);
            sum = Graphs.Where(x => x.DataName == "I(S_PMC)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_PMCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 90);

            //dol
            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(DNC)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(40, 40, 40, 110);

            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, "Dmnc");
            canvas.DrawLine(60, 180, 110, 180);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_NMCz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, "Dnmc");
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);

            sum = Graphs.Where(x => x.DataName == "I(D_NMCz)").FirstOrDefault().Y[Index] + Graphs.Where(x => x.DataName == "I(S_NMC)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);

            sum = Graphs.Where(x => x.DataName == "I(S_NMC)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NMCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 110);
            DrawDot(canvas, dirtyRect, 100, 160);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_NMC)").FirstOrDefault().Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_NMC)").FirstOrDefault().Y[Index], -40, 130, "Snmc");
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 180);

            sum = Graphs.Where(x => x.DataName == "I(S_NC)").FirstOrDefault().Y[Index] + Graphs.Where(x => x.DataName == "I(D_NCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 170);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(D_NCz)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, "Dnc");
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(S_NC)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 110, 190);
            Transistor(canvas, dirtyRect, Graphs.Where(x => x.DataName == "I(S_NC)").FirstOrDefault().Y[Index], -40, 190, "Snc");

            sum = Graphs.Where(x => x.DataName == "I(S_NC)").FirstOrDefault().Y[Index] +
                  Graphs.Where(x => x.DataName == "I(D_NCz)").FirstOrDefault().Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);
            DrawDot(canvas, dirtyRect, 100, 220);

            ReturnColor(canvas, Graphs.Where(x => x.DataName == "I(VoC)").FirstOrDefault().Y[Index]);
            canvas.DrawLine(110, 110, 190, 110);
            canvas.DrawLine(190, 110, 190, 250);

            DrawDot(canvas, dirtyRect, 100, 100);

        }
        private void OdbLine(ICanvas canvas, RectF dirtyRect)
        {
            if (BlackWhite)
            {
                canvas.StrokeColor = Colors.White;
                canvas.FillColor = Colors.White;
            }
            else
            {
                canvas.StrokeColor = Colors.Black;
                canvas.FillColor = Colors.Black;
            }

            canvas.DrawRectangle(0, -10, 400, 35);
            canvas.FontSize = 28;
            canvas.DrawString("Odbiornik 3-fazy", 200, 18, HorizontalAlignment.Center);
        }


        private Color ReturnColor(ICanvas canvas, float current)
        {
            Color c = new Color();

            if (Math.Abs(current) > stillOpen)
            {
                c = Colors.Red;
            }
            else
            {
                if (BlackWhite)
                    c = Colors.White;
                else
                    c = Colors.Black;
            }

            canvas.StrokeColor = c;
            canvas.FillColor = c;
            //canvas.FontColor = c;
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

            DrawName(canvas, dirtyRect, name);

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

            DrawName(canvas, dirtyRect, name);

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

            DrawName(canvas, dirtyRect, name, 30, 25);

            canvas.RestoreState();
        }

        private void Transistor(ICanvas canvas, RectF dirtyRect, float current, int moveX = 0, int moveY = 0, string name = null)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

            if (current > stillOpen)
            {
                canvas.DrawLine(dirtyRect.Left + 150, 0, 150, 40);
            }
            else
            {
                canvas.DrawLine(dirtyRect.Left + 150, 0, 140, 25);
                canvas.DrawLine(150, 40, 150, 30);
            }
            DrawName(canvas, dirtyRect, name, 160, 85);
            canvas.RestoreState();
        }

        private void Source(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null)
        {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

            canvas.DrawCircle(10, 10, 10 * Scala);
            canvas.DrawLine(10, 0, 10, 10 * Scala);//pionowa
            canvas.DrawLine(10, 0, 20, 4 * Scala);//prawo
            canvas.DrawLine(0, 4 * Scala, 10, 0);//lewo
            canvas.Translate(dirtyRect.Left + 50, dirtyRect.Top + 65);

            DrawName(canvas, dirtyRect, name, -30, -10);

            canvas.RestoreState();
        }

        private void DrawName(ICanvas canvas, RectF dirtyRect, string name, int moveX = 0, int moveY = 0)
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

        private void DrawDot(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0)
        {
            canvas.Translate(moveX, moveY);
            canvas.FillCircle(10, 10, 5);
            canvas.Translate(-moveX, -moveY);
        }

    }
}
