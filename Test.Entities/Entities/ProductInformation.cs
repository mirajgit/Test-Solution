using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class ProductInformation
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal Price { get; set; }
}
