using Microsoft.AspNetCore.Mvc.Rendering;

namespace Test.UI.Models
{
    public class PrinterSelectViewModel
    {
        public string? CNSelectedPrinter { get; set; }
        public string? BarcodeSelectedPrinter { get; set; }
        public List<SelectListItem> PrinterList { get; set; }
        public int? PrinterId { get; set; }

        public string AccessUser { get; set; } = null!;

        public string? CNPrinter { get; set; }

        public string? BarcodePrinter { get; set; }
        public string? PrinterType { get; set; }

        public string? HV { get; set; } = null!;

        public DateTime SelectedDate { get; set; }
    }
}
