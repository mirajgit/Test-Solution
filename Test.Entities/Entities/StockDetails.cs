using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class StockDetails
{
    public long StockDetailsId { get; set; }

    public long StockId { get; set; }

    public string Barcode { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}
