using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class ItemInformation
{
    public long ItemId { get; set; }

    public int AuthorId { get; set; }

    public string ItemName { get; set; } = null!;

    public int ItemCategory { get; set; }

    public int UnitId { get; set; }

    public decimal ItemPrice { get; set; }

    public decimal ItemDiscount { get; set; }

    public decimal ActualPrice { get; set; }

    public string Description { get; set; } = null!;
}
