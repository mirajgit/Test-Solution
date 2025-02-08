﻿namespace Test.UI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int? Discount { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }

    }

    public class ProductForGraph
    {
        public string Category { get; set; }
        public int Products { get; set; }
    }
}
