namespace Inverter.Helpers {
    public static class MyEnums {
        public enum configName {
            PspicePath,
            FontSize,
            Scale,
            OptionsVisibility,
        }
    }
    public class AppConst {
        public static string Fourier { get => "fft"; }
    }
    public class Config {
        public static int FontSize { get; set; } = 20;
        public static string PspicePath { get; set; }
        public static bool OptionsVisibility { get; set; }

    }
}