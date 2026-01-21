using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class StockSummary
{
    public long StockId { get; set; }

    public string VendorName { get; set; } = null!;

    public string StockInvoice { get; set; } = null!;

    public DateTime? StockInDate { get; set; }

    public string? Remarks { get; set; }
}
