using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal Price { get; set; }

    public int? Discount { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }
}
