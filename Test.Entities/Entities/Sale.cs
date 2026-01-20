using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Sale
{
    public int Id { get; set; }

    public string? Customer { get; set; }

    public decimal Amount { get; set; }

    public DateOnly? PurchasedOn { get; set; }
}
