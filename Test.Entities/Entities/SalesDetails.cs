using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class SalesDetails
{
    public long SalesDetailsId { get; set; }

    public long SalesId { get; set; }

    public string Item { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? Total { get; set; }
}
