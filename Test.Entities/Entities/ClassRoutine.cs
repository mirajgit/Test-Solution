using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class ClassRoutine
{
    public int RoutingId { get; set; }

    public string TeacherName { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
}
