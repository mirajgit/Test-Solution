using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? Mobile { get; set; }
}
