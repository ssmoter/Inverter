
namespace Inverter.Models {
    public class LoggModel {
        public LoggModel(DateTime date, string error) {
            Date = date;
            Error = error;
        }

        public DateTime Date { get; set; }
        public string Error { get; set; }
    }
}
