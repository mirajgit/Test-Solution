using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class Sale
{
    public int Id { get; set; }

    public string? Customer { get; set; }

    public decimal Amount { get; set; }

    public DateOnly? PurchasedOn { get; set; }
}
