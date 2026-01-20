using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class View_Cricket_CrecketerInformation
{
    public int CricketId { get; set; }

    public string CricketerName { get; set; } = null!;

    public string CricketerMobile { get; set; } = null!;

    public string CricketerAddress { get; set; } = null!;

    public int CricketerAge { get; set; }

    public DateOnly? DateOfBirth { get; set; }
}
