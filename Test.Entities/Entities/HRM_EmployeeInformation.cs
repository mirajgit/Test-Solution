using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class HRM_EmployeeInformation
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public int? DepartmentId { get; set; }

    public decimal? Salary { get; set; }

    public string? UserEid { get; set; }

    public string? EmployeeStatus { get; set; }

    public bool Status { get; set; }
}
