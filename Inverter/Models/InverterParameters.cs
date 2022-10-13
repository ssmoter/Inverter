using System.Text;

namespace Inverter.Models
{
    public class InverterParameters
    {
        #region ParametryFalownika
        public double Uz { get; set; } = 100;
        public double Fo { get; set; } = 50;
        public double Ma { get; set; } = 0.5;
        public double R_on { get; set; } = 0.01;
        public double R_off { get; set; } = 100_000;
        public double Ro { get; set; } = 10;
        public double Lo { get; set; } = 0.003;
        public double Eo { get; set; } = 0;
        public double Fio { get; set; } = 0;
        public double Fi { get; set; } = 1500;
        public int Alfa { get; set; } = 18;
        public int K { get; set; } = 0; //1 lub 0
        public double pi
        {
            get => Math.PI;
        }
        public double Y
        {
            get => Alfa / 90;
        }
        #endregion

        #region Tran
        public string T1 { get; set; } = "100u";
        public string T2 { get; set; } = "60m";
        public string T3 { get; set; } = "20m";
        public string T4 { get; set; } = "10u";
        public string T5 { get; set; } = "UIC";
        public string T6 { get; set; } = "";
        #endregion

        #region Options


        #endregion

        #region Probe
        public string ExtraProbe { get; set; }
        #endregion

        #region Print Tran
        public string ExtraPrintTran { get; set; }
        public string[] DefaultPrintTran { get; set; } =
        {
           " I(VzP) "," I(VzN) "," I(V0) "," I(S_PA) "," I(S_PB) "," I(S_PC) "," I(S_PMA) ",
           " I(S_PMB) "," I(S_PMC) "," I(S_NMA) "," I(S_NMB) "," I(S_NMC) "," I(S_NA) ",
           " I(S_NB) "," I(S_NC) "," I(D_PAz) "," I(D_PBz) "," I(D_PCz) "," I(D_PMAz) ",
           " I(D_PMBz) "," I(D_PMCz) "," I(D_NMAz) "," I(D_NMBz) "," I(D_NMCz) "," I(D_NAz) ",
           " I(D_NBz) "," I(D_NCz) "," I(DPA) "," I(DPB) "," I(DPC) "," I(DNA) "," I(DNB) ",
            " I(DNC) "," I(RoA) "," I(RoB) "," I(RoC) ",
        };
        #endregion



        public InverterParameters()
        {
            StringModel = CreateNewModel();
        }

        #region StringModel


