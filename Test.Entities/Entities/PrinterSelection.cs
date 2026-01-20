using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class PrinterSelection
{
    public int PrinterId { get; set; }

    public string AccessUser { get; set; } = null!;

    public string? CNPrinter { get; set; }

    public string? BarcodePrinter { get; set; }

    public string HV { get; set; } = null!;

    public DateTime SelectedDate { get; set; }
}
