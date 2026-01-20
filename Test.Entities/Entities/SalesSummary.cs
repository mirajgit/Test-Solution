using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class SalesSummary
{
    public long SaleId { get; set; }

    public string? InvoiceNo { get; set; }

    public string? CustomarName { get; set; }

    public DateTime? SaleDate { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Discount { get; set; }

    public decimal? GrandTotal { get; set; }

    public decimal? TotalQuantity { get; set; }

    public string? PaidStatus { get; set; }
}
