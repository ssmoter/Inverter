namespace Inverter.Helpers
{
    public static class MyEnums
    {
        public enum configName
        {
            PspicePath,
            FontSize,
        }
    }

    public class Config
    {
        public static int FontSize { get; set; }
        public static string PspicePath { get; set; }
    }
}