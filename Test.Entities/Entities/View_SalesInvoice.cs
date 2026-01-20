using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class View_SalesInvoice
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

    public string BarcodeImage { get; set; } = null!;

    public long SalesDetailsId { get; set; }

    public long SalesId { get; set; }

    public string Item { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? Total { get; set; }
}
