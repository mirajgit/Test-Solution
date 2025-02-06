using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class Tbl_HRM_UserInformation
{
    public int UserId { get; set; }

    public string LoginName { get; set; } = null!;

    public string? UserName { get; set; }

    public decimal? Salary { get; set; }

    public string? UserEid { get; set; }

    public string Password { get; set; } = null!;

    public bool Status { get; set; }
}
