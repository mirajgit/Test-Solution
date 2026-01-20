using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Tbl_HRM_CustomarInformation
{
    public int CustomarId { get; set; }

    public string UserName { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string Address { get; set; } = null!;
}
