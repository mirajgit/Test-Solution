using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class Tbl_HRM_EmployeeInformation
{
    public int EmpId { get; set; }

    public string Role { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public string EmpContact { get; set; } = null!;

    public string EmpAddress { get; set; } = null!;

    public bool Status { get; set; }
}
