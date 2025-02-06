using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class AssetInformation
{
    public long AssetId { get; set; }

    public int ProjectId { get; set; }

    public int Department { get; set; }

    public string UserEID { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public string ItemStatus { get; set; } = null!;
}
