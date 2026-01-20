using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class HRM_Employee
{
    public int EmployeeID { get; set; }

    public string? Name { get; set; }

    public string? Department { get; set; }

    public string? Status { get; set; }

    public string? Phone { get; set; }
}
