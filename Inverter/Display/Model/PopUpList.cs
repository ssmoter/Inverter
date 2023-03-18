namespace Inverter.Display.Model {
    public class PopUpList {
        public static char[] SignList = new char[] {
           Add,Sub,Multi,Div
        };

        public static char Add { get => '+'; }
        public static char Sub { get => '-'; }
        public static char Multi { get => '*'; }
        public static char Div { get => '/'; }

    }
}
