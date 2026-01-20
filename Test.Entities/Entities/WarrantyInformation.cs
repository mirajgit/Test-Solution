using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class WarrantyInformation
{
    public long WarrantyId { get; set; }

    public string ItemId { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public string UserEID { get; set; } = null!;

    public DateOnly WarrantyStartDate { get; set; }

    public DateOnly WarrantyEndDate { get; set; }
}