        public string StringModel { get; private set; }
        public string CreateNewModel()
        {
            StringBuilder sr = new();

            #region Param

            sr.Append(".PARAM Uz=");
            sr.AppendLine(Uz.ToString());
            sr.Append(".PARAM fo=");
            sr.AppendLine(Fo.ToString());
            sr.Append(".PARAM ma=");
            sr.AppendLine(Ma.ToString());
            sr.AppendLine(".PARAM UG_on=1");
            sr.AppendLine(".PARAM UG_off=0");
            sr.Append(".PARAM R_on=");
            sr.AppendLine(R_on.ToString());
            sr.Append(".PARAM R_off=");
            sr.AppendLine(R_off.ToString());
            sr.Append(".PARAM Ro=");
            sr.AppendLine(Ro.ToString());
            sr.Append(".PARAM Lo=");
            sr.AppendLine(Lo.ToString());
            sr.Append(".PARAM Eo=");
            sr.AppendLine(Eo.ToString());
            sr.Append(".PARAM fio=");
            sr.AppendLine(Fio.ToString());
            sr.Append(".PARAM fi=");
            sr.AppendLine(Fi.ToString());
            sr.Append(".PARAM alfa=");
            sr.AppendLine(Alfa.ToString());
            sr.Append(".PARAM k=");
            sr.AppendLine(K.ToString());
            sr.AppendLine(".PARAM y=alfa/90");
            sr.Append(".PARAM pi=");
            sr.AppendLine(Math.PI.ToString());

            #endregion

            #region Tran
            sr.AppendLine();
            sr.Append(".TRAN ");
            sr.Append(T1);
            sr.Append(" ");
            sr.Append(T2);
            sr.Append(" ");
            sr.Append(T3);
            sr.Append(" ");
            sr.Append(T4);
            sr.Append(" ");
            sr.Append(T5);
            sr.Append(" ");
            sr.Append(T6);
            sr.AppendLine();
            #endregion

            #region Options
            sr.AppendLine();
            sr.Append(".OPTIONS itl4=200  reltol=0.03  numdgt=16  node");
            #endregion

            #region Probe
            if (!string.IsNullOrEmpty(ExtraProbe))
            {
                sr.AppendLine();
                sr.Append(".PROBE ");
                sr.Append(ExtraProbe);
                sr.AppendLine();
            }
            #endregion

            #region Print Train
            sr.AppendLine();
            sr.Append(".PRINT TRAN ");
            for (int i = 0; i < DefaultPrintTran.Length; i++)
            {
                if (i % 5 == 0)
                {
                    sr.AppendLine();
                }
                sr.Append(DefaultPrintTran[i]);
            }
            if (!string.IsNullOrEmpty(ExtraPrintTran))
            {
                sr.Append(" ");
                sr.Append(ExtraPrintTran);
            }

            #endregion

            #region Model
            sr.AppendLine();
            sr.Append(".MODEL DIODA d BV=2k");
            sr.AppendLine();
            sr.Append(".MODEL  KLUCZ  VSWITCH  ROFF={R_off}  RON={R_on}  VOFF={UG_off}  VON={UG_on}");
            #endregion

            #region Zasilanie
            sr.AppendLine();
            sr.Append("VzP 1 0 {Uz/2}");
            sr.AppendLine();
            sr.Append("VzN 0 23 {Uz/2}");
            sr.AppendLine();
            sr.Append("V0 24 0 ");

            #endregion

            #region Sterowanie Falownika

            #region Przebiegi nosne

            sr.AppendLine();
            sr.Append("ViP    101    0   pulse(  0  1  0  {(0.5-0.5m)/fi}  {(0.5-0.5m)/fi}  {1m/fi}  {1/fi} )");
            sr.AppendLine();
            sr.Append("ViN    102    0   pulse( -1  0  0  {(0.5-0.5m)/fi}  {(0.5-0.5m)/fi}  {1m/fi}  {1/fi} )");

            #endregion

            #region Przebiegi Modulujące
            sr.AppendLine();
            sr.Append("EmA    103    0   VALUE  {ma*sin(2*pi*fo*TIME)}");
            sr.AppendLine();
            sr.Append("EmB    104    0   VALUE  {ma*sin(2*pi*fo*TIME-2*pi/3)}");
            sr.AppendLine();
            sr.Append("EmC    105    0   VALUE  {ma*sin(2*pi*fo*TIME+2*pi/3)}");
            #endregion

            #region Przebiegi pomocnicze dla fali prostokatnej napiecia wyjaciowego
            sr.AppendLine();
            sr.Append("V_A    118    0   pulse( -1  1   0          {(0.5-0.5m)/fo}  {(0.5-0.5m)/fo}  {1m/fo}  {1/fo} )");
            sr.AppendLine();
            sr.Append("V_B    119    0   pulse( -1  1  {1/(3*fo)}  {(0.5-0.5m)/fo}  {(0.5-0.5m)/fo}  {1m/fo}  {1/fo} )");
            sr.AppendLine();
            sr.Append("V_C    120    0   pulse( -1  1  {2/(3*fo)}  {(0.5-0.5m)/fo}  {(0.5-0.5m)/fo}  {1m/fo}  {1/fo} )");
            #endregion

            #region Przebiegi sterujace (bramkowe)
            sr.AppendLine();
            sr.Append("EGPA   106    0   VALUE  {k*(1+sgn(V(103,101)))/2+(1-k)*(1+sgn(V(118)-y))/2}");
            sr.AppendLine();
            sr.Append("EGPB   107    0   VALUE  {k*(1+sgn(V(104,101)))/2+(1-k)*(1+sgn(V(119)-y))/2}");
            sr.AppendLine();
            sr.Append("EGPC   108    0   VALUE  {k*(1+sgn(V(105,101)))/2+(1-k)*(1+sgn(V(120)-y))/2}");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("EGPMA  109    0   VALUE  {k*(1+sgn(V(103,102)))/2+(1-k)*(1+sgn(V(118)+y))/2}");
            sr.AppendLine();
            sr.Append("EGPMB  110    0   VALUE  {k*(1+sgn(V(104,102)))/2+(1-k)*(1+sgn(V(119)+y))/2}");
            sr.AppendLine();
            sr.Append("EGPMC  111    0   VALUE  {k*(1+sgn(V(105,102)))/2+(1-k)*(1+sgn(V(120)+y))/2}");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("EGNMA  112    0   VALUE  {k*(1+sgn(V(101,103)))/2+(1-k)*(1+sgn(y-V(118)))/2}");
            sr.AppendLine();
            sr.Append("EGNMB  113    0   VALUE  {k*(1+sgn(V(101,104)))/2+(1-k)*(1+sgn(y-V(119)))/2}");
            sr.AppendLine();
            sr.Append("EGNMC  114    0   VALUE  {k*(1+sgn(V(101,105)))/2+(1-k)*(1+sgn(y-V(120)))/2}");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("EGNA   115    0   VALUE  {k*(1+sgn(V(102,103)))/2+(1-k)*(1+sgn(-y-V(118)))/2}");
            sr.AppendLine();
            sr.Append("EGNB   116    0   VALUE  {k*(1+sgn(V(102,104)))/2+(1-k)*(1+sgn(-y-V(119)))/2}");
            sr.AppendLine();
            sr.Append("EGNC   117    0   VALUE  {k*(1+sgn(V(102,105)))/2+(1-k)*(1+sgn(-y-V(120)))/2}");
            sr.AppendLine();
            #endregion
            #endregion

            #region Falownik

            #region Tranzystory
            sr.AppendLine();
            sr.Append("S_PA     1    2    106   0   klucz");
            sr.AppendLine();
            sr.Append("S_PB     1    3    107   0   klucz");
            sr.AppendLine();
            sr.Append("S_PC     1    4    108   0   klucz\r\n");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("S_PMA    5    8    109   0   klucz");
            sr.AppendLine();
            sr.Append("S_PMB    6    9    110   0   klucz");
            sr.AppendLine();
            sr.Append("S_PMC    7   10    111   0   klucz\r\n");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("S_NMA   11   14    112   0   klucz");
            sr.AppendLine();
            sr.Append("S_NMB   12   15    113   0   klucz");
            sr.AppendLine();
            sr.Append("S_NMC   13   16    114   0   klucz");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("S_NA    17   20    115   0   klucz");
            sr.AppendLine();
            sr.Append("S_NB    18   21    116   0   klucz");
            sr.AppendLine();
            sr.Append("S_NC    19   22    117   0   klucz");
            sr.AppendLine();

            #endregion

            #region Diody pomocnicze (szeregowe)

            sr.AppendLine();
            sr.Append("D_PA    2     5   dioda");
            sr.AppendLine();
            sr.Append("D_PB    3     6   dioda");
            sr.AppendLine();
            sr.Append("D_PC    4     7   dioda");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("D_PMA   8    11   dioda");
            sr.AppendLine();
            sr.Append("D_PMB   9    12   dioda");
            sr.AppendLine();
            sr.Append("D_PMC  10    13   dioda");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("D_NMA  14    17   dioda");
            sr.AppendLine();
            sr.Append("D_NMB  15    18   dioda");
            sr.AppendLine();
            sr.Append("D_NMC  16    19   dioda");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("D_NA   20    23   dioda");
            sr.AppendLine();
            sr.Append("D_NB   21    23   dioda");
            sr.AppendLine();
            sr.Append("D_NC   22    23   dioda");
            sr.AppendLine();
            #endregion

            #region Diody zwrotne

            sr.AppendLine();
            sr.Append("D_PAz   5     1   dioda");
            sr.AppendLine();
            sr.Append("D_PBz   6     1   dioda");
            sr.AppendLine();
            sr.Append("D_PCz   7     1   dioda");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("D_PMAz 11     5   dioda");
            sr.AppendLine();
            sr.Append("D_PMBz 12     6   dioda");
            sr.AppendLine();
            sr.Append("D_PMCz 13     7   dioda\r\n");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("D_NMAz 17    11   dioda");
            sr.AppendLine();
            sr.Append("D_NMBz 18    12   dioda");
            sr.AppendLine();
            sr.Append("D_NMCz 19    13   dioda");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("D_NAz  23    17   dioda");
            sr.AppendLine();
            sr.Append("D_NBz  23    18   dioda");
            sr.AppendLine();
            sr.Append("D_NCz  23    19   dioda");
            sr.AppendLine();
            #endregion

            #region Diody poziomujace

            sr.AppendLine();
            sr.Append("DPA    24     5   dioda");
            sr.AppendLine();
            sr.Append("DPB    24     6   dioda");
            sr.AppendLine();
            sr.Append("DPC    24     7   dioda");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("DNA    17    24   dioda");
            sr.AppendLine();
            sr.Append("DNB    18    24   dioda");
            sr.AppendLine();
            sr.Append("DNC    19    24   dioda");
            sr.AppendLine();
            #endregion

            #region odbiornik RLE

            sr.AppendLine();
            sr.Append("VoA    11    25   sin( 0  {Eo}  {fo}  {fio/(360*fo)} )");
            sr.AppendLine();
            sr.Append("VoB    12    26   sin( 0  {Eo}  {fo}  {1/(3*fo)+fio/(360*fo)} )");
            sr.AppendLine();
            sr.Append("VoC    13    27   sin( 0  {Eo}  {fo}  {2/(3*fo)+fio/(360*fo)} )");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("RoA    25    28   {Ro}");
            sr.AppendLine();
            sr.Append("RoB    26    29   {Ro}");
            sr.AppendLine();
            sr.Append("RoC    27    30   {Ro}\r\n");
            sr.AppendLine();

            sr.AppendLine();
            sr.Append("LoA    28    31   {Lo}");
            sr.AppendLine();
            sr.Append("LoB    29    31   {Lo}");
            sr.AppendLine();
            sr.Append("LoC    30    31   {Lo}");
            sr.AppendLine();
            #endregion

            #endregion

            sr.AppendLine();
            sr.Append(".END");

            StringModel = sr.ToString();
            return sr.ToString();
        }
        #endregion

    }
}
