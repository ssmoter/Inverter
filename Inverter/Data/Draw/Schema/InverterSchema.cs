using Inverter.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace Inverter.Data.Draw.Schema {
    internal class InverterSchema : BaseDrawable {
        public float MaxYValue { get; set; } = 1;
        public float MinYValue { get; set; } = -1;
        public List<DataGraph> Graphs { get; set; }
        public bool BlackWhite { get; set; }

        private readonly int Scala = 2;
        private int StrokeSize = 5;
        public int Index { get; set; }
        private float stillOpen = 0.1f;
        public float StillOpen = 0.1f;
        public float StillOpenUser { get; set; }
        public bool OpenUser { get; set; }
        private FileManager _fm;

        #region Zmienne

        private const string M = "M";
        private const string A = "A";
        private const string B = "B";
        private const string C = "C";
        private const string IVzP = "I(VzP)";
        private const string UVzP = "UVzP";
        private const string IS_PA = "I(S_PA)";
        private const string ID_PAz = "I(D_PAz)";
        private const string IS_PB = "I(S_PB)";
        private const string ID_PBz = "I(D_PBz)";
        private const string IS_PC = "I(S_PC)";
        private const string ID_PCz = "I(D_PCz)";
        private const string IDPA = "I(DPA)";
        private const string IDNA = "I(DNA)";
        private const string IDPB = "I(DPB)";
        private const string IDNB = "I(DNB)";
        private const string IDPC = "I(DPC)";
        private const string IDNC = "I(DNC)";
        private const string IVzN = "I(VzN)";
        private const string UVzN = "UVzN";
        private const string IS_NC = "I(S_NC)";
        private const string ID_NCz = "I(D_NCz)";
        private const string IS_NB = "I(S_NB)";
        private const string ID_NBz = "I(D_NBz)";
        private const string IS_NA = "I(S_NA)";
        private const string ID_NAz = "I(D_NAz)";
        private const string DPA = "DPA";
        private const string D_PAz = "D_PAz";
        private const string S_PA = "S_PA";
        private const string ID_PMAz = "I(D_PMAz)";
        private const string IS_PMA = "I(S_PMA)";
        private const string D_PMAz = "D_PMAz";
        private const string S_PMA = "S_PMA";
        private const string DNA = "DNA";
        private const string ID_NMAz = "I(D_NMAz)";
        private const string D_NMAz = "D_NMAz";
        private const string IS_NMA = "I(S_NMA)";
        private const string S_NMA = "S_NMA";
        private const string D_NAz = "D_NAz";
        private const string S_NA = "S_NA";
        private const string IVoA = "I(VoA)";
        private const string DPB = "DPB";
        private const string D_PBz = "D_PBz";
        private const string S_PB = "S_PB";
        private const string ID_PMBz = "I(D_PMBz)";
        private const string IS_PMB = "I(S_PMB)";
        private const string D_PMBz = "D_PMBz";
        private const string S_PMB = "S_PMB";
        private const string DNB = "DNB";
        private const string ID_NMBz = "I(D_NMBz)";
        private const string D_NMBz = "D_NMBz";
        private const string IS_NMB = "I(S_NMB)";
        private const string S_NMB = "S_NMB";
        private const string D_NBz = "D_NBz";
        private const string S_NB = "S_NB";
        private const string IVoB = "I(VoB)";
        private const string DPC = "DPC";
        private const string D_PCz = "D_PCz";
        private const string S_PC = "S_PC";
        private const string ID_PMCz = "I(D_PMCz)";
        private const string IS_PMC = "I(S_PMC)";
        private const string D_PMCz = "D_PMCz";
        private const string S_PMC = "S_PMC";
        private const string DNC = "DNC";
        private const string ID_NMCz = "I(D_NMCz)";
        private const string D_NMCz = "D_NMCz";
        private const string IS_NMC = "I(S_NMC)";
        private const string S_NMC = "S_NMC";
        private const string D_NCz = "D_NCz";
        private const string S_NC = "S_NC";
        private const string IVoC = "I(VoC)";

        private const string ODB = "Odbiornik 3-fazy";


        #endregion

        public InverterSchema(List<DataGraph> graphs, FileManager fm) {
            Graphs = graphs;
            StillOpen = 0.1f;
            StillOpen = Graphs.Where(x => x.UserDataName.Contains("Tranzystor")).Min(x => x.Max) / 10;
            _fm = fm;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect) {
            try {
                if (OpenUser)
                    stillOpen = StillOpenUser;
                else
                    stillOpen = StillOpen;


                canvas.SaveState();
                if (BlackWhite) {
                    canvas.StrokeColor = Colors.White;
                    canvas.FillColor = Colors.White;
                    canvas.FontColor = Colors.White;
                }
                else {
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
                SecondLine(canvas, dirtyRect);
                canvas.Translate(190, 0);
                ThirdLine(canvas, dirtyRect);

                canvas.Translate(-195, 260);
                OdbLine(canvas, dirtyRect);
                canvas.RestoreState();
            }
            catch (Exception ex) {
                _fm.SaveLog(ex.ToString());
            }
        }

        private void SourceLine(ICanvas canvas, RectF dirtyRect) {

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IVzP).Y[Index]);
            Source(canvas, dirtyRect, -10, 0, UVzP); //A
            canvas.DrawLine(dirtyRect.Left, 30, dirtyRect.Left, 110);//190


            float sum = Graphs.FirstOrDefault(x => x.DataName == IS_PA).Y[Index] +
                     Graphs.FirstOrDefault(x => x.DataName == ID_PAz).Y[Index] +
                     Graphs.FirstOrDefault(x => x.DataName == IS_PB).Y[Index] +
                     Graphs.FirstOrDefault(x => x.DataName == ID_PBz).Y[Index] +
                     Graphs.FirstOrDefault(x => x.DataName == IS_PC).Y[Index] +
                     Graphs.FirstOrDefault(x => x.DataName == ID_PCz).Y[Index];
            ReturnColor(canvas, sum);

            canvas.DrawLine(dirtyRect.Left, -10, dirtyRect.Left, -20);
            canvas.DrawLine(dirtyRect.Left, -20, dirtyRect.Left + 120, -20);
            DrawDot(canvas, dirtyRect, 110, -30);



            sum = Graphs.FirstOrDefault(x => x.DataName == IDPA).Y[Index] +
                        Graphs.FirstOrDefault(x => x.DataName == IDNA).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 30, 110, 50, 110);

            sum = Graphs.FirstOrDefault(x => x.DataName == IDPA).Y[Index] +
                         Graphs.FirstOrDefault(x => x.DataName == IDNA).Y[Index] +
                         Graphs.FirstOrDefault(x => x.DataName == IDPB).Y[Index] +
                         Graphs.FirstOrDefault(x => x.DataName == IDNB).Y[Index] +
                         Graphs.FirstOrDefault(x => x.DataName == IDPC).Y[Index] +
                         Graphs.FirstOrDefault(x => x.DataName == IDNC).Y[Index];
            ReturnColor(canvas, sum);


            DrawDot(canvas, dirtyRect, 20, 100);
            canvas.DrawLine(dirtyRect.Left, 110, 30, 110);

            sum += Graphs.FirstOrDefault(x => x.DataName == IVzP).Y[Index] +
            Graphs.FirstOrDefault(x => x.DataName == IVzN).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, -10, 100);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString(M, -10, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;

            sum = Graphs.FirstOrDefault(x => x.DataName == IDPB).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IDNB).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IDPC).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IDNC).Y[Index];
            ReturnColor(canvas, sum);

            canvas.DrawLine(30, 110, 30, -40);
            canvas.DrawLine(30, -40, 230, -40); //druga linia
            DrawDot(canvas, dirtyRect, 220, -50);

            sum = Graphs.FirstOrDefault(x => x.DataName == IDPB).Y[Index] +
                    Graphs.FirstOrDefault(x => x.DataName == IDNB).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(230, -40, 230, 40);
            DrawDot(canvas, dirtyRect, 220, 30);


            sum = Graphs.FirstOrDefault(x => x.DataName == IDPC).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IDNC).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(230, -40, 420, -40); //trzecia linia
            canvas.DrawLine(420, -40, 420, 40);
            DrawDot(canvas, dirtyRect, 410, 30);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IVzN).Y[Index]);
            Source(canvas, dirtyRect, -10, 200, UVzN); //B
            canvas.DrawLine(dirtyRect.Left, 110, dirtyRect.Left, 190);//190

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NC).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NCz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 300, 240, dirtyRect.Left + 490, 240);

            sum += Graphs.FirstOrDefault(x => x.DataName == IS_NB).Y[Index] +
                   Graphs.FirstOrDefault(x => x.DataName == ID_NBz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 120, 240, dirtyRect.Left + 300, 240);
            DrawDot(canvas, dirtyRect, 290, 230);

            sum += Graphs.FirstOrDefault(x => x.DataName == IS_NA).Y[Index] +
                   Graphs.FirstOrDefault(x => x.DataName == ID_NAz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 110, 230);
            canvas.DrawLine(dirtyRect.Left, 230, dirtyRect.Left, 240);
            canvas.DrawLine(dirtyRect.Left, 240, dirtyRect.Left + 120, 240);

        }
        private void FirstLine(ICanvas canvas, RectF dirtyRect) {

            canvas.Font = Font.DefaultBold;
            canvas.DrawString(A, 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;


            //gora
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IDPA).Y[Index]);
            canvas.DrawLine(40, 40, 40, 110);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, DPA);
            canvas.DrawLine(70, 40, 110, 40);
            float sum = Graphs.FirstOrDefault(x => x.DataName == IDPA).Y[Index] +
              Graphs.FirstOrDefault(x => x.DataName == IS_PA).Y[Index] +
              Graphs.FirstOrDefault(x => x.DataName == ID_PAz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 30);

            sum = Graphs.FirstOrDefault(x => x.DataName == IDPA).Y[Index] +
                          Graphs.FirstOrDefault(x => x.DataName == IDNA).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 30, 100);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_PAz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, D_PAz);
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_PA).Y[Index] +
               Graphs.FirstOrDefault(x => x.DataName == ID_PAz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            DrawDot(canvas, dirtyRect, 100, -20);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_PA).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_PA).Y[Index], -40, 0, S_PA);

            sum = Graphs.FirstOrDefault(x => x.DataName == ID_PMAz).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IS_PMA).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            DrawDot(canvas, dirtyRect, 100, 40);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_PMAz).Y[Index]); //odnoga dla diody 
            DiodeTop(canvas, dirtyRect, 120, 60, D_PMAz);
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_PMA).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_PMA).Y[Index], -40, 60, S_PMA);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);
            sum = Graphs.FirstOrDefault(x => x.DataName == IS_PMA).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_PMAz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 90);


            //dol
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IDNA).Y[Index]);
            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, DNA);
            canvas.DrawLine(60, 180, 110, 180);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_NMAz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, D_NMAz);
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);

            sum = Graphs.FirstOrDefault(x => x.DataName == ID_NMAz).Y[Index] + Graphs.FirstOrDefault(x => x.DataName == IS_NMA).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NMA).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NMAz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 110);
            DrawDot(canvas, dirtyRect, 100, 160);
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_NMA).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_NMA).Y[Index], -40, 130, S_NMA);
            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NMA).Y[Index] + Graphs.FirstOrDefault(x => x.DataName == ID_NAz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 180);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_NAz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, D_NAz);
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_NA).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 110, 190);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_NA).Y[Index], -40, 190, S_NA);
            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NA).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NAz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 220);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IVoA).Y[Index]);
            canvas.DrawLine(110, 110, 200, 110);
            canvas.DrawLine(200, 110, 200, 250);

            DrawDot(canvas, dirtyRect, 100, 100);
        }
        private void SecondLine(ICanvas canvas, RectF dirtyRect) {

            float sum = Graphs.FirstOrDefault(x => x.DataName == IS_PB).Y[Index] +
                        Graphs.FirstOrDefault(x => x.DataName == ID_PBz).Y[Index] +
                        Graphs.FirstOrDefault(x => x.DataName == IS_PC).Y[Index] +
                        Graphs.FirstOrDefault(x => x.DataName == ID_PCz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, -30);
            canvas.DrawLine(dirtyRect.Left - 70, -20, dirtyRect.Left + 110, -20);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString(B, 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;


            //gora
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IDPB).Y[Index]);

            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, DPB);
            canvas.DrawLine(70, 40, 110, 40);

            sum = Graphs.FirstOrDefault(x => x.DataName == IDPB).Y[Index] +
                    Graphs.FirstOrDefault(x => x.DataName == IS_PB).Y[Index] +
                    Graphs.FirstOrDefault(x => x.DataName == ID_PBz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 30);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_PBz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, D_PBz);
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_PB).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == ID_PBz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            DrawDot(canvas, dirtyRect, 100, -20);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_PB).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_PB).Y[Index], -40, 0, S_PB);

            sum = Graphs.FirstOrDefault(x => x.DataName == ID_PMBz).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IS_PMB).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            DrawDot(canvas, dirtyRect, 100, 40);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_PMBz).Y[Index]); //odnoga dla diody 
            DiodeTop(canvas, dirtyRect, 120, 60, D_PMBz);
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_PMB).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_PMB).Y[Index], -40, 60, S_PMB);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);
            sum = Graphs.FirstOrDefault(x => x.DataName == IS_PMB).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_PMBz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 90);

            //dol
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IDNB).Y[Index]);
            canvas.DrawLine(40, 40, 40, 110);

            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, DNB);
            canvas.DrawLine(60, 180, 110, 180);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_NMBz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, D_NMBz);
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);

            sum = Graphs.FirstOrDefault(x => x.DataName == ID_NBz).Y[Index]
                + Graphs.FirstOrDefault(x => x.DataName == IS_NMB).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NMB).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NMBz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 110);
            DrawDot(canvas, dirtyRect, 100, 160);
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_NMB).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_NMB).Y[Index], -40, 130, S_NMB);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 180);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NB).Y[Index] 
                + Graphs.FirstOrDefault(x => x.DataName == ID_NBz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 110, 190);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_NBz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, D_NBz);
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_NB).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_NB).Y[Index], -40, 190, S_NB);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NB).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NBz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);
            DrawDot(canvas, dirtyRect, 100, 220);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName ==  IVoB).Y[Index]);
            canvas.DrawLine(110, 110, 200, 110);
            canvas.DrawLine(200, 110, 200, 250);

            DrawDot(canvas, dirtyRect, 100, 100);

        }
        private void ThirdLine(ICanvas canvas, RectF dirtyRect) {

            float sum = Graphs.FirstOrDefault(x => x.DataName == IS_PC).Y[Index] +
                        Graphs.FirstOrDefault(x => x.DataName == ID_PCz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left - 80, -20, dirtyRect.Left + 110, -20);

            canvas.Font = Font.DefaultBold;
            canvas.DrawString(C, 100, 110, HorizontalAlignment.Right);
            canvas.Font = Font.Default;


            //gora
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IDPC).Y[Index]);
            canvas.DrawLine(40, 40, 60, 40);
            DiodeRight(canvas, dirtyRect, 60, 30, DPC);
            canvas.DrawLine(70, 40, 110, 40);

            sum = Graphs.FirstOrDefault(x => x.DataName == IDPC).Y[Index] +
                   Graphs.FirstOrDefault(x => x.DataName ==IS_PC).Y[Index] +
                   Graphs.FirstOrDefault(x => x.DataName ==ID_PCz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 30);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_PCz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, -10, dirtyRect.Left + 130, -10);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, -10, dirtyRect.Left + 130, 10);
            DiodeTop(canvas, dirtyRect, 120, 0, D_PCz);
            canvas.DrawLine(dirtyRect.Left + 130, 20, dirtyRect.Left + 130, 40);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 130, 40);

            sum = Graphs.FirstOrDefault(x => x.DataName ==IS_PC).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == ID_PCz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, -20, dirtyRect.Left + 110, 0);
            DrawDot(canvas, dirtyRect, 100, -20);


            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_PC).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_PC).Y[Index], -40, 0, S_PC);

            sum = Graphs.FirstOrDefault(x => x.DataName == ID_PMCz).Y[Index] +
                Graphs.FirstOrDefault(x => x.DataName == IS_PMC).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 40, dirtyRect.Left + 110, 60);
            DrawDot(canvas, dirtyRect, 100, 40);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_PMCz).Y[Index]); //odnoga dla diody 
            DiodeTop(canvas, dirtyRect, 120, 60, D_PMCz);
            canvas.DrawLine(dirtyRect.Left + 110, 50, dirtyRect.Left + 130, 50);
            canvas.DrawLine(dirtyRect.Left + 130, 50, dirtyRect.Left + 130, 80);
            canvas.DrawLine(dirtyRect.Left + 130, 80, dirtyRect.Left + 130, 100);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 130, 100);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_PMC).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_PMC).Y[Index], -40, 60, S_PMC);
            canvas.DrawLine(dirtyRect.Left + 110, 100, dirtyRect.Left + 110, 120);
            sum = Graphs.FirstOrDefault(x => x.DataName == IS_PMC).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_PMCz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 90);

            //dol
            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IDNC).Y[Index]);
            canvas.DrawLine(40, 40, 40, 110);

            canvas.DrawLine(40, 110, 40, 180);
            canvas.DrawLine(40, 180, 60, 180);
            DiodeLeft(canvas, dirtyRect, 50, 170, DNC);
            canvas.DrawLine(60, 180, 110, 180);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_NMCz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 120, dirtyRect.Left + 130, 120);  //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 120, dirtyRect.Left + 130, 140);
            DiodeTop(canvas, dirtyRect, 120, 130, D_NMCz);
            canvas.DrawLine(dirtyRect.Left + 130, 150, dirtyRect.Left + 130, 170);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 130, 170);

            sum = Graphs.FirstOrDefault(x => x.DataName == ID_NMCz).Y[Index] 
                + Graphs.FirstOrDefault(x => x.DataName == IS_NMC).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 110, dirtyRect.Left + 110, 130);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NMC).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NMCz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 110);
            DrawDot(canvas, dirtyRect, 100, 160);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_NMC).Y[Index]);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_NMC).Y[Index], -40, 130, S_NMC);
            canvas.DrawLine(dirtyRect.Left + 110, 170, dirtyRect.Left + 110, 180);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NC).Y[Index] 
                + Graphs.FirstOrDefault(x => x.DataName == ID_NCz).Y[Index];
            ReturnColor(canvas, sum);
            DrawDot(canvas, dirtyRect, 100, 170);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == ID_NCz).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 130, 180);   //odnoga dla diody 
            canvas.DrawLine(dirtyRect.Left + 130, 180, dirtyRect.Left + 130, 210);
            DiodeTop(canvas, dirtyRect, 120, 190, D_NCz);
            canvas.DrawLine(dirtyRect.Left + 130, 210, dirtyRect.Left + 130, 230);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 130, 230);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IS_NC).Y[Index]);
            canvas.DrawLine(dirtyRect.Left + 110, 180, dirtyRect.Left + 110, 190);
            Transistor(canvas, dirtyRect, Graphs.FirstOrDefault(x => x.DataName == IS_NC).Y[Index], -40, 190, S_NC);

            sum = Graphs.FirstOrDefault(x => x.DataName == IS_NC).Y[Index] +
                  Graphs.FirstOrDefault(x => x.DataName == ID_NCz).Y[Index];
            ReturnColor(canvas, sum);
            canvas.DrawLine(dirtyRect.Left + 110, 230, dirtyRect.Left + 110, 240);
            DrawDot(canvas, dirtyRect, 100, 220);

            ReturnColor(canvas, Graphs.FirstOrDefault(x => x.DataName == IVoC).Y[Index]);
            canvas.DrawLine(110, 110, 200, 110);
            canvas.DrawLine(200, 110, 200, 250);

            DrawDot(canvas, dirtyRect, 100, 100);

        }
        private void OdbLine(ICanvas canvas, RectF dirtyRect) {
            if (BlackWhite) {
                canvas.StrokeColor = Colors.White;
                canvas.FillColor = Colors.White;
            }
            else {
                canvas.StrokeColor = Colors.Black;
                canvas.FillColor = Colors.Black;
            }

            canvas.DrawRectangle(0, -10, 400, 35);
            canvas.FontSize = 28;
            canvas.DrawString(ODB, 200, 18, HorizontalAlignment.Center);
        }


        private Color ReturnColor(ICanvas canvas, float current) {
            Color c = new Color();

            if (Math.Abs(current) > stillOpen) {
                c = Colors.Red;
            }
            else {
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

        private void DiodeRight(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null) {
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
        private void DiodeLeft(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null) {
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
        private void DiodeTop(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null) {
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

        private void Transistor(ICanvas canvas, RectF dirtyRect, float current, int moveX = 0, int moveY = 0, string name = null) {
            canvas.SaveState();
            canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

            if (current > stillOpen) {
                canvas.DrawLine(dirtyRect.Left + 150, 0, 150, 40);
            }
            else {
                canvas.DrawLine(dirtyRect.Left + 150, 0, 140, 25);
                canvas.DrawLine(150, 40, 150, 30);
            }
            DrawName(canvas, dirtyRect, name, 160, 85);
            canvas.RestoreState();
        }
        private void Source(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0, string name = null) {
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

        private void DrawName(ICanvas canvas, RectF dirtyRect, ReadOnlySpan<char> name, int moveX = 0, int moveY = 0) {
            if (name.Length > 1) {

                canvas.Translate(dirtyRect.Left + moveX, dirtyRect.Top + moveY);

                var a = FirstChar(name).ToString().ToUpper();
                var b = RestChars(name).ToString().ToUpper();
                canvas.DrawString(a, -52, -75, HorizontalAlignment.Center);
                canvas.DrawString(b, -48, -70, HorizontalAlignment.Left);
            }
        }
        private ReadOnlySpan<char> FirstChar(ReadOnlySpan<char> value) {
            return value.Slice(0, 1);
        }
        private ReadOnlySpan<char> RestChars(ReadOnlySpan<char> value) {
            return value.Slice(1, value.Length - 1);
        }

        private void DrawDot(ICanvas canvas, RectF dirtyRect, int moveX = 0, int moveY = 0) {
            canvas.Translate(moveX, moveY);
            canvas.FillCircle(10, 10, 5);
            canvas.Translate(-moveX, -moveY);
        }

    }
}
